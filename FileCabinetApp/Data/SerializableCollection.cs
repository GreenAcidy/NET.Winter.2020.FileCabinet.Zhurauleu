﻿
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    [XmlRoot("Records")]
    public class SerializableCollection
    {
        [XmlElement("Record")]
        public SerializableRecord[] SerializeRecords { get; set; }
    }
}