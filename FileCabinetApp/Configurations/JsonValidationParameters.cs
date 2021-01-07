using System;
namespace FileCabinetApp.Configurations
{
    public class JsonValidationParameters
    {
        public int FirstNameMinLength { get; set; }

        public int FirstNameMaxLenght { get; set; }

        public int LastNameMinLength { get; set; }

        public int LastNameMaxLength { get; set; }

        public DateTime DateOfBirthFrom { get; set; }

        public DateTime DateOfBirthTo { get; set; }

        public short ExperienceMinValue { get; set; }

        public short ExperienceMaxValue { get; set; }

        public int AccountMinValue { get; set; }

        public string Gender { get; set; }
    }
}
