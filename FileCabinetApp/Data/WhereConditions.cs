using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class contains conditions for improved find.
    /// </summary>
    public class WhereConditions
    {
        /// <summary>
        /// Gets or sets condition firstName.
        /// </summary>
        /// <value>FirstName.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets condition lastName.
        /// </summary>
        /// <value>LastName.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets condition nullable dateOfBirth.
        /// </summary>
        /// <value>Nullable date.</value>
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets condition nullable experience.
        /// </summary>
        /// <value>Nullable experience.</value>
        public short? Experience { get; set; }

        /// <summary>
        /// Gets or sets condition nullable account.
        /// </summary>
        /// <value>Nullable account.</value>
        public decimal? Account { get; set; }

        /// <summary>
        /// Gets or sets condition nullable gender.
        /// </summary>
        /// <value>Nullable gender.</value>
        public char? Gender { get; set; }
    }
}
