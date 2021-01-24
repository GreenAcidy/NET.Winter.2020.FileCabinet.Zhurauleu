using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    public class InsertCommandHandler : ServiceCommandHandlerBase
    {
        private readonly Random randomGenerator;
        public const string InsertConstant = "insert";
        private const string InsertKeyWord = "values";

        public InsertCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
            this.randomGenerator = new Random();
        }

        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, InsertConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Insert(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Insert(string parameters)
        {
            var (properties, values) = this.Parse(parameters);
            var recordPropertyInfo = typeof(FileCabinetRecord).GetProperties();

            var record = new FileCabinetRecord()
            {
                FirstName = this.GeneRateName(),
                LastName = this.GeneRateName(),
                DateOfBirth = this.GenerateDateOfBirth(),
                Gender = this.GenerateGender(),
                Account = this.randomGenerator.Next(),
                Experience = Convert.ToInt16(this.randomGenerator.Next(DateTime.Now.Year - this.GenerateDateOfBirth().Year)),
            };

            for (int i = 0; i < properties.Length; i++)
            {
                var recordProperty = recordPropertyInfo.FirstOrDefault(prop => string.Equals(prop.Name, properties[i], StringComparison.OrdinalIgnoreCase));
                if (recordProperty is null)
                {
                    continue;
                }

                var converter = TypeDescriptor.GetConverter(recordProperty.PropertyType);
                recordProperty.SetValue(record, converter.ConvertFromInvariantString(values[i]));
            }

            var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

            this.fileCabinetService.CreateRecord(data);
        }

        private (string[] properties, string[] values) Parse(string parameters)
        {
            var insertArray = parameters.Split(InsertKeyWord);

            if (insertArray.Length != 2)
            {
                throw new ArgumentException($"{InsertConstant} input incorrect.");
            }

            var properties = insertArray[0].Split('(', ')', ',', ' ');
            var values = insertArray[1].Split('(', ')', ',', ' ', '\'');

            values = values.Where(x => x.Length != 0).ToArray();
            properties = properties.Where(x => x.Length != 0).ToArray();

            return (properties, values);
        }

        private string GeneRateName()
        {
            string alphabet = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            var sb = new StringBuilder();
            int countSymbols = random.Next(8, 60);

            for (int i = 0; i < countSymbols - 1; i++)
            {
                int position = random.Next(alphabet.Length - 1);
                sb.Append(i == 0 ? char.ToUpper(alphabet[position]) : alphabet[position]);
            }

            return sb.ToString();
        }

        private DateTime GenerateDateOfBirth()
        {
            DateTime startDate = new DateTime(1950, 1, 1);
            return startDate.AddDays(randomGenerator.Next((DateTime.Today - startDate).Days));
        }

        private char GenerateGender()
        {
            string genders = "mfn";
            var random = new Random();

            return genders[random.Next(0, 2)];
        }
    }
}
