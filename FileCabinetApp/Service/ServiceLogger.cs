using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

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

        public ReadOnlyCollection<FileCabinetRecord> FindByCommandName(string commandName)
        {
            var collection = this.service.FindByCommandName(commandName);
            WriteLogInFile(nameof(this.service.FindByCommandName), commandName);
            WriteLogReturnInFile(nameof(this.service.FindByCommandName), collection.ToString());
            return collection;
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByExecutionDate(DateTime executionDate)
        {
            var collection = this.service.FindByExecutionDate(executionDate);

            this.WriteLogInFile(nameof(this.service.FindByExecutionDate),  executionDate.ToString());
            this.WriteLogReturnInFile(nameof(this.service.FindByExecutionDate), collection.ToString());
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

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            var collection = this.service.GetRecords();
            this.WriteLogInFile(nameof(this.service.GetRecords), string.Empty);
            this.WriteLogReturnInFile(nameof(this.service.GetRecords), collection.ToString());
            return collection;
        }

        public (int real, int removed) GetStat()
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
            => $"CommandName = '{parameters.CommandName}', " +
                $"ExecutionDate = '{parameters.ExecutionDate}', Experience = '{parameters.Experience}'";
    }
}
