using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetDefaultService contains default methods of validation input data.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        { }
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
