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

            ValidationName(inputData.FirstName);
            ValidationName(inputData.LastName);
            ValidationData(inputData.DateOfBirth);
            ValidationGender(inputData.Gender);
            ValidationExpirience(inputData.Experience);
            ValidationAccount(inputData.Account);
        }

        private static void ValidationName(string name)
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

        private static void ValidationData(DateTime dateOfBirth)
        {
            DateTime date = new DateTime(1980, 01, 01);
            if (dateOfBirth > DateTime.Today || dateOfBirth < date)
            {
                throw new ArgumentException("Date of birth must be in range from 01-Jan-1980 to current day");
            }
        }

        private static void ValidationGender(char gender)
        {
            if (char.IsWhiteSpace(gender))
            {
                throw new ArgumentException("Must contain except space symbol M/F");
            }

            if (!(gender == 'm' || gender == 'M' || gender == 'W' || gender == 'w'))
            {
                throw new ArgumentException("Must equal symbol 'M' or 'W'");
            }
        }

        private static void ValidationExpirience(short experience)
        {
            if (experience < 5)
            {
                throw new ArgumentException("Experience can not be less than 5 years");
            }
        }

        private static void ValidationAccount(decimal account)
        {
            if (account <= 0)
            {
                throw new ArgumentException("Account must be positive");
            }
        }
    }
}
