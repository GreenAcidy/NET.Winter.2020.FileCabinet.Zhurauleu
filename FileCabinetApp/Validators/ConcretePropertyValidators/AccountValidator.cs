using System;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// The account validator.
    /// </summary>
    public class AccountValidator : IRecordValidator
    {
        private readonly decimal min;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountValidator"/> class.
        /// </summary>
        /// <param name="min">The min account.</param>
        public AccountValidator(decimal min)
        {
            this.min = min;
        }

        /// <summary>
        /// Valadate parameters.
        /// </summary>
        /// <param name="inputData">Record data.</param>
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
