using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Printers
{
    public class DefaultRecordPrinter
    {
        public void Print(IEnumerable<FileCabinetRecord> records)
        {
            foreach (var record in records)
            {
                Console.WriteLine($"#{record.Id}: {record.FirstName} {record.LastName}; Date of birth: {record.DateOfBirth.ToLongDateString()};" +
                    $" Gender: {record.Gender}; Experience: {record.Experience} years; Account: {record.Account}.");
            }
        }
    }
}
