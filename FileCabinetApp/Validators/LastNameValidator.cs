using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class LastNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        public LastNameValidator(int minLength, int maxLength)
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

            if (string.IsNullOrWhiteSpace(inputData.LastName))
            {
                if (inputData.LastName is null)
                {
                    throw new ArgumentNullException($"{nameof(inputData.LastName)} cannot be null.");
                }

                if (inputData.LastName.Length < this.minLength || inputData.LastName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(inputData.LastName.Length)} must be in range {this.minLength} to {this.maxLength}.");
                }

                throw new ArgumentException($"{nameof(inputData.LastName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
