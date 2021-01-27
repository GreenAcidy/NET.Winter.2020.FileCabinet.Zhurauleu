using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Serializable record data.
    /// </summary>
    [Serializable]
    public class SerializableRecord
    {
        /// <summary>
        /// Gets or sets records id.
        /// </summary>
        /// <value>The records id.</value>
        [XmlAttribute("Id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets records firstName.
        /// </summary>
        /// <value>The records firstName.</value>
        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets record lastName.
        /// </summary>
        /// <value>The records lastName.</value>
        [XmlElement("LastName")]
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets records gender.
        /// </summary>
        /// <value>The records gender.</value>
        [XmlElement("Gender")]
        public char Gender { get; set; }

        /// <summary>
        /// Gets or sets records experience.
        /// </summary>
        /// <value>The records experience.</value>
        [XmlElement("Experience")]
        public short Experience { get; set; }

        /// <summary>
        /// Gets or sets records account.
        /// </summary>
        /// <value>The records account.</value>
        [XmlElement("Account")]
        public decimal Account { get; set; }

        /// <summary>
        /// Gets or sets records date of birth.
        /// </summary>
        /// <value>The records date of birth.</value>
        [XmlElement("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
