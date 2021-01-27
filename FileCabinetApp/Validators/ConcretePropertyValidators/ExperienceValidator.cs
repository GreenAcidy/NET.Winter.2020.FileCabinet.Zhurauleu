using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The experience validator.
    /// </summary>
    public class ExperienceValidator : IRecordValidator
    {
        private readonly int min;
        private readonly int max;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExperienceValidator"/> class.
        /// </summary>
        /// <param name="min">The min experience.</param>
        /// <param name="max">The max experience.</param>
        public ExperienceValidator(int min, int max)
        {
            if (max <= min)
            {
                throw new ArgumentException($"{nameof(min)} must be less than {nameof(max)}");
            }

            this.max = max;
            this.min = min;
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

            if (inputData.Experience < this.min || inputData.Experience > this.max)
            {
                throw new ArgumentException($"{nameof(inputData.Experience)} must be in range from {nameof(this.min)} to {nameof(this.max)}.");
            }
        }
    }
}
