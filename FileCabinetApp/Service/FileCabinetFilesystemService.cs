﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        public const string FirstName = "firstName";
        public const string LastName = "lastName";
        public const string DateOfBirth = "dateOfBirth";
        public const string Gender = "gender";
        public const string Experience = "experience";
        public const string Account = "account";

        public const int LengtOfString = 120;
        public const int RecordSize = 518;

        private const short isActiveRecord = 0;
        private const short isRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;

        private Dictionary<int, long> activeRecords;
        private Dictionary<int, long> removedRecords;

        private bool disposed;


        public IRecordValidator Validator { get; }

        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            if (validator is null)
            {
                throw new ArgumentNullException($"{nameof(validator)} cannot be null.");
            }

            if (fileStream is null)
            {
                throw new ArgumentNullException($"{nameof(fileStream)} cannot be null.");
            }

            this.Validator = validator;
            this.fileStream = fileStream;
            this.binReader = new BinaryReader(fileStream);
            this.binWriter = new BinaryWriter(fileStream);

            this.activeRecords = new Dictionary<int, long>();
            this.removedRecords = new Dictionary<int, long>();

            this.disposed = true;
        }

        public int CreateRecord(FileCabinetInputData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentException($"{nameof(parameters)} cannot be null.");
            }

            this.Validator.ValidateParameters(parameters);
            int? id = this.GetCurrentId();

            if (id is null)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} bigger than int.MaxValue.");
            }

            return this.CreateRecordWithId(parameters, id.Value);
        }

        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            if (parameters is null)
            {
                throw new ArgumentNullException($"{nameof(parameters)} cannot be null.");
            }

            this.Validator.ValidateParameters(parameters);

            if (!this.activeRecords.ContainsKey(id))
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.WriteRecordToBinaryFile(this.activeRecords[id], parameters, id);
        }

        public IEnumerable<FileCabinetRecord> FindBy(string propertyName, string value)
        {
            if (string.Equals(propertyName, FirstName, StringComparison.OrdinalIgnoreCase))
            {
                return this.FindByFirstName(value);
            }
            else if (string.Equals(propertyName, LastName, StringComparison.OrdinalIgnoreCase))
            {
                return this.FindByLastName(value);
            }
            else if (string.Equals(propertyName, DateOfBirth, StringComparison.OrdinalIgnoreCase))
            {
                return this.FindByDateOfBirth(Convert.ToDateTime(value));
            }
            else if (string.Equals(propertyName, Experience, StringComparison.OrdinalIgnoreCase))
            {
                return FindByExperience(value);
            }
            else if (string.Equals(propertyName, Account, StringComparison.OrdinalIgnoreCase))
            {
                return FindByAccount(value);
            }
            else if (string.Equals(propertyName, Gender, StringComparison.OrdinalIgnoreCase))
            {
                return FindByGender(value);
            }
            else
            {
                throw new ArgumentException($"This property {propertyName} is not exist.");
            }
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var records = this.GetRecordsCollection();

            foreach (var record in records)
            {
                if (record.DateOfBirth == dateOfBirth)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByExperience(string experience)
        {
            short exp = short.Parse(experience);

            foreach (var record in this.GetRecords())
            {
                if (record.Experience == exp)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByAccount(string account)
        {
            decimal acc = decimal.Parse(account);

            foreach (var record in this.GetRecords())
            {
                if (record.Account == acc)
                {
                    yield return record;
                }
            }
        }

        public IEnumerable<FileCabinetRecord> FindByGender(string gender)
        {
            foreach (var record in this.GetRecords())
            {
                if (record.Gender == gender[0])
                {
                    yield return record;
                }
            }
        }

        public bool Remove(int id)
        {
            if (!this.activeRecords.ContainsKey(id))
            {
                return false;
            }

            long position = this.activeRecords[id];
            this.binWriter.BaseStream.Position = position;
            this.binWriter.Write(isRemovedRecord);

            this.activeRecords.Remove(id);
            this.removedRecords.Add(id, position);
            return true;
        }

        public void Purge()
        {
            long currentPosition = 0;
            var activePositions = this.activeRecords.Values.ToArray();
            Array.Sort(activePositions);

            foreach (var activePosition in activePositions)
            {
                var record = this.ReadRecordOutBinaryFile(activePosition);

                int id = record.Id;
                var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

                this.WriteRecordToBinaryFile(currentPosition, data, id);
                currentPosition += RecordSize;
            }

            this.fileStream.SetLength(currentPosition);
            this.removedRecords.Clear();
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var record in this.GetRecordsCollection())
            {
                yield return record;
            }
        }

        public (int active, int removed) GetStat()
            => (this.activeRecords.Count, this.removedRecords.Count);

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            if (snapshot is null)
            {
                throw new ArgumentNullException($"{nameof(snapshot)} cannot be null.");
            }

            int count = 0;
            foreach (var record in snapshot.Records)
            {
                try
                {
                    int id = record.Id;
                    if (id <= 0)
                    {
                        throw new ArgumentOutOfRangeException($"{nameof(id)} must be positive.");
                    }

                    var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

                    if (this.GetRecordsCollection().Any(item => item.Id == record.Id))
                    {
                        this.EditRecord(record.Id, data);
                        count++;
                    }
                    else
                    {
                        this.CreateRecordWithId(data, record.Id);
                        count++;
                    }
                }
                catch (IndexOutOfRangeException indexOutOfRangeException)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed: {indexOutOfRangeException.Message}");
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed: {exception.Message}");
                }
            }

            return count;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.binReader.Close();
                this.binWriter.Close();
                this.fileStream.Close();
            }

            this.disposed = true;
        }

        private int CreateRecordWithId(FileCabinetInputData parameters, int id)
        {
            this.Validator.ValidateParameters(parameters);

            if (this.activeRecords.ContainsKey(id))
            {
                throw new ArgumentException($"#{nameof(id)} is not unique.");
            }

            long position = this.removedRecords.ContainsKey(id) ? this.removedRecords[id] : this.fileStream.Length;
            this.activeRecords.Add(id, position);
            this.removedRecords.Remove(id);
            this.WriteRecordToBinaryFile(position, parameters, id);

            return id;
        }

        private void WriteRecordToBinaryFile(long position, FileCabinetInputData parameters, int id)
        {
            this.binWriter.Seek((int)position, SeekOrigin.Begin);
            this.binWriter.Write(isActiveRecord);
            this.binWriter.Write(id);
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.FirstName, new string(' ', LengtOfString - parameters.FirstName.Length)).ToCharArray()));
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(parameters.LastName, new string(' ', LengtOfString - parameters.LastName.Length)).ToCharArray()));
            this.binWriter.Write(parameters.DateOfBirth.Month);
            this.binWriter.Write(parameters.DateOfBirth.Day);
            this.binWriter.Write(parameters.DateOfBirth.Year);
            this.binWriter.Write(parameters.Experience);
            this.binWriter.Write(parameters.Account);
            this.binWriter.Write(Encoding.Unicode.GetBytes(parameters.Gender.ToString(CultureInfo.InvariantCulture)));
        }

        private FileCabinetRecord ReadRecordOutBinaryFile(long position)
        {
            this.binReader.BaseStream.Position = position;
            short readKey = this.binReader.ReadInt16();

            var record = new FileCabinetRecord()
            {
                Id = this.binReader.ReadInt32(),
                FirstName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                LastName = Encoding.Unicode.GetString(this.binReader.ReadBytes(LengtOfString * 2)).Trim(),
                DateOfBirth = DateTime.Parse($"{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}/{this.binReader.ReadInt32()}", CultureInfo.InvariantCulture),
                Experience = this.binReader.ReadInt16(),
                Account = this.binReader.ReadDecimal(),
                Gender = Encoding.Unicode.GetString(this.binReader.ReadBytes(sizeof(char))).First(),
            };

            return record;
        }

        private List<FileCabinetRecord> GetRecordsCollection()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            var activePositions = this.activeRecords.Values.ToArray();
            Array.Sort(activePositions);

            foreach (var activePosition in activePositions)
            {
                var record = this.ReadRecordOutBinaryFile(activePosition);
                records.Add(record);
            }

            return records;
        }

        private int? GetCurrentId()
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                if (!this.activeRecords.ContainsKey(i))
                {
                    return i;
                }
            }

            return null;
        }
    }
}