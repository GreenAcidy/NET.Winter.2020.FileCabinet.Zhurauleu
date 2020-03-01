using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Sets behaviour of validation input data.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Get input data and validate.
        /// </summary>
        /// <param name="inputData">input data.</param>
        public void ValidateParameters(FileCabinetInputData inputData);

        public void ValidationFirstName(string firstName);

        public void ValidationLastName(string lastName);

        public void ValidationData(DateTime dateOfBirth);

        public void ValidationGender(char gender);

        public void ValidationExperience(short experience);

        public void ValidationAccount(decimal bankAccount);
    }
}
