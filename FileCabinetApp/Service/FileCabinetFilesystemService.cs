using FileCabinetApp.Validators;
using System;
using System.Collections.ObjectModel;
using System.IO;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.Service
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private readonly IRecordValidator validator;

        public IRecordValidator Validator => throw new NotImplementedException();

        public FileCabinetFilesystemService(FileStream fileStream)
            : this(new DefaultValidator(), fileStream)
        {
            throw new NotImplementedException();
        }

        public FileCabinetFilesystemService(IRecordValidator validator, FileStream fileStream)
        {
            throw new NotImplementedException();
        }

        public int CreateRecord(FileCabinetInputData parameters)
        {
            throw new NotImplementedException();
        }

        public void EditRecord(int id, FileCabinetInputData parameters)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
            =>
            throw new NotImplementedException();

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapShot()
        {
            throw new NotImplementedException();
        }
    }
}
