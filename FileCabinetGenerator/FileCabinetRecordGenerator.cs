﻿using System;
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
            record.FirstName = this.GenerateName(randomGenerator.Next(3, 60));
            record.LastName = this.GenerateName(randomGenerator.Next(3, 60));
            record.DateOfBirth = GenerateDateOfBirth();
            record.Gender = char.ToUpper(gender[randomGenerator.Next(gender.Length)]);
            record.Account = randomGenerator.Next();
            record.Experience = Convert.ToInt16(randomGenerator.Next(DateTime.Now.Year - record.DateOfBirth.Year));
            

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

        private DateTime GenerateDateOfBirth()
        {
            return startDate.AddDays(randomGenerator.Next((DateTime.Today - startDate).Days));
        }
    }
}