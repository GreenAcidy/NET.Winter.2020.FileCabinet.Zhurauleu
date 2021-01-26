using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ExperienceValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        public ExperienceValidator(int min, int max)
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

            if (inputData.Experience < this.min || inputData.Experience > this.max)
            {
                throw new ArgumentException($"{nameof(inputData.Experience)} must be in range from {nameof(this.min)} to {nameof(this.max)}.");
            }
        }
    }
}
