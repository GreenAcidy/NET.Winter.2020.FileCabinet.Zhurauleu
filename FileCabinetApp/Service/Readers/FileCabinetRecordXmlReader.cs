using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class Xml reader.
    /// </summary>
    public class FileCabinetRecordXmlReader : IDisposable
    {
        private readonly StreamReader streamReader;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
        /// </summary>
        /// <param name="streamReader">The xml reader.</param>
        public FileCabinetRecordXmlReader(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException($"{nameof(streamReader)} cannot be null.");
            }

            this.streamReader = streamReader;
        }

        /// <summary>
        /// Read recards from xml file.
        /// </summary>
        /// <returns>The records collection.</returns>
        public IEnumerable<FileCabinetRecord> Read()
        {
            var readRecords = new List<FileCabinetRecord>();
            this.streamReader.BaseStream.Position = 0;

            using (var xmlReader = new XmlTextReader(this.streamReader))
            {
                var serializer = new XmlSerializer(typeof(SerializableCollection));
                var serializableRecords = (SerializableCollection)serializer.Deserialize(xmlReader);

                foreach (var serializableRecord in serializableRecords.SerializeRecords)
                {
                    readRecords.Add(BuildRecord(serializableRecord));
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(readRecords);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Is disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.streamReader.Close();
            }

            this.disposed = true;
        }

        private static FileCabinetRecord BuildRecord(SerializableRecord record)
    => new FileCabinetRecord
    {
        Id = record.Id,
        FirstName = record.FirstName,
        LastName = record.LastName,
        DateOfBirth = record.dateOfBirth,
        Gender = record.Gender,
        Experience = record.Experience,
        Account = record.Account,
    };
    }
}
