using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.Service
{
    public class ServiceLogger : IFileCabinetService, IDisposable
    {
        private readonly IFileCabinetService service;
        private readonly StreamWriter writer;
        private bool disposed;

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

        public int CreateRecord(FileCabinetInputData parameters)
        {
            int id = this.service.CreateRecord(parameters);
            this.WriteLogInFile(nameof(this.CreateRecord), this.GetInfoFileCabinetInputData(parameters));
            this.WriteLogReturnInFile<int>(nameof(this.service.CreateRecord), id);
            return id;
        }

        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            this.service.EditRecord(id, parameters);
            this.WriteLogInFile(nameof(this.service.EditRecord), GetInfoFileCabinetInputData(parameters));
        }

        public IEnumerable<FileCabinetRecord> FindByAnd(WhereConditions conditions)
        {
            var collection = this.service.FindByAnd(conditions);
            this.WriteLogInFile(nameof(this.service.FindByAnd), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByAnd), conditions.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByOr(WhereConditions conditions)
        {
            var collection = this.service.FindByOr(conditions);
            this.WriteLogInFile(nameof(this.service.FindByOr), conditions.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByOr), conditions.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            var collection = this.service.FindByFirstName(firstName);
            WriteLogInFile(nameof(this.service.FindByFirstName), firstName);
            WriteLogReturnInFile(nameof(this.service.FindByFirstName), collection.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            var collection = this.service.FindByLastName(lastName);
            WriteLogInFile(nameof(this.service.FindByLastName), lastName);
            WriteLogReturnInFile(nameof(this.service.FindByLastName), collection.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            var collection = this.service.FindByDateOfBirth(dateOfBirth);

            this.WriteLogInFile(nameof(this.service.FindByDateOfBirth),  dateOfBirth.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByDateOfBirth), collection.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByGender(string gender)
        {
            var collection = this.service.FindByGender(gender);
            WriteLogInFile(nameof(this.service.FindByGender), gender);
            WriteLogReturnInFile(nameof(this.service.FindByGender), collection.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByExperience(string experience)
        {
            var collection = this.service.FindByExperience(experience);
            WriteLogInFile(nameof(this.service.FindByExperience), experience);
            WriteLogReturnInFile(nameof(this.service.FindByExperience), collection.ToString());
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByAccount(string account)
        {
            var collection = this.service.FindByAccount(account);
            WriteLogInFile(nameof(this.service.FindByAccount), account);
            WriteLogReturnInFile(nameof(this.service.FindByAccount), collection.ToString());
            return collection;
        }

        public bool Remove(int id)
        {
            bool isRemoved = this.service.Remove(id);
            this.WriteLogInFile(nameof(this.service.Remove), id.ToString(CultureInfo.InvariantCulture));
            this.WriteLogReturnInFile(nameof(this.service.Remove), isRemoved.ToString());
            return isRemoved;
        }

        public void Purge()
        {
            this.service.Purge();
            this.WriteLogInFile(nameof(this.service.Purge), string.Empty);
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            var collection = this.service.GetRecords();
            this.WriteLogInFile(nameof(this.service.GetRecords), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetRecords), collection.ToString());
            return collection;
        }

        public (int active, int removed) GetStat()
        {
            var stat = this.service.GetStat();
            this.WriteLogInFile(nameof(this.service.GetStat), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetStat), stat.ToString());
            return stat;
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            var snapshot = this.service.MakeSnapShot();
            this.WriteLogInFile(nameof(this.service.MakeSnapShot), string.Empty);
            return snapshot;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int count = this.service.Restore(snapshot);
            this.WriteLogInFile(nameof(this.service.Restore), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.Restore), count.ToString(CultureInfo.InvariantCulture));
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
