﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class FileCabinetServiceSnapshot saves statement for FileCabinetService.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="records">The array of records.</param>
        /// <exception cref="ArgumentNullException">Throws, when records is null.</exception>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.records = records;
        }

        /// <summary>
        /// Gets record collection.
        /// </summary>
        /// <value>The record collection.</value>
        public IReadOnlyCollection<FileCabinetRecord> Records => this.records;

        /// <summary>
        /// Save records array to csv file.
        /// </summary>
        /// <param name="writer">The csv writer.</param>
        public void SaveToCSV(StreamWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            var csvWriter = new FileCabinetRecordCsvWriter(writer, this.records);
            csvWriter.Write();
        }

        /// <summary>
        /// Save records array to xml file.
        /// </summary>
        /// <param name="writer">The xml writer.</param>
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
                serializeRecord.FirstName = record.FirstName;
                serializeRecord.LastName = record.LastName;
                serializeRecord.dateOfBirth = record.DateOfBirth;
                serializeRecord.Experience = record.Experience;
                serializeRecord.Account = record.Account;
                serializeRecord.Gender = record.Gender;

                collection.Add(serializeRecord);
            }

            var serializableRecords = new SerializableCollection();
            serializableRecords.SerializeRecords = collection.ToArray();

            var xmlWriter = new FileCabinetRecordXmlWriter(XmlWriter.Create(writer), serializableRecords);
            xmlWriter.Write();
        }

        /// <summary>
        /// Read the records from csv file.
        /// </summary>
        /// <param name="reader">The csv reader.</param>
        public void LoadFromCsv(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            using var csvReader = new FileCabinetRecordCsvReader(reader);
            this.records = csvReader.Read().ToArray();
        }

        /// <summary>
        /// Read the records from xml file.
        /// </summary>
        /// <param name="reader">The xml reader.</param>
        public void LoadFromXml(StreamReader reader)
        {
            if (reader is null)
            {
                throw new ArgumentNullException($"{nameof(reader)} cannot be null.");
            }

            using var xmlReader = new FileCabinetRecordXmlReader(reader);
            this.records = xmlReader.Read().ToArray();
        }
    }
}
