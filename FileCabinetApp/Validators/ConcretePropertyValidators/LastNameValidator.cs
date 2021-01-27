using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The last name validator.
    /// </summary>
    public class LastNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">The min length name.</param>
        /// <param name="maxLength">the max length name.</param>
        public LastNameValidator(int minLength, int maxLength)
        {
            if (maxLength <= minLength)
            {
                throw new ArgumentException($"{nameof(minLength)} must be less than {nameof(maxLength)}");
            }

            this.maxLength = maxLength;
            this.minLength = minLength;
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
