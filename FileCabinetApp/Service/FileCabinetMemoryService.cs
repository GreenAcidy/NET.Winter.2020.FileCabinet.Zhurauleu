using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Data;
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
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetMemoryService"/> class.
        /// </summary>
        public FileCabinetMemoryService()
        {
        }

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

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }

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

            if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
            }
            else
            {
                this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
            else
            {
                this.dateOfBirthDictionary.Add(record.DateOfBirth, new List<FileCabinetRecord> { record });
            }

            CashedData.ClearCashe();
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions)
        {
            foreach (var item in this.GetRecords())
            {
                if (conditions is null)
                {
                    throw new ArgumentNullException($"{nameof(conditions)} cannot be null.");
                }

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
            if (firstName is null)
            {
                throw new ArgumentNullException($"{nameof(firstName)} cannot be null.");
            }

            if (CashedData.FirstNameCashe.ContainsKey(firstName))
            {
                return CashedData.FirstNameCashe[firstName];
            }

            CashedData.FirstNameCashe.Add(firstName, this.FindFirstName(firstName));
            return this.FindFirstName(firstName);
        }

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            if (lastName is null)
            {
                throw new ArgumentNullException($"{nameof(lastName)} cannot be null.");
            }

            if (CashedData.LastNameCashe.ContainsKey(lastName))
            {
                return CashedData.FirstNameCashe[lastName];
            }

            CashedData.LastNameCashe.Add(lastName, this.FindLastName(lastName));
            return this.FindLastName(lastName);
        }

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (CashedData.DateOfBirthCashe.ContainsKey(dateOfBirth))
            {
                return CashedData.DateOfBirthCashe[dateOfBirth];
            }

            CashedData.DateOfBirthCashe.Add(dateOfBirth, this.FindDateOfBirth(dateOfBirth));
            return this.FindDateOfBirth(dateOfBirth);
        }

        /// <summary>
        /// Find by experience.
        /// </summary>
        /// <param name="experience">Experience.</param>
        /// <returns>The sequance of record.</returns>
        public IEnumerable<FileCabinetRecord> FindByExperience(string experience)
        {
            if (experience is null)
            {
                throw new ArgumentNullException($"{nameof(experience)} cannot be null.");
            }

            if (CashedData.ExperienceCashe.ContainsKey(experience))
            {
                return CashedData.ExperienceCashe[experience];
            }

            CashedData.ExperienceCashe.Add(experience, this.FindExperience(experience));
            return this.FindExperience(experience);
        }

        /// <summary>
        /// Find by account.
        /// </summary>
        /// <param name="account">Account.</param>
        /// <returns>The sequance of records.</returns>
        public IEnumerable<FileCabinetRecord> FindByAccount(string account)
        {
            if (account is null)
            {
                throw new ArgumentNullException($"{nameof(account)} cannot be null");
            }

            if (CashedData.AccountCashe.ContainsKey(account))
            {
                return CashedData.AccountCashe[account];
            }

            CashedData.AccountCashe.Add(account, this.FindAccount(account));
            return this.FindAccount(account);
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

            if (CashedData.GenderCashe.ContainsKey(gender))
            {
                return CashedData.GenderCashe[gender];
            }

            CashedData.GenderCashe.Add(gender, this.FindGender(gender));
            return this.FindGender(gender);
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>Is removed.</returns>
        public bool Remove(int id)
        {
            foreach (var record in this.list)
            {
                if (record.Id == id)
                {
                    this.list.Remove(record);
                    this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
                    this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Remove(record);
                    this.dateOfBirthDictionary[record.DateOfBirth].Remove(record);
                    CashedData.ClearCashe();
                    return true;
                }
            }

            CashedData.ClearCashe();
            return false;
        }

        /// <summary>
        /// Only for file service.
        /// </summary>
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

        /// <summary>
        /// Make snapshot.
        /// </summary>
        /// <returns>Snapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            return new FileCabinetServiceSnapshot(this.list.ToArray());
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

                    if (id <= this.list.Count)
                    {
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.CreateRecord(data);

                        if (this.firstNameDictionary.ContainsKey(record.FirstName.ToUpper(CultureInfo.InvariantCulture)))
                        {
                            this.firstNameDictionary[record.FirstName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
                        }
                        else
                        {
                            this.firstNameDictionary.Add(record.FirstName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
                        }

                        if (this.lastNameDictionary.ContainsKey(record.LastName.ToUpper(CultureInfo.InvariantCulture)))
                        {
                            this.lastNameDictionary[record.LastName.ToUpper(CultureInfo.InvariantCulture)].Add(record);
                        }
                        else
                        {
                            this.lastNameDictionary.Add(record.LastName.ToUpper(CultureInfo.InvariantCulture), new List<FileCabinetRecord> { record });
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
                catch (ArgumentException)
                {
                    Console.WriteLine($"Import record with id {record.Id} failed.");
                }
            }

            return count;
        }

        private IEnumerable<FileCabinetRecord> FindFirstName(string firstName)
        {
            if (this.firstNameDictionary.ContainsKey(firstName.ToUpper(CultureInfo.InvariantCulture)))
            {
                var collection = this.firstNameDictionary[firstName.ToUpper(CultureInfo.InvariantCulture)];

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

        private IEnumerable<FileCabinetRecord> FindLastName(string lastName)
        {
            if (this.lastNameDictionary.ContainsKey(lastName.ToUpper(CultureInfo.InvariantCulture)))
            {
                var collection = this.lastNameDictionary[lastName.ToUpper(CultureInfo.InvariantCulture)];

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

        private IEnumerable<FileCabinetRecord> FindDateOfBirth(DateTime dateOfBirth)
        {
            /*int month = int.Parse(dateOfBirth.Substring(0, 2), CultureInfo.InvariantCulture);
            int day = int.Parse(dateOfBirth.Substring(3, 2), CultureInfo.InvariantCulture);
            int year = int.Parse(dateOfBirth.Substring(6, 4), CultureInfo.InvariantCulture);

            var key = new DateTime(year, month, day);*/

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

        private IEnumerable<FileCabinetRecord> FindExperience(string experience)
        {
            short exp = short.Parse(experience, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Experience == exp)
                {
                    yield return record;
                }
            }
        }

        private IEnumerable<FileCabinetRecord> FindAccount(string account)
        {
            decimal acc = decimal.Parse(account, CultureInfo.InvariantCulture);

            foreach (var record in this.GetRecords())
            {
                if (record.Account == acc)
                {
                    yield return record;
                }
            }
        }

        private IEnumerable<FileCabinetRecord> FindGender(string gender)
        {
            foreach (var record in this.GetRecords())
            {
                if (record.Gender == gender[0])
                {
                    yield return record;
                }
            }
        }
    }
}