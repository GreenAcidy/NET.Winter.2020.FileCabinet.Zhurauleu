using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FileCabinetApp.Service
{
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.records = records;
        }

        public IReadOnlyCollection<FileCabinetRecord> Records => this.records;

        public void SaveToCSV(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            var csvWriter = new FileCabinetRecordCsvWriter(writer, this.records);
            csvWriter.Write();
        }

        public void SaveToXML(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            var collection = new List<SerializableRecord>();

            foreach (var record in this.records)
            {
                var serializeRecord = new SerializableRecord();
                serializeRecord.Id = record.Id;
                serializeRecord.CommandName = record.CommandName;
                serializeRecord.ExecutionDate = record.ExecutionDate;
                serializeRecord.Experience = record.Experience;

                collection.Add(serializeRecord);
            }

            var serializableRecords = new SerializableCollection();
            serializableRecords.SerializeRecords = collection.ToArray();

            var xmlWriter = new FileCabinetRecordXmlWriter(XmlWriter.Create(writer), serializableRecords);
            xmlWriter.Write();
        }

        public void LoadFromCsv(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            var csvReader = new FileCabinetRecordCsvReader(reader);
            this.records = csvReader.Read().ToArray();
        }

        public void LoadFromXml(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            var xmlReader = new FileCabinetRecordXmlReader(reader);
            this.records = xmlReader.Read().ToArray();
        }
    }
}
