using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Date of birth validator.
    /// </summary>
    public class DateOfBirthValidator : IRecordValidator
    {
        private readonly DateTime from;
        private readonly DateTime to;

        /// <summary>
        /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
        /// </summary>
        /// <param name="from">The start date.</param>
        /// <param name="to">The end date.</param>
        public DateOfBirthValidator(DateTime from, DateTime to)
        {
            if (to <= from)
            {
                throw new ArgumentException($"{nameof(from)} must be early than {nameof(to)}");
            }

            this.from = from;
            this.to = to;
        }

        /// <summary>
        /// Validate parameters.
        /// </summary>
        /// <param name="inputData">The Record data.</param>
        public void ValidateParameters(FileCabinetInputData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException($"{nameof(inputData)} cannot be null.");
            }

            if (inputData.DateOfBirth < this.from || inputData.DateOfBirth > this.to)
            {
                throw new ArgumentException($"{nameof(inputData.DateOfBirth)} is incorrect.");
            }
        }
    }
}
