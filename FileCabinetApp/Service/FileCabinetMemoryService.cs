using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FileCabinetApp.Interfaces;
using FileCabinetApp.Service;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetService contain methods for working with FileCabinetRecord.
    /// </summary>
    public class FileCabinetMemoryService : IFileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listFirstName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listLastName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listDateOfBirth = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private ReadOnlyCollection<FileCabinetRecord> records;

        public FileCabinetMemoryService() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        /// <param name="validator">type of validation input data.</param>
        public FileCabinetMemoryService(IRecordValidator validator)
        {
            if (validator is null)
            {
                throw new ArgumentException($"{nameof(validator)} cannot be null.");
            }

            this.Validator = validator;
        }

        public IRecordValidator Validator { get; }

        /// <summary>
        /// Method get data and create record.
        /// </summary>
        /// <param name="inputData">input data.</param>
        /// <returns>id of new record.</returns>
        public int CreateRecord(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException(nameof(inputData), "must not be null");
            }

            this.Validator.ValidateParameters(inputData);
            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = inputData.FirstName,
                LastName = inputData.LastName,
                DateOfBirth = inputData.DateOfBirth,
                Gender = inputData.Gender,
                Experience = inputData.Experience,
                Account = inputData.Account,
            };

            this.list.Add(record);
            this.listFirstName.Add(record);
            this.listLastName.Add(record);
            this.listDateOfBirth.Add(record);

            this.firstNameDictionary.Add(inputData.FirstName, this.listFirstName);
            this.lastNameDictionary.Add(inputData.LastName, this.listLastName);
            this.dateOfBirthDictionary.Add(inputData.DateOfBirth, this.listDateOfBirth);

            return record.Id;
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
                throw new ArgumentNullException(nameof(inputData), "must not be null");
            }

            this.Validator.ValidateParameters(inputData);
            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = inputData.FirstName,
                LastName = inputData.LastName,
                DateOfBirth = inputData.DateOfBirth,
                Gender = inputData.Gender,
                Experience = inputData.Experience,
                Account = inputData.Account,
            };
            this.list[id - 1] = record;

            this.listFirstName[id - 1] = this.list[id - 1];
            this.listLastName[id - 1] = this.list[id - 1];
            this.listDateOfBirth[id - 1] = this.list[id - 1];

            this.firstNameDictionary[inputData.FirstName] = this.listFirstName;
            this.lastNameDictionary[inputData.LastName] = this.listLastName;
            this.dateOfBirthDictionary[inputData.DateOfBirth] = this.listDateOfBirth;
        }

        /// <summary>
        /// Method find record by input first name.
        /// </summary>
        /// <param name="firstName">input first name.</param>
        /// <returns>all records whose first name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.firstNameDictionary.TryGetValue(firstName, out result);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(result);
            return this.records;
        }

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.lastNameDictionary.TryGetValue(lastName, out result);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(result);
            return this.records;
        }

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out result);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(result);
            return this.records;
        }

        public bool Remove(int id)
        {
            if (id > this.list.Count)
            {
                return false;
            }

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName.ToUpper()].Remove(record);
                    this.lastNameDictionary[record.LastName.ToUpper()].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    return true;
                }
            }

            return false;
        }

        public void Purge()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method return all records.
        /// </summary>
        /// <returns>all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.list);
        }

        /// <summary>
        /// Method return count of records.
        /// </summary>
        /// <returns>count of records.</returns>
        public (int real, int removed) GetStat()
        {
            return (this.list.Count, 0);
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
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

                    if (id <= this.list.Count)
                    {
                        var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.list.Add(record);

                        if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper()))
                        {
                            this.firstNameDictionary[record.FirstName.ToUpper()].Add(record);
                        }
                        else
                        {
                            this.firstNameDictionary.Add(record.FirstName.ToUpper(), new List<FileCabinetRecord> { record });
                        }

                        if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper()))
                        {
                            this.lastNameDictionary[record.LastName.ToUpper()].Add(record);
                        }
                        else
                        {
                            this.lastNameDictionary.Add(record.LastName.ToUpper(), new List<FileCabinetRecord> { record });
                        }

                        if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
                        {
                            this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
                        }
                        else
                        {
                            this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
                        }

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
    }
}