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
        private readonly List<FileCabinetRecord> listCommandName = new List<FileCabinetRecord>();
        private readonly List<FileCabinetRecord> listExecutionDate = new List<FileCabinetRecord>();

        private readonly Dictionary<string, List<FileCabinetRecord>> commandNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> executionDateDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();
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
                CommandName = inputData.CommandName,
                ExecutionDate = inputData.ExecutionDate,
                Experience = inputData.Experience,
            };

            this.list.Add(record);
            this.listCommandName.Add(record);
            this.listExecutionDate.Add(record);

            this.commandNameDictionary.Add(inputData.CommandName, this.listCommandName);
            this.executionDateDictionary.Add(inputData.ExecutionDate, this.listExecutionDate);

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
                CommandName = inputData.CommandName,
                ExecutionDate = inputData.ExecutionDate,
                Experience = inputData.Experience,
            };
            this.list[id - 1] = record;

            this.listCommandName[id - 1] = this.list[id - 1];
            this.listExecutionDate[id - 1] = this.list[id - 1];

            this.commandNameDictionary[inputData.CommandName] = this.listCommandName;
            this.executionDateDictionary[inputData.ExecutionDate] = this.listExecutionDate;
        }

        /// <summary>
        /// Method find record by input command name.
        /// </summary>
        /// <param name="commandName">input command name.</param>
        /// <returns>all records whose command name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByCommandName(string commandName)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.commandNameDictionary.TryGetValue(commandName, out result);
            this.records = new ReadOnlyCollection<FileCabinetRecord>(result);
            return this.records;
        }

        /// <summary>
        /// Method find record by input execution date.
        /// </summary>
        /// <param name="executionDate">input command name.</param>
        /// <returns>all records whose execution date matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByExecutionDate(DateTime executionDate)
        {
            List<FileCabinetRecord> result = new List<FileCabinetRecord>();
            this.executionDateDictionary.TryGetValue(executionDate, out result);
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
                    this.commandNameDictionary[record.CommandName.ToUpper()].Remove(record);
                    this.executionDateDictionary[record.ExecutionDate].Remove(record);
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
                        var data = new FileCabinetInputData(record.CommandName, record.ExecutionDate, record.Experience);
                        this.EditRecord(id, data);
                        count++;
                    }
                    else
                    {
                        this.list.Add(record);

                        if (this.commandNameDictionary.ContainsKey(record.CommandName.ToUpper()))
                        {
                            this.commandNameDictionary[record.CommandName.ToUpper()].Add(record);
                        }
                        else
                        {
                            this.commandNameDictionary.Add(record.CommandName.ToUpper(), new List<FileCabinetRecord> { record });
                        }

                        if (this.executionDateDictionary.ContainsKey(record.ExecutionDate))
                        {
                            this.executionDateDictionary[record.ExecutionDate].Add(record);
                        }
                        else
                        {
                            this.executionDateDictionary.Add(record.ExecutionDate, new List<FileCabinetRecord> { record });
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