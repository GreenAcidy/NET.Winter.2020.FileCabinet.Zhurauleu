using FileCabinetApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Validators
{
    public class ValidatorBuilder
    {
        private List<IRecordValidator> validators = new List<IRecordValidator>();

        public IRecordValidator Create()
            => new CompositeValidator(this.validators);

        public ValidatorBuilder ValidateCommandName(int min, int max)
        {
            this.validators.Add(new CommandNameValidator(min, max));
            return this;
        }

        public ValidatorBuilder ValidateExecutionDate(DateTime from, DateTime to)
        {
            this.validators.Add(new ExecutionDateValidator(from, to));
            return this;
        }

        public ValidatorBuilder ValidateExperience(short min, short max)
        {
            this.validators.Add(new ExperienceValidator(min, max));
            return this;
        }
    }
}
