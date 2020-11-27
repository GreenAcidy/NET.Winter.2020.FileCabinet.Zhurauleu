using System;
using System.Globalization;
using System.IO;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public class RecordCsvWriter
    {
        private readonly TextWriter writer;

        public RecordCsvWriter(TextWriter writer)
        {
            if (writer is null)
            {
                throw new ArgumentNullException($"{nameof(writer)} cannot be null.");
            }

            this.writer = writer;
        }

        public void Write(FileCabinetRecord record)
        {
            if (record is null)
            {
                throw new ArgumentNullException($"{nameof(record)} cannot be null.");
            }

            writer.WriteLine(record.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
