using System;
using System.Text;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    public class FileCabinetRecordGenerator
    {
        public const string alphabet = "abcdefghijklmnopqrstuvwxyz";
        public const string gender = "mf";
        private readonly DateTime startDate;
        private readonly Random randomGenerator;

        public FileCabinetRecordGenerator()
        {
            this.startDate = new DateTime(1950, 1, 1);
            this.randomGenerator = new Random();
        }

        public FileCabinetRecord Generate(int recordId)
        {
            var record = new FileCabinetRecord();

            record.Id = recordId;
            record.CommandName = this.GenerateName(randomGenerator.Next(3, 60));
            record.ExecutionDate = GenerateExecutionDate();
            record.Experience = Convert.ToInt16(randomGenerator.Next(DateTime.Now.Year - record.ExecutionDate.Year));
            

            return record;
        }

        private string GenerateName(int countSymbols)
        {
            var random = new Random();
            var sb = new StringBuilder();

            for (int i = 0; i < countSymbols - 1; i++)
            {
                int position = random.Next(alphabet.Length - 1);
                sb.Append(i == 0 ? char.ToUpper(alphabet[position]) : alphabet[position]);
            }

            return sb.ToString();
        }

        private DateTime GenerateExecutionDate()
        {
            return startDate.AddDays(randomGenerator.Next((DateTime.Today - startDate).Days));
        }
    }
}