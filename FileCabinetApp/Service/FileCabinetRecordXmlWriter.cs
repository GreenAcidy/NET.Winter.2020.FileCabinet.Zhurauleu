using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;
        private XmlSerializer serializer;
        private SerializableCollection records;

        public FileCabinetRecordXmlWriter(XmlWriter writer, SerializableCollection records)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.writer = writer;
            this.serializer = new XmlSerializer(typeof(SerializableCollection));
            this.records = records;
        }

        public void Write()
        {
            this.serializer.Serialize(this.writer, this.records);
        }
    }
}
