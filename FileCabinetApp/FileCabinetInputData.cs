﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetRecord contain input data of people.
    /// </summary>
    public class FileCabinetInputData
    {
        /// <summary>
        /// Gets or sets first name of person.
        /// </summary>
        /// <value>
        /// first name of person.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of person.
        /// </summary>
        /// <value>
        /// last name of person in record..
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth of person.
        /// </summary>
        /// <value>
        /// date of birth of person.
        /// </value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets gender of person.
        /// </summary>
        /// <value>
        /// gender of person.
        /// </value>
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets experience of person.
        /// </summary>
        /// <value>
        /// experience of person.
        /// </value>
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets account of person.
        /// </summary>
        /// <value>
        /// date of account of person.
        /// </value>
        public decimal Account { get; set; }

        /// <summary>
        /// Overrided method return one record.
        /// </summary>
        /// <returns>One record of person.</returns>
        public override string ToString()
        {
            return $"#{this.FirstName}, {this.LastName}, {this.DateOfBirth.ToLongDateString()}, " +
                   $"{this.Gender}, {this.Experience}, {this.Account}";
        }
    }
}