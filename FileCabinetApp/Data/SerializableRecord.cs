using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    [Serializable]
    public class SerializableRecord
    {

        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("FirstName")]
        public string FirstName { get; set; }

        [XmlElement("LastName")]
        public string LastName { get; set; }

        [XmlElement("Gender")]
        public char Gender { get; set; }

        [XmlElement("Experience")]
        public short Experience { get; set; }

        [XmlElement("Account")]
        public decimal Account { get; set; }

        [XmlElement("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }
    }
}
