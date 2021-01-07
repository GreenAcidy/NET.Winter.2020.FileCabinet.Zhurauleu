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
        /// Method find record by input first name.
        /// </summary>
        /// <param name="firstName">input first name.</param>
        /// <returns>all records whose first name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Method find record by input last name.
        /// </summary>
        /// <param name="lastName">input first name.</param>
        /// <returns>all records whose last name matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Method find record by input date of birth.
        /// </summary>
        /// <param name="dateOfBirth">input first name.</param>
        /// <returns>all records whose date of birth matches the incoming.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

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
