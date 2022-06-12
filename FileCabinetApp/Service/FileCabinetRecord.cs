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
        /// Gets or sets command name of command in record.
        /// </summary>
        /// <value>
        /// command name of command in record.
        /// </value>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets or sets execution date of command in record.
        /// </summary>
        /// <value>
        /// execution date of command in record..
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets code of command in record.
        /// </summary>
        /// <value>
        /// code of command in record..
        /// </value>
        public short Code { get; set; }

        /// <summary>
        /// Overrided method return one record of command.
        /// </summary>
        /// <returns>One record of command.</returns>
        public override string ToString()
        {
            return string.Format(new CultureInfo("en-US"), "#{0}, {1}, {2}, {3}",
                this.Id, this.CommandName, this.ExecutionDate, this.Code);
        }
    }
}