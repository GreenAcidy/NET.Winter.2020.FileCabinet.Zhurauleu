﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Service
{
    public class ServiceMeter : IFileCabinetService
    {
        private readonly Stopwatch stopwatch;
        private readonly IFileCabinetService service;

        public ServiceMeter(IFileCabinetService service)
        {
            if (service is null)
            {
                throw new ArgumentNullException($"{nameof(service)} cannot be null.");
            }

            this.stopwatch = new Stopwatch();
            this.service = service;
        }

        public int CreateRecord(FileCabinetInputData parameters)
        {
            this.stopwatch.Restart();
            var id = this.service.CreateRecord(parameters);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.CreateRecord), this.stopwatch.ElapsedTicks);
            return id;
        }

        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            this.stopwatch.Restart();
            this.service.EditRecord(id, parameters);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.EditRecord), this.stopwatch.ElapsedTicks);
        }

        public IEnumerable<FileCabinetRecord> FindBy(string propertyName, string value)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindBy(propertyName, value);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindBy), this.stopwatch.ElapsedTicks);
            return collection;
        }

            public IEnumerable<FileCabinetRecord> FindByFirstName(string firstName)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByFirstName(firstName);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByFirstName), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByLastName(string lastName)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByLastName(lastName);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByLastName), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByDateOfBirth(dateOfBirth);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByDateOfBirth), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByExperience(string expirience)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByExperience(expirience);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByExperience), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByAccount(string balance)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByAccount(balance);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByAccount), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public IEnumerable<FileCabinetRecord> FindByGender(string englishLevel)
        {
            this.stopwatch.Restart();
            var collection = this.service.FindByGender(englishLevel);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.FindByGender), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public bool Remove(int id)
        {
            this.stopwatch.Restart();
            var isRemove = this.service.Remove(id);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Remove), this.stopwatch.ElapsedTicks);
            return isRemove;
        }

        public void Purge()
        {
            this.stopwatch.Restart();
            this.service.Purge();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Purge), this.stopwatch.ElapsedTicks);
        }

        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            this.stopwatch.Restart();
            var collection = this.service.GetRecords();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.GetRecords), this.stopwatch.ElapsedTicks);
            return collection;
        }

        public (int active, int removed) GetStat()
        {
            this.stopwatch.Restart();
            var stat = this.service.GetStat();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.GetStat), this.stopwatch.ElapsedTicks);
            return stat;
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            this.stopwatch.Restart();
            var snapshot = this.service.MakeSnapShot();
            this.stopwatch.Stop();
            this.Information(nameof(this.service.MakeSnapShot), this.stopwatch.ElapsedTicks);
            return snapshot;
        }

        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            this.stopwatch.Restart();
            var count = this.service.Restore(snapshot);
            this.stopwatch.Stop();
            this.Information(nameof(this.service.Restore), this.stopwatch.ElapsedTicks);
            return count;
        }

        private void Information(string methodName, long ticks)
            => Console.WriteLine($"{methodName} method execution duration is {ticks} ticks.");
    }
}
