using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class Service logger.
    /// </summary>
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly StreamWriter writer;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLogger"/> class.
        /// </summary>
        /// <param name="service">The current service.</param>
        public ServiceLogger(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException($"{nameof(service)} cannot be null.");
            }

            this.service = service;

            string path = @"logData.txt";
            /*E:\GitRepositories\NET.Winter.2020.FileCabinet.Zhurauleu\*/
            var stream = File.Exists(path) ? File.OpenWrite(path) : File.Create(path);
            this.writer = new StreamWriter(stream);
        }

        /// <summary>
        /// Generates a unique user record.
        /// </summary>
        /// <param name="parameters">Parameters of new record.</param>
        /// <returns>Id of record.</returns>
        public int CreateRecord(FileCabinetInputData parameters)
        {
            int id = this.service.CreateRecord(parameters);
            this.WriteLogInFile(nameof(this.CreateRecord), this.GetInfoFileCabinetInputData(parameters));
            this.WriteLogReturnInFile<int>(nameof(this.service.CreateRecord), id);
            return id;
        }

        /// <summary>
        /// Changes the record by given ID.
        /// </summary>
        /// <param name="id">Id of record.</param>
        /// <param name="parameters">Parameters of record.</param>
        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            this.service.EditRecord(id, parameters);
            this.WriteLogInFile(nameof(this.service.EditRecord), this.GetInfoFileCabinetInputData(parameters));
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions)
        {
            var collection = this.service.FindByAnd(conditions);
            this.WriteLogInFile(nameof(this.service.FindByAnd), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByAnd), conditions.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records, who is mathes the conditions.
        /// </summary>
        /// <param name="conditions">Find condtions.</param>
        /// <returns>Records sequance.</returns>
        public IEnumerable<FileCabinetRecord> FindByOr(WhereConditions conditions)
        {
            var collection = this.service.FindByOr(conditions);
            this.WriteLogInFile(nameof(this.service.FindByOr), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByOr), conditions.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given firstName.
        /// </summary>
        /// <param name="firstName">User firstName.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var collection = this.service.FindByFirstName(firstName);
            this.WriteLogInFile(nameof(this.service.FindByFirstName), firstName);
            this.WriteLogReturnInFile(nameof(this.service.FindByFirstName), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given lastName.
        /// </summary>
        /// <param name="lastName">User lastNeme.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var collection = this.service.FindByLastName(lastName);
            this.WriteLogInFile(nameof(this.service.FindByLastName), lastName);
            this.WriteLogReturnInFile(nameof(this.service.FindByLastName), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given date of birth.
        /// </summary>
        /// <param name="dateOfBirth">The user's date of birth.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var collection = this.service.FindByDateOfBirth(dateOfBirth);

            this.WriteLogInFile(nameof(this.service.FindByDateOfBirth),  dateOfBirth.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByDateOfBirth), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given gender.
        /// </summary>
        /// <param name="gender">The user's gender.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByGender(string gender)
        {
            var collection = this.service.FindByGender(gender);
            this.WriteLogInFile(nameof(this.service.FindByGender), gender);
            this.WriteLogReturnInFile(nameof(this.service.FindByGender), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given experience.
        /// </summary>
        /// <param name="experience">The user's experience.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByExperience(string experience)
        {
            var collection = this.service.FindByExperience(experience);
            this.WriteLogInFile(nameof(this.service.FindByExperience), experience);
            this.WriteLogReturnInFile(nameof(this.service.FindByExperience), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Find all records with given account.
        /// </summary>
        /// <param name="account">The user's account.</param>
        /// <returns>The sequence of founded records.</returns>
        public IEnumerable<FileCabinetRecord> FindByAccount(string account)
        {
            var collection = this.service.FindByAccount(account);
            this.WriteLogInFile(nameof(this.service.FindByAccount), account);
            this.WriteLogReturnInFile(nameof(this.service.FindByAccount), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Remove record with given id.
        /// </summary>
        /// <param name="id">The id of removed record.</param>
        /// <returns>Is removed record.</returns>
        public bool Remove(int id)
        {
            bool isRemoved = this.service.Remove(id);
            this.WriteLogInFile(nameof(this.service.Remove), id.ToString(CultureInfo.InvariantCulture));
            this.WriteLogReturnInFile(nameof(this.service.Remove), isRemoved.ToString());
            return isRemoved;
        }

        /// <summary>
        /// Deleted all removed record from file.
        /// </summary>
        public void Purge()
        {
            this.service.Purge();
            this.WriteLogInFile(nameof(this.service.Purge), string.Empty);
        }

        /// <summary>
        /// Give all records.
        /// </summary>
        /// <returns>The array of all records.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            var collection = this.service.GetRecords();
            this.WriteLogInFile(nameof(this.service.GetRecords), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetRecords), collection.ToString());
            return collection;
        }

        /// <summary>
        /// Give the count of records.
        /// </summary>
        /// <returns>The count of records.</returns>
        public (int active, int removed) GetStat()
        {
            var stat = this.service.GetStat();
            this.WriteLogInFile(nameof(this.service.GetStat), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetStat), stat.ToString());
            return stat;
        }

        /// <summary>
        /// Make snapshot of the current service.
        /// </summary>
        /// <returns>Snapshot of the current service.</returns>
        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            var snapshot = this.service.MakeSnapShot();
            this.WriteLogInFile(nameof(this.service.MakeSnapShot), string.Empty);
            return snapshot;
        }

        /// <summary>
        /// Recovers saved snapshot recordings.
        /// </summary>
        /// <param name="snapshot">Snapshot.</param>
        /// <returns>count of recovered records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int count = this.service.Restore(snapshot);
            this.WriteLogInFile(nameof(this.service.Restore), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.Restore), count.ToString(CultureInfo.InvariantCulture));
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
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.writer.Dispose();
            }

            this.disposed = true;
        }

        private void WriteLogInFile(string methodName, string info)
        {
            this.writer.WriteLine($"{DateTime.UtcNow} - Calling {methodName}() with {info}");
            this.writer.Flush();
        }

        private void WriteLogReturnInFile<T>(string methodName, T value)
        {
            this.writer.WriteLine($"{DateTime.UtcNow} - {methodName} return {value}");
            this.writer.Flush();
        }

        private string GetInfoFileCabinetInputData(FileCabinetInputData parameters)
            => $"FirstName = '{parameters.FirstName}', LastName = '{parameters.LastName}', " +
                $"DateOfBirth = '{parameters.DateOfBirth}', Experience = '{parameters.Experience}', " +
                $"Account = '{parameters.Account}', Gender = '{parameters.Gender}'.";
    }
}
