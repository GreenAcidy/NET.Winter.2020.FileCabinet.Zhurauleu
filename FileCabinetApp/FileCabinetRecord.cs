using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public char Gender { get; set; }

        public short Experience { get; set; }

        public decimal Account { get; set; }

        public override string ToString()
        {
            return $"#{this.Id}, {this.FirstName}, {this.LastName}, {this.DateOfBirth.ToLongDateString()}, " +
                   $"{this.Gender}, {this.Experience}, {this.Account}";
        }
    }
}