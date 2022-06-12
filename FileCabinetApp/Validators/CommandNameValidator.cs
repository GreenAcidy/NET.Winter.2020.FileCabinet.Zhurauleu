using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class CommandNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        public CommandNameValidator(int minLength, int maxLength)
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

            if (string.IsNullOrWhiteSpace(inputData.CommandName))
            {
                if (inputData.CommandName is null)
                {
                    throw new ArgumentNullException($"{nameof(inputData.CommandName)} cannot be null.");
                }

                if (inputData.CommandName.Length < this.minLength || inputData.CommandName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(inputData.CommandName.Length)} must be in range {this.minLength} to {this.maxLength}.");
                }

                throw new ArgumentException($"{nameof(inputData.CommandName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
