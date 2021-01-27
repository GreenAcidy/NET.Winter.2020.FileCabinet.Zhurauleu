using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class Xml writer.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        private readonly XmlWriter writer;
        private XmlSerializer serializer;
        private SerializableCollection records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
        /// <param name="records">The serializable records.</param>
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

        /// <summary>
        /// Write record in xml format.
        /// </summary>
        public void Write()
        {
            this.serializer.Serialize(this.writer, this.records);
        }
    }
}
