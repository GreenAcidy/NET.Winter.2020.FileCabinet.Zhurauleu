using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class CustomValidator contains custom methods of validation input data.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Valides input data.
        /// </summary>
        /// <param name="inputData">input data.</param>
        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException(nameof(inputData), "must not be null");
            }

            this.ValidateFirstName(inputData.FirstName);
            this.ValidateLastName(inputData.LastName);
            this.ValidateDateOfBirth(inputData.DateOfBirth);
            this.ValidateCode(inputData.Code);
            this.ValidateExperience(inputData.Experience);
            this.ValidateAccount(inputData.Account);
        }

        private void ValidateCommandName(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name), "Must not be null!");
            }

            int lenth = name.Length;

            if (lenth < 5 || lenth > 30)
            {
                throw new ArgumentException("Name must be shorter than 30 symbols and larger than 5 symbols");
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("Must contain at least five symbols except space");
            }
        }

        private void ValidateExecutionDate(DateTime dateOfBirth)
        {
            DateTime date = new DateTime(1980, 01, 01);
            if (dateOfBirth > DateTime.Today || dateOfBirth < date)
            {
                throw new ArgumentException("Date of birth must be in range from 01-Jan-1980 to current day");
            }
        }

        private void ValidateCode(char Code)
        {
            if (char.IsWhiteSpace(Code))
            {
                throw new ArgumentException("Must contain except space symbol M/W");
            }

            if (!(Code == 'm' || Code == 'M' || Code == 'W' || Code == 'w'))
            {
                throw new ArgumentException("Must equal symbol 'M' or 'W'");
            }
        }
    }
}
