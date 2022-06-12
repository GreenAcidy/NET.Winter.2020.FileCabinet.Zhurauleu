using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FileCabinetApp.Service;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Interface IFileCabinetService sets behaviour of working with Cabinet.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Method get data and create record.
        /// </summary>
        /// <param name="inputData">input data.</param>
        /// <returns>id of new record.</returns>
        public int CreateRecord(FileCabinetInputData inputData);

        /// <summary>
        /// Method get data and edit existing record.
        /// </summary>
        /// <param name="id">input id of existing record.</param>
        /// <param name="inputData">input data.</param>
        public void EditRecord(int id, FileCabinetInputData inputData);

        /// <summary>
        /// Method find record by input command name.
        /// </summary>
        /// <param name="commandName">input command name.</param>
        /// <returns>all records whose command name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByCommandName(string commandName);

        /// <summary>
        /// Method find record by input execution date.
        /// </summary>
        /// <param name="executionDate">input command name.</param>
        /// <returns>all records whose execution date matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByExecutionDate(DateTime executionDate);

        public bool Remove(int id);

        public void Purge();

        /// <summary>
        /// Method return all records.
        /// </summary>
        /// <returns>all records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Method return count of records.
        /// </summary>
        /// <returns>count of records.</returns>
        public (int real, int removed) GetStat();

        public FileCabinetServiceSnapshot MakeSnapShot();

        public int Restore(FileCabinetServiceSnapshot snapshot);
    }
}
