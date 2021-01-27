using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The first name validator.
    /// </summary>
    public class FirstNameValidator : IRecordValidator
    {
        private readonly int maxLength;
        private readonly int minLength;

        /// <summary>
        /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
        /// </summary>
        /// <param name="minLength">The min length name.</param>
        /// <param name="maxLength">The max length name.</param>
        public FirstNameValidator(int minLength, int maxLength)
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

            if (string.IsNullOrWhiteSpace(inputData.FirstName))
            {
                if (inputData.FirstName is null)
                {
                    throw new ArgumentNullException($"{nameof(inputData.FirstName)} cannot be null.");
                }

                if (inputData.FirstName.Length < this.minLength || inputData.FirstName.Length > this.maxLength)
                {
                    throw new ArgumentException($"{nameof(inputData.FirstName.Length)} must be in range {this.minLength} to {this.maxLength}.");
                }

                throw new ArgumentException($"{nameof(inputData.FirstName)} cannot be empty or whiteSpace.");
            }
        }
    }
}
