using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Interfaces
{
    public interface IRecordValidator
    {
        public void ValidateParameters(FileCabinetInputData inputData);
    }
}
