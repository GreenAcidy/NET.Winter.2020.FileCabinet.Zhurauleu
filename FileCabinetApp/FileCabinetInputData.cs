using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetRecord contain input data of people.
    /// </summary>
    public class FileCabinetInputData
    {
        public FileCabinetInputData(string commandname, DateTime date, short code)
        {
            this.CommandName = commandname;
            this.ExecutionDate = date;
            this.Code = code;
        }

        /// <summary>
        /// Gets or sets command name of command.
        /// </summary>
        /// <value>
        /// command name of command.
        /// </value>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets or sets execution date of command.
        /// </summary>
        /// <value>
        /// execution date of command.
        /// </value>
        public DateTime ExecutionDate { get; set; }

        /// <summary>
        /// Gets or sets code of command.
        /// </summary>
        /// <value>
        /// code of command.
        /// </value>
        public short Code { get; set; }

        /// <summary>
        /// Overrided method return one record.
        /// </summary>
        /// <returns>One record of command.</returns>
        public override string ToString()
        {
            return $"#{this.CommandName}, {this.ExecutionDate.ToLongDateString()}, " +
                   $"{this.Code}";
        }
    }
}
