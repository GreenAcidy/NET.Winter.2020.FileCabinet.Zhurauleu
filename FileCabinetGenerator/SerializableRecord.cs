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

        [XmlAttribute("Id")]
        public int Id { get; set; }

        [XmlElement("CommandName")]
        public string CommandName { get; set; }

        [XmlElement("Code")]
        public short Code { get; set; }

        public DateTime executionDate;

        [XmlElement("ExecutionDate")]
        public string ExecutionDate
        {
            get => this.executionDate.ToString("mm/dd/yyyy", CultureInfo.InvariantCulture);
        }
    }
}