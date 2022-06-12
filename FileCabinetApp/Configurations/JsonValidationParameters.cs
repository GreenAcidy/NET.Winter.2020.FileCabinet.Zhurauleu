using System;
namespace FileCabinetApp.Configurations
{
    public class JsonValidationParameters
    {
        public int CommandNameMinLength { get; set; }

        public int CommandNameMaxLenght { get; set; }

        public int LastNameMinLength { get; set; }

        public int LastNameMaxLength { get; set; }

        public DateTime ExecutionDateFrom { get; set; }

        public DateTime ExecutionDateTo { get; set; }

        public short CodeMinValue { get; set; }

        public short CodeMaxValue { get; set; }

        public int AccountMinValue { get; set; }

        public string Gender { get; set; }
    }
}
