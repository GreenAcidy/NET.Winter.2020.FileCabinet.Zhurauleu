using FileCabinetApp.Configurations;
using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public static class ValidatorExtention
    {
        public static IRecordValidator Create(this ValidatorBuilder builder, string validationRule = "default")
        {
            if (builder is null)
            {
                throw new ArgumentNullException($"{nameof(builder)} canot be null.");
            }

            var setters = new ConfigurationSetter(validationRule);
            var validateParameters = setters.GetParameters();

            return builder.
                        ValidateCommandName(validateParameters.CommandNameMinLength, validateParameters.CommandNameMaxLenght).
                        ValidateExecutionDate(validateParameters.ExecutionDateFrom, validateParameters.ExecutionDateTo).
                        ValidateCode(validateParameters.CodeMinValue, validateParameters.CodeMaxValue).
                        Create();
        }
    }
}
