using FileCabinetApp.Validators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.IO;
using System.Text;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        public const int LengtOfString = 120;
        public const int RecordSize = 518;
        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;
        private int position;
        private bool disposed;
        private int id;

        public FileCabinetFilesystemService(FileStream fileStream)
            : this(new DefaultValidator(), fileStream)
        {
        }

        public FileCabinetFilesystemService(IRecordValidator validator)
        {
            this.Validator = validator;
        }

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
            this.disposed = true;
            this.position = 0;
            this.id = 1;
        }

        public int CreateRecord(FileCabinetInputData inputData)
        {
            this.Validator.ValidateParameters(inputData);
            this.WriteRecordToBinaryFile(this.position, inputData, this.id);
            this.position += RecordSize;
            return this.id++;
        }

        public void EditRecord(int id, FileCabinetInputData inputData)
        {
            if (id > this.id)
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            int position = (id - 1) * RecordSize;
            this.WriteRecordToBinaryFile(position, inputData, id);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var records = this.GetRecordsCollection();
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
            var records = this.GetRecordsCollection();
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

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
            =>
            new ReadOnlyCollection<FileCabinetRecord>(this.GetRecordsCollection());

        public int GetStat()
            => this.GetRecordsCollection().Count;

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

                    int countRecords = this.GetStat();
                    int size = countRecords;
                    var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

                    if (id <= countRecords)
                    {
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.WriteRecordToBinaryFile(RecordSize * size++, data, id);
                        this.position += RecordSize;
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

        private void WriteRecordToBinaryFile(int position, FileCabinetInputData inputData, int id)
        {
            this.binWriter.Seek(position, SeekOrigin.Begin);
            this.binWriter.Write((short)0);
            this.binWriter.Write(id);
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(inputData.FirstName, new string(' ', LengtOfString - inputData.FirstName.Length)).ToCharArray()));
            this.binWriter.Write(Encoding.Unicode.GetBytes(string.Concat(inputData.LastName, new string(' ', LengtOfString - inputData.LastName.Length)).ToCharArray()));
            this.binWriter.Write(inputData.DateOfBirth.Month);
            this.binWriter.Write(inputData.DateOfBirth.Day);
            this.binWriter.Write(inputData.DateOfBirth.Year);
            this.binWriter.Write(inputData.Experience);
            this.binWriter.Write(inputData.Account);
            this.binWriter.Write(Encoding.Unicode.GetBytes(inputData.Gender.ToString(CultureInfo.InvariantCulture)));
        }

        private FileCabinetRecord ReadRecordOutBinaryFile(long position)
        {
            this.binReader.BaseStream.Position = position;
            this.binReader.ReadInt16();

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

            for (int i = 0; i < this.position; i += RecordSize)
            {
                var record = this.ReadRecordOutBinaryFile(i);
                records.Add(record);
            }

            return records;
        }
    }
}
