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

        [XmlElement("CommandName")]
        public string CommandName { get; set; }

        [XmlElement("Experience")]
        public short Experience { get; set; }

        [XmlElement("ExecutionDate")]
        public DateTime ExecutionDate { get; set; }
    }
}
