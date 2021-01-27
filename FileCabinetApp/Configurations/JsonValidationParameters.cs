using System;

namespace FileCabinetApp.Configurations
{
    /// <summary>
    /// Class incapsulate validation parameters.
    /// </summary>
    public class JsonValidationParameters
    {
        /// <summary>
        /// Gets or sets max length firstName.
        /// </summary>
        /// <value>The max length firstName.</value>
        public int FirstNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets min length firstName.
        /// </summary>
        /// <value>The min length lastName.</value>
        public int FirstNameMaxLenght { get; set; }

        /// <summary>
        /// Gets or sets max length lastName.
        /// </summary>
        /// <value>The max length lastName.</value>
        public int LastNameMinLength { get; set; }

        /// <summary>
        /// Gets or sets min length lastName.
        /// </summary>
        /// <value>The min length lastName.</value>
        public int LastNameMaxLength { get; set; }

        /// <summary>
        /// Gets or sets start date of birth.
        /// </summary>
        /// <value>The start date of birth.</value>
        public DateTime DateOfBirthFrom { get; set; }

        /// <summary>
        /// Gets or sets end date of birth.
        /// </summary>
        /// <value>The end date of birth.</value>
        public DateTime DateOfBirthTo { get; set; }

        /// <summary>
        /// Gets or sets max experience.
        /// </summary>
        /// <value>The max experience.</value>
        public short ExperienceMinValue { get; set; }

        /// <summary>
        /// Gets or sets min experience.
        /// </summary>
        /// <value>The min experience.</value>
        public short ExperienceMaxValue { get; set; }

        /// <summary>
        /// Gets or sets min account.
        /// </summary>
        /// <value>The min account.</value>
        public int AccountMinValue { get; set; }

        /// <summary>
        /// Gets or sets genders.
        /// </summary>
        /// <value>The genders.</value>
        public string Gender { get; set; }
    }
}
