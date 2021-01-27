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
    /// <summary>
    /// Class FileCabinetFilesystemService implements IFileCabinetRecord interface.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const int LengtOfString = 120;
        private const int RecordSize = 518;

        private const short IsActiveRecord = 0;
        private const short IsRemovedRecord = 1;

        private readonly FileStream fileStream;
        private readonly BinaryReader binReader;
        private readonly BinaryWriter binWriter;

        private Dictionary<int, long> activeRecords;
        private Dictionary<int, long> removedRecords;

        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="validator">The validator.</param>
        /// <param name="fileStream">The filestream.</param>
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

        private IRecordValidator Validator { get; }

        /// <summary>
        /// Method get data and create record.
        /// </summary>
        /// <param name="inputData">input data.</param>
        /// <returns>id of new record.</returns>
        public int CreateRecord(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentException($"{nameof(inputData)} cannot be null.");
            }

            this.Validator.ValidateParameters(inputData);
            int? id = this.GetCurrentId();

            if (id is null)
            {
                throw new ArgumentOutOfRangeException($"{nameof(id)} bigger than int.MaxValue.");
            }

            return this.CreateRecordWithId(inputData, id.Value);
        }

        /// <summary>
        /// Method get data and edit existing record.
        /// </summary>
        /// <param name="id">input id of existing record.</param>
        /// <param name="inputData">input data.</param>
        public void EditRecord(int id, FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            this.Validator.ValidateParameters(inputData);

            if (!this.activeRecords.ContainsKey(id))
            {
                throw new ArgumentException($"Element with #{nameof(id)} can't fine in this records list.");
            }

            this.WriteRecordToBinaryFile(this.activeRecords[id], inputData, id);
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException($"{nameof(conditions)} cannot be null.");
            }

            foreach (var item in this.GetRecords())
            {
                bool isMath = true;
                if (conditions.FirstName != null)
                {
                    isMath = conditions.FirstName == item.FirstName && isMath;
                }

                if (conditions.LastName != null)
                {
                    isMath = conditions.LastName == item.LastName && isMath;
                }

                if (conditions.DateOfBirth != null)
                {
                    isMath = conditions.DateOfBirth == item.DateOfBirth && isMath;
                }

                if (conditions.Experience != null)
                {
                    isMath = conditions.Experience == item.Experience && isMath;
                }

                if (conditions.Account != null)
                {
                    isMath = conditions.Account == item.Account && isMath;
                }

                if (conditions.Gender != null)
                {
                    isMath = conditions.Gender == item.Gender && isMath;
                }

                if (isMath)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByOr(WhereConditions conditions)
        {
            if (conditions is null)
            {
                throw new ArgumentNullException($"{nameof(conditions)} cannot be null.");
            }

            foreach (var item in this.GetRecords())
            {
                bool isMath = false;
                if (conditions.FirstName != null)
                {
                    isMath = conditions.FirstName == item.FirstName || isMath;
                }

                if (conditions.LastName != null)
                {
                    isMath = conditions.LastName == item.LastName || isMath;
                }

                if (conditions.DateOfBirth != null)
                {
                    isMath = conditions.DateOfBirth == item.DateOfBirth || isMath;
                }

                if (conditions.Experience != null)
                {
                    isMath = conditions.Experience == item.Experience || isMath;
                }

                if (conditions.Account != null)
                {
                    isMath = conditions.Account == item.Account || isMath;
                }

                if (conditions.Gender != null)
                {
                    isMath = conditions.Gender == item.Gender || isMath;
                }

                if (isMath)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Method find record by input first name.
        /// </summary>
        /// <param name="firstName">input first name.</param>
        /// <returns>all records whose first name matches the incoming.</returns>
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

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
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

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>
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

        /// <summary>
        /// Find by experience.
        /// </summary>
        /// <param name="experience">Experience.</param>
        /// <returns>The sequance of record.</returns>
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

        /// <summary>
        /// Find by account.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <returns>The sequance of records.</returns>
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

        /// <summary>
        /// Find by english level.
        /// </summary>
        /// <param name="gender">Gender.</param>
        /// <returns>the sequance of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByGender(string gender)
        {
            if (gender is null)
            {
                throw new ArgumentNullException($"{nameof(gender)} cannot be null.");
            }

            foreach (var record in this.GetRecords())
            {
                if (record.Gender == gender[0])
                {
                    yield return record;
                }
            }
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Is removed.</returns>
        public bool Remove(int id)
        {
            if (!this.activeRecords.ContainsKey(id))
            {
                return false;
            }

            long position = this.activeRecords[id];
            this.binWriter.BaseStream.Position = position;
            this.binWriter.Write(IsRemovedRecord);

            this.activeRecords.Remove(id);
            this.removedRecords.Add(id, position);
            return true;
        }

        /// <summary>
        /// Remove deleted record from file.
        /// </summary>
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

        /// <summary>
        /// Method return all records.
        /// </summary>
        /// <returns>all records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var record in this.GetRecordsCollection())
            {
                yield return record;
            }
        }

        /// <summary>
        /// Method return count of records.
        /// </summary>
        /// <returns>count of records.</returns>
        public (int active, int removed) GetStat()
            => (this.activeRecords.Count, this.removedRecords.Count);

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.GetRecords().ToArray());
        }

        /// <summary>
        /// Recovers saved snapshot recordings.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <returns>count of restored recordings.</returns>
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
            }

            return count;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
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
            this.binWriter.Write(IsActiveRecord);
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