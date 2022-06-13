using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class DefaultValidator contains default methods of validation input data.
    /// </summary>
    public class DefaultValidator : IRecordValidator
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

        private void ValidateFirstName(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name), "Must not be null!");
            }

            int lenth = name.Length;

            if (lenth < 2 || lenth > 60)
            {
                throw new ArgumentException("Name must be shorter than 61 symbol and larger than 1 symbol");
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("Must contain at least two symbols except space");
            }
        }

        private void ValidateLastName(string name)
        {
            if (name is null)
            {
                throw new ArgumentNullException(nameof(name), "Must not be null!");
            }

            int lenth = name.Length;

            if (lenth < 2 || lenth > 60)
            {
                throw new ArgumentException("Name must be shorter than 61 symbol and larger than 1 symbol");
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                throw new ArgumentException("Must contain at least two symbols except space");
            }
        }

        private void ValidateDateOfBirth(DateTime dateOfBirth)
        {
            DateTime date = new DateTime(1950, 01, 01);
            if (dateOfBirth > DateTime.Today || dateOfBirth < date)
            {
                throw new ArgumentException("Date of birth must be in range from 01-Jan-1950 to current day");
            }
        }

        private void ValidateCode(char Code)
        {
            if (char.IsWhiteSpace(Code))
            {
                throw new ArgumentException("Must contain except space symbol M/F");
            }

            if (!(Code == 'm' || Code == 'M' || Code == 'f' || Code == 'F'))
            {
                throw new ArgumentException("Must equal symbol 'M' or 'F'");
            }
        }

        private void ValidateExperience(short experience)
        {
            if (experience < 0)
            {
                throw new ArgumentException("Experience can not be negative");
            }
        }

        private void ValidateAccount(decimal account)
        {
            if (account <= 0)
            {
                throw new ArgumentException("Account must be positive");
            }
        }
    }
}
