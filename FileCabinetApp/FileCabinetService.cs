using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetService contain methods for working with FileCabinetRecord.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listFirstName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listLastName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listDateOfBirth = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

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

            this.CreateValidator().ValidateParameters(inputData);
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

            this.CreateValidator().ValidateParameters(inputData);
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
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.firstNameDictionary.TryGetValue(firstName, out result);

            return result.ToArray();
        }

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.lastNameDictionary.TryGetValue(lastName, out result);

            return result.ToArray();
        }

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out result);

            return result.ToArray();
        }

        /// <summary>
        /// Method return all records.
        /// </summary>
        /// <returns>all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Method return count of records.
        /// </summary>
        /// <returns>count of records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }

        /// <summary>
        /// Validate input data.
        /// </summary>
        protected abstract IRecordValidator CreateValidator();
    }
}