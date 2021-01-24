﻿using System;
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
        public const string FirstName = "firstName";
        public const string LastName = "lastName";
        public const string DateOfBirth = "dateOfBirth";
        public const string Gender = "gender";
        public const string Experience = "experience";
        public const string Account = "account";

        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listFirstName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listLastName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listDateOfBirth = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
        private IEnumerable<FileCabinetRecord> records;

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
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpper()))
            {
                var collection = this.firstNameDictionary[firstName.ToUpper()];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpper()))
            {
                var collection = this.lastNameDictionary[lastName.ToUpper()];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
            }
        }

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>

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

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                var collection = this.dateOfBirthDictionary[dateOfBirth];

                foreach (var item in collection)
                {
                    yield return item;
                }
            }
            else
            {
                yield break;
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
            if (id > this.list.Count)
            {
                return false;
            }

            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName/*.ToUpper()*/].Remove(record);
                    this.lastNameDictionary[record.LastName/*.ToUpper()*/].Remove(record);
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
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            foreach (var item in this.list)
            {
                yield return item;
            }
        }

        /// <summary>
        /// Method return count of records.
        /// </summary>
        /// <returns>count of records.</returns>
        public (int active, int removed) GetStat()
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