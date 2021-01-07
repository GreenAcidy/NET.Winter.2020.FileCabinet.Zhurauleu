using System;
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
        public const int LengtOfString = 120;
        public const int RecordSize = 518;

        private const short isRealRecord = 0;
        private const short isRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;

        private int cursor;
        private int currentID;

        private bool disposed;

        private Dictionary<int, bool> records;
        private Dictionary<int, int> idPositions;

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
            this.records = new Dictionary<int, bool>();
            this.idPositions = new Dictionary<int, int>();
            this.disposed = true;
            this.cursor = 0;
            this.currentID = 1;
        }

        public int CreateRecord(FileCabinetInputData parameters)
        {
            return this.CreateRecordWithId(parameters, this.currentID++);
        }

        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            if (!this.records.ContainsKey(id) || this.records[id] == false)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.WriteRecordToBinaryFile(this.idPositions[id], parameters, id);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            foreach (var record in records)
            {
                if (string.Equals(record.FirstName, firstName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            var records = GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            foreach (var record in records)
            {
                if (string.Equals(record.LastName, lastName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var records = this.GetRecordsCollection();
            var result = new List<FileCabinetRecord>();

            var key = new DateTime(dateOfBirth.Year, dateOfBirth.Month, dateOfBirth.Day);

            foreach (var record in records)
            {
                if (record.DateOfBirth == key)
                {
                    result.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(result);
        }

        public bool Remove(int id)
        {
            if (!this.records.ContainsKey(id))
            {
                return false;
            }

            this.binWriter.BaseStream.Position = this.idPositions[id];
            this.binWriter.Write(isRemovedRecord);
            this.records[id] = false;
            this.binWriter.BaseStream.Position = this.cursor;
            return true;
        }

        public void Purge()
        {
            var collection = this.GetRecordsCollection();
            this.binWriter.BaseStream.Position = 0;
            this.records.Clear();
            this.cursor = 0;

            foreach (var record in collection)
            {
                var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

                this.WriteRecordToBinaryFile(this.cursor, data, record.Id);
                this.cursor += RecordSize;
                this.records.Add(record.Id, true);
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
            =>
            new ReadOnlyCollection<FileCabinetRecord>(this.GetRecordsCollection());

        public (int real, int removed) GetStat()
        {
            int removed = 0;
            int real = 0;

            foreach (var recrodId in this.records)
            {
                if (recrodId.Value == true)
                {
                    real++;
                    continue;
                }

                removed++;
            }

            return (real, removed);
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
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

                    if (this.records.ContainsKey(id))
                    {
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.CreateRecordWithId(data, id);
                        this.currentID = id + 1;
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
            this.WriteRecordToBinaryFile(this.cursor, parameters, id);
            this.idPositions[id] = this.cursor;
            this.cursor += RecordSize;
            this.records.Add(id, true);
            return id;
        }

        private void WriteRecordToBinaryFile(int position, FileCabinetInputData parameters, int id)
        {
            this.binWriter.Seek(position, SeekOrigin.Begin);
            this.binWriter.Write(isRealRecord);
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

        private FileCabinetRecord ReadRecordOutBinaryFile(long position, out bool removedKey)
        {
            this.binReader.BaseStream.Position = position;
            short readKey = this.binReader.ReadInt16();
            removedKey = readKey == isRemovedRecord;

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
            bool removedKey;
            for (int i = 0; i < this.records.Count * RecordSize; i += RecordSize)
            {
                var record = this.ReadRecordOutBinaryFile(i, out removedKey);
                if (!removedKey)
                {
                    records.Add(record);
                }
            }

            return records;
        }
    }
}