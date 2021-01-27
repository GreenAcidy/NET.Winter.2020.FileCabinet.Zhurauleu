using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Gender validator.
    /// </summary>
    public class GenderValidator : IRecordValidator
    {
        private readonly string genders;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenderValidator"/> class.
        /// </summary>
        /// <param name="genders">The genders.</param>
        public GenderValidator(string genders)
        {
            if (genders is null)
            {
                throw new ArgumentNullException($"{nameof(genders)} cannot be null.");
            }

            this.genders = genders;
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

            if (!char.IsLetter(inputData.Gender))
            {
                throw new ArgumentException($"{nameof(inputData.Gender)} must be letter.");
            }

            if (!this.genders.Contains(inputData.Gender, StringComparison.Ordinal))
            {
                throw new ArgumentException($"{nameof(inputData.Gender)} must be correct gender.");
            }
        }
    }
}
