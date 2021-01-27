
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class Serializeble record array.
    /// </summary>
    [XmlRoot("Records")]
    public class SerializableCollection
    {
        /// <summary>
        /// Gets or sets serializable record.
        /// </summary>
        /// <value>Serializable record.</value>
        [XmlElement("Record")]
#pragma warning disable CA1819 // Properties should not return arrays
        public SerializableRecord[] SerializeRecords { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
