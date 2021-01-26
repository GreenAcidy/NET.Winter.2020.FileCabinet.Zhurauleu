using System;
using System.Globalization;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetRecord contain brief data of people.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id of record.
        /// </summary>
        /// <value>
        /// Id of record.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name of person in record.
        /// </summary>
        /// <value>
        /// first name of person in record.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of person in record.
        /// </summary>
        /// <value>
        /// last name of person in record..
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth of person in record.
        /// </summary>
        /// <value>
        /// date of birth of person in record..
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets gender of person in record.
        /// </summary>
        /// <value>
        /// gender of person in record..
        /// </value>
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets experience of person in record.
        /// </summary>
        /// <value>
        /// experience of person in record..
        /// </value>
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets account of person in record.
        /// </summary>
        /// <value>
        /// date of account of person in record..
        /// </value>
        public decimal Account { get; set; }

        /// <summary>
        /// Overrided method return one record of person.
        /// </summary>
        /// <returns>One record of person.</returns>
        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"), "#{0}, {1}, {2}, {3}, {4}, {5}, {6}",
                this.Id, this.FirstName, this.LastName, this.DateOfBirth, this.Gender, this.Experience, this.Account);
        }
    }
}