using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;
        private FileCabinetRecord[] records;

        public FileCabinetRecordCsvWriter(TextWriter writer, FileCabinetRecord[] records)
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
            this.records = records;
        }

        public void Write()
        {
            writer.WriteLine("Id, First Name, Last Name, Date, of, Birth, Gender, Experience, Account");
            foreach (var record in this.records)
            {
                writer.WriteLine(record.ToString(), CultureInfo.InvariantCulture);
            }
        }
    }
}