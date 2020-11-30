using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public static class ValidatorExtention
    {
        public static IRecordValidator CreateDefault(this ValidatorBuilder builder)
            =>
            builder
                .ValidateFirstName(2, 60)
                .ValidateLastName(2, 60)
                .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
                .ValidateGender()
                .ValidateExperience(0, 20)
                .ValidateAccount(0)
                .Create();

        public static IRecordValidator CreateCustom(this ValidatorBuilder builder)
            =>
            builder
                .ValidateFirstName(3, 30)
                .ValidateLastName(3, 30)
                .ValidateDateOfBirth(new DateTime(1980, 1, 1), DateTime.Now)
                .ValidateGender()
                .ValidateExperience(0, 10)
                .ValidateAccount(0)
                .Create();
    }
}
