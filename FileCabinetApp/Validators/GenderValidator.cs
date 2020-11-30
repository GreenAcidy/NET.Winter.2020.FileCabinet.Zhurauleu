using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class GenderValidator : IRecordValidator
    {
        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (!char.IsLetter(inputData.Gender))
            {
                throw new ArgumentException($"{nameof(inputData.Gender)} must be letter.");
            }
        }
    }
}
