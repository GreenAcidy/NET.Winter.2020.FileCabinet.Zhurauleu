using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        public FirstNameValidator(int minLength, int maxLength)
        {
            if (maxLength <= minLength)
            {
                throw new ArgumentException($"{nameof(minLength)} must be less than {nameof(maxLength)}");
            }

            this.maxLength = maxLength;
            this.minLength = minLength;
        }

        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(inputData.FirstName))
            {
                if (inputData.FirstName is null)
                {
                    throw new ArgumentNullException($"{nameof(inputData.FirstName)} cannot be null.");
                }

                if (inputData.FirstName.Length < this.minLength || inputData.FirstName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(inputData.FirstName.Length)} must be in range {this.minLength} to {this.maxLength}.");
                }

                throw new ArgumentException($"{nameof(inputData.FirstName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
