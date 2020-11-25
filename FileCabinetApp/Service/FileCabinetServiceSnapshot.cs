using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace FileCabinetApp.Service
{
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        public FileCabinetServiceSnapshot(FileCabinetRecord[] records)
        {
            if (records is null)
            {
                throw new ArgumentNullException($"{nameof(records)} cannot be null.");
            }

            this.records = records;
        }

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

            var xmlWriter = new FileCabinetRecordXmlWriter(XmlWriter.Create(writer), this.records);
            xmlWriter.Write();
        }
    }
}
