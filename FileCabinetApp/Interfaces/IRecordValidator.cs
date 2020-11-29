using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Interfaces
{
    /// <summary>
    /// Sets behaviour of validation input data.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Get input data and validate.
        /// </summary>
        /// <param name="inputData">input data.</param>
        public void ValidateParameters(FileCabinetInputData inputData);
    }
}
