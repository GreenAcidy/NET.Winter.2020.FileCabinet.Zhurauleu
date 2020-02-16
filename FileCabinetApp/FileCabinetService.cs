﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listFirstName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listLastName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listDateOfBirth = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, char gender, short expirience, decimal account)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                if (firstName is null)
                {
                    throw new ArgumentNullException(nameof(firstName), "must not be null!");
                }
                else
                {
                    throw new ArgumentException("must contain at least two symbols except space");
                }
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must be shorter than 61 symbol and larger than 1 symbol");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                if (lastName is null)
                {
                    throw new ArgumentNullException(nameof(lastName), "must not be null!");
                }
                else
                {
                    throw new ArgumentException("must contain at least two symbols except space");
                }
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("First name must be shorter than 61 symbol and larger than 1 symbol");
            }

            DateTime date = new DateTime(1950, 01, 01);
            if (dateOfBirth > DateTime.Today || dateOfBirth < date)
            {
                throw new ArgumentException("Date of birth must be in range from 01-Jan-1950 to current day");
            }

            if (char.IsWhiteSpace(gender))
            {
                throw new ArgumentException("Must contain except space symbol");
            }

            if (expirience < 0)
            {
                throw new ArgumentException("Experience can not be negative");
            }

            if (account <= 0)
            {
                throw new ArgumentException("Account must be positive");
            }

            var record = new FileCabinetRecord
            {
                Id = this.list.Count + 1,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Experience = expirience,
                Account = account,
            };

            this.list.Add(record);
            this.listFirstName.Add(record);
            this.listLastName.Add(record);
            this.listDateOfBirth.Add(record);

            this.firstNameDictionary.Add(firstName, this.listFirstName);
            this.lastNameDictionary.Add(lastName, this.listLastName);
            this.dateOfBirthDictionary.Add(dateOfBirth, this.listDateOfBirth);

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, char gender, short expirience, decimal account)
        {
            if (this.list.Count < id)
            {
                throw new ArgumentException("Current id is not found");
            }

            if (string.IsNullOrWhiteSpace(firstName))
            {
                if (firstName is null)
                {
                    throw new ArgumentNullException(nameof(firstName), "must not be null!");
                }
                else
                {
                    throw new ArgumentException("must contain at least two symbols except space");
                }
            }

            if (firstName.Length < 2 || firstName.Length > 60)
            {
                throw new ArgumentException("First name must be shorter than 61 symbol and larger than 1 symbol");
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                if (firstName is null)
                {
                    throw new ArgumentNullException(nameof(lastName), "must not be null!");
                }
                else
                {
                    throw new ArgumentException("must contain at least two symbols except space");
                }
            }

            if (lastName.Length < 2 || lastName.Length > 60)
            {
                throw new ArgumentException("First name must be shorter than 61 symbol and larger than 1 symbol");
            }

            DateTime date = new DateTime(1950, 01, 01);
            if (dateOfBirth > DateTime.Today || dateOfBirth < date)
            {
                throw new ArgumentException("Date of birth must be in range from 01-Jan-1950 to current day");
            }

            if (char.IsWhiteSpace(gender))
            {
                throw new ArgumentException("Must contain except space symbol");
            }

            if (expirience < 0)
            {
                throw new ArgumentException("Experience can not be negative");
            }

            if (account <= 0)
            {
                throw new ArgumentException("Account must be positive");
            }

            var record = new FileCabinetRecord
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                Gender = gender,
                Experience = expirience,
                Account = account,
            };
            this.list[id - 1] = record;

            this.listFirstName[id - 1] = this.list[id - 1];
            this.listLastName[id - 1] = this.list[id - 1];
            this.listDateOfBirth[id - 1] = this.list[id - 1];

            this.firstNameDictionary[firstName] = this.listFirstName;
            this.lastNameDictionary[lastName] = this.listLastName;
            this.dateOfBirthDictionary[dateOfBirth] = this.listDateOfBirth;
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.firstNameDictionary.TryGetValue(firstName, out result);

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.lastNameDictionary.TryGetValue(lastName, out result);

            return result.ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.dateOfBirthDictionary.TryGetValue(dateOfBirth, out result);

            return result.ToArray();
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}