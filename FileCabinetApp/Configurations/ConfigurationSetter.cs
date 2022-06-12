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

            this.validationParameters.CommandNameMaxLenght = ruleCection.GetSection("commandName").GetValue<int>("max");
            this.validationParameters.CommandNameMinLength = ruleCection.GetSection("commandName").GetValue<int>("min");

            this.validationParameters.ExecutionDateFrom = ruleCection.GetSection("executionDate").GetValue<DateTime>("from");
            this.validationParameters.ExecutionDateTo = ruleCection.GetSection("executionDate").GetValue<DateTime>("to");

            this.validationParameters.ExperienceMaxValue = ruleCection.GetSection("experience").GetValue<short>("max");
            this.validationParameters.ExperienceMinValue = ruleCection.GetSection("experience").GetValue<short>("min");

        }
    }
}
