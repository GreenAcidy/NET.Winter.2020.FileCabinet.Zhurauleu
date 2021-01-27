using System;
using System.Collections.Generic;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Validators
{
    /// <summary>
    /// Class ValidatorBuilder is used for buildinf validators.
    /// </summary>
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        /// <summary>
        /// Create validator.
        /// </summary>
        /// <returns>The created validator.</returns>
        public IRecordValidator Create()
            => new CompositeValidator(this.validators);

        /// <summary>
        /// Return the firstName validator.
        /// </summary>
        /// <param name="min">The min firstName length.</param>
        /// <param name="max">The max firstName length.</param>
        /// <returns>The firstName validator.</returns>
        public ValidatorBuilder ValidateFirstName(int min, int max)
        {
            this.validators.Add(new FirstNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return the lastName validator.
        /// </summary>
        /// <param name="min">The min lastName length.</param>
        /// <param name="max">The max lastName length.</param>
        /// <returns>The lastName validator.</returns>
        public ValidatorBuilder ValidateLastName(int min, int max)
        {
            this.validators.Add(new LastNameValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return the dateOfBirth validator.
        /// </summary>
        /// <param name="from">The start date.</param>
        /// <param name="to">The end date.</param>
        /// <returns>The date of birth validator.</returns>
        public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
        {
            this.validators.Add(new DateOfBirthValidator(from, to));
            return this;
        }

        /// <summary>
        /// Return experience validator.
        /// </summary>
        /// <param name="min">The min experience.</param>
        /// <param name="max">The max experience.</param>
        /// <returns>The experience validator.</returns>
        public ValidatorBuilder ValidateExperience(short min, short max)
        {
            this.validators.Add(new ExperienceValidator(min, max));
            return this;
        }

        /// <summary>
        /// Return gender validator.
        /// </summary>
        /// <param name="genders">The genders.</param>
        /// <returns>The gender validator.</returns>
        public ValidatorBuilder ValidateGender(string genders)
        {
            this.validators.Add(new GenderValidator(genders));
            return this;
        }

        /// <summary>
        /// Return account vaildator.
        /// </summary>
        /// <param name="min">The min account.</param>
        /// <returns>The account validator.</returns>
        public ValidatorBuilder ValidateAccount(decimal min)
        {
            this.validators.Add(new AccountValidator(min));
            return this;
        }
    }
}
