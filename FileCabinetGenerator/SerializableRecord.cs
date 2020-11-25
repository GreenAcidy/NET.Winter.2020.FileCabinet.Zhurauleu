using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetGenerator
{
    [Serializable]
    public class SerializableRecord
    {
        public DateTime dateOfBirth;

        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Experience")]
        public short Epirience { get; set; }

        [XmlElement("Account")]
        public decimal Account { get; set; }

        [XmlElement("Gender")]
        public char Gender { get; set; }

        [XmlElement("DateOfBirth")]
        public string DateOfBirth
        {
            get => this.dateOfBirth.ToString("mm/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}