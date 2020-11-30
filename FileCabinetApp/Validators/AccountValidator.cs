using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class AccountValidator : IRecordValidator
    {
        private readonly decimal min;

        public AccountValidator(decimal min)
        {
            this.min = min;
        }

        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (inputData.Account < this.min)
            {
                throw new ArgumentException($"{nameof(inputData.Account)} must be greatest than {this.min}");
            }
        }
    }
}
