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
                Console.WriteLine($"#{record.Id}: {record.CommandName} ; Execution date: {record.ExecutionDate.ToLongDateString()}, {record.ExecutionDate.TimeOfDay};" +
                    $" Code: {record.Code} .");
            }
        }
    }
}
