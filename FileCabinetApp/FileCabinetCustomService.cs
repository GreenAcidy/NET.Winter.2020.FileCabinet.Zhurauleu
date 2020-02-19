using FileCabinetApp.Interfaces;
using FileCabinetApp.Validators;

namespace FileCabinetApp
{
    /// <summary>
    /// Class FileCabinetDefaultService contains custom methods of validation input data.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        public FileCabinetCustomService()
            : base(new CustomValidator())
        { }

        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
