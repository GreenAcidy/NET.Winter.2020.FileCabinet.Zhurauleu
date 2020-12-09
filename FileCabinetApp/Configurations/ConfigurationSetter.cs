using System;
using Microsoft.Extensions.Configuration;

namespace FileCabinetApp.Configurations
{
    public class ConfigurationSetter
    {
        private const string SettersPath = @"validation-rules.json";
        private readonly IConfiguration configuration;
        private readonly string validationRule;
        private readonly JsonValidationParameters validationParameters;

        public ConfigurationSetter(string validationRule)
        {
            if (validationRule is null)
            {
                throw new ArgumentNullException($"{nameof(validationRule)} cannot be null.");
            }

            this.validationRule = validationRule;
            this.configuration = new ConfigurationBuilder().AddJsonFile(SettersPath).Build();
            this.validationParameters = new JsonValidationParameters();
        }

        public JsonValidationParameters GetParameters()
        {
            this.SetParameters();
            return this.validationParameters;
        }

        private void SetParameters()
        {
            var ruleCection = this.configuration.GetSection(this.validationRule);

            this.validationParameters.FirstNameMaxLenght = ruleCection.GetSection("firstName").GetValue<int>("max");
            this.validationParameters.FirstNameMinLength = ruleCection.GetSection("firstName").GetValue<int>("min");

            this.validationParameters.LastNameMaxLength = ruleCection.GetSection("lastName").GetValue<int>("max");
            this.validationParameters.LastNameMinLength = ruleCection.GetSection("lastName").GetValue<int>("min");

            this.validationParameters.DateOfBirthFrom = ruleCection.GetSection("dateOfBirth").GetValue<DateTime>("from");
            this.validationParameters.DateOfBirthTo = ruleCection.GetSection("dateOfBirth").GetValue<DateTime>("to");

            this.validationParameters.ExperienceMaxValue = ruleCection.GetSection("experience").GetValue<short>("max");
            this.validationParameters.ExperienceMinValue = ruleCection.GetSection("experience").GetValue<short>("min");

            this.validationParameters.AccountMinValue = ruleCection.GetSection("account").GetValue<int>("min");

            this.validationParameters.Gender = ruleCection.GetSection("gender").GetValue<string>("genders");
        }
    }
}
