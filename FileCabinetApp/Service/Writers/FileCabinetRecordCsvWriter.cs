using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Class csv writer.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private TextWriter writer;
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">The csv writer.</param>
        /// <param name="records">Record.</param>
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

        /// <summary>
        /// Write records in csv file.
        /// </summary>
        public void Write()
        {
            // this.writer.WriteLine("Id, First Name, Last Name, Date of Birth, Gender, Experience, Account");
            foreach (var record in this.records)
            {
                this.writer.WriteLine(record.ToString(), CultureInfo.InvariantCulture);
            }
        }
    }
}