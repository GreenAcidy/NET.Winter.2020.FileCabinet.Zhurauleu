using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ExecutionDateValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        public ExecutionDateValidator(DateTime from, DateTime to)
        {
            if (to <= from)
            {
                throw new ArgumentException($"{nameof(from)} must be early than {nameof(to)}");
            }

            this.from = from;
            this.to = to;
        }

        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (inputData.ExecutionDate < this.from || inputData.ExecutionDate > this.to)
            {
                throw new ArgumentException($"{nameof(inputData.ExecutionDate)} is incorrect.");
            }
        }
    }
}
