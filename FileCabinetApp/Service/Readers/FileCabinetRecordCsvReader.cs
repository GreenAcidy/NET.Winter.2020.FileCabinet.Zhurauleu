using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace FileCabinetApp.Service
{
    /// <summary>
    /// Classs Csv reader.
    /// </summary>
    public class FileCabinetRecordCsvReader : IDisposable
    {
        private readonly StreamReader streamReader;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="streamReader">The csv reader.</param>
        public FileCabinetRecordCsvReader(StreamReader streamReader)
        {
            if (streamReader is null)
            {
                throw new ArgumentNullException($"{nameof(streamReader)} cannot be null.");
            }

            this.streamReader = streamReader;
            this.disposed = true;
        }

        /// <summary>
        /// Read recards from csv file.
        /// </summary>
        /// <returns>The records collection.</returns>
        public IEnumerable<FileCabinetRecord> Read()
        {
            var readRecords = new List<FileCabinetRecord>();
            this.streamReader.BaseStream.Position = 0;

            while (!this.streamReader.EndOfStream)
            {
                var data = this.streamReader.ReadLine().Split(',');
                var record = this.BuildRecord(data);
                readRecords.Add(record);
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

        private FileCabinetRecord BuildRecord(string[] data)
            => new FileCabinetRecord
            {
                Id = int.Parse(data[0].Substring(1).Trim(), CultureInfo.InvariantCulture),
                FirstName = data[1].Trim(),
                LastName = data[2].Trim(),
                DateOfBirth = DateTime.Parse(data[3].Trim(), CultureInfo.InvariantCulture),
                Gender = char.Parse(data[4].Trim()),
                Experience = short.Parse(data[5].Trim(), CultureInfo.InvariantCulture),
                Account = decimal.Parse(data[6].Trim(), CultureInfo.InvariantCulture),
            };
    }
}
