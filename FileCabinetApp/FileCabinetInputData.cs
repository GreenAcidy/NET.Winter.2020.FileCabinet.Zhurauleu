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
        public FileCabinetInputData(string commandname, DateTime date, short experience)
        {
            this.CommandName = commandname;
            this.ExecutionDate = date;
            this.Experience = experience;
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
        /// Gets or sets experience of command.
        /// </summary>
        /// <value>
        /// experience of command.
        /// </value>
        public short Experience { get; set; }

        /// <summary>
        /// Overrided method return one record.
        /// </summary>
        /// <returns>One record of command.</returns>
        public override string ToString()
        {
            return $"#{this.CommandName}, {this.ExecutionDate.ToLongDateString()}, " +
                   $"{this.Experience}";
        }
    }
}
