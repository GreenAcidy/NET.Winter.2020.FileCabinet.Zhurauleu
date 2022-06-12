using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class CodeValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        public CodeValidator(int min, int max)
        {
            if (max <= min)
            {
                throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)}");
            }

            this.max = max;
            this.min = min;
        }

        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (inputData.Code < this.min || inputData.Code > this.max)
            {
                throw new ArgumentException($"{nameof(inputData.Code)} must be in range from {nameof(this.min)} to {nameof(this.max)}.");
            }
        }
    }
}
