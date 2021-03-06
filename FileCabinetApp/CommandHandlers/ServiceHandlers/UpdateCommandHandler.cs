﻿using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.CommandHandlers.HandlerInfrastructure;
using FileCabinetApp.Interfaces;

namespace FileCabinetApp.CommandHandlers.ServiceHandlers
{
    /// <summary>
    /// Calss Update command handler.
    /// </summary>
    public class UpdateCommandHandler : ServiceCommandHandlerBase
    {
        private const string UpdateConstant = "update";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCommandHandler"/> class.
        /// </summary>
        /// <param name="fileCabinetService">The current service.</param>
        public UpdateCommandHandler(IFileCabinetService fileCabinetService)
            : base(fileCabinetService)
        {
        }

        /// <summary>
        /// Handle request.
        /// </summary>
        /// <param name="commandRequest">The command request.</param>
        public override void Handle(AppCommandRequest commandRequest)
        {
            if (commandRequest is null)
            {
                throw new ArgumentNullException($"{nameof(commandRequest)} cannot be null.");
            }

            if (string.Equals(commandRequest.Commands, UpdateConstant, StringComparison.OrdinalIgnoreCase))
            {
                this.Update(commandRequest.Parameters);
            }
            else
            {
                base.Handle(commandRequest);
            }
        }

        private void Update(string parameters)
        {
            var (newProp, whereProp) = this.Parse(parameters);

            var set = newProp;
            var where = whereProp;

            if (where[0].whereProp.Equals("id", StringComparison.OrdinalIgnoreCase))
            {
                int id = int.Parse(where[0].whereVal);
                foreach (var record in this.fileCabinetService.GetRecords())
                {
                    if (record.Id == id)
                    {
                        FileCabinetInputData data = this.CreateDataForEditing(record, set);
                        this.fileCabinetService.EditRecord(id, data);
                        return;
                    }
                }
            }

            var finded = new List<List<FileCabinetRecord>>();
            foreach (var item in where)
            {
                if (string.Equals(item.whereProp, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    var firstNames = new List<FileCabinetRecord>(this.fileCabinetService.FindByFirstName(item.whereVal));
                    finded.Add(firstNames);
                }
                else if (string.Equals(item.whereProp, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    var lastNames = new List<FileCabinetRecord>(this.fileCabinetService.FindByLastName(item.whereVal));
                    finded.Add(lastNames);
                }
                else if (string.Equals(item.whereProp, "dateOfBirth", StringComparison.OrdinalIgnoreCase))
                {
                    var dates = new List<FileCabinetRecord>(this.fileCabinetService.FindByDateOfBirth(Convert.ToDateTime(item.whereVal)));
                    finded.Add(dates);
                }
                else if (string.Equals(item.whereProp, "experience", StringComparison.OrdinalIgnoreCase))
                {
                    var experiences = new List<FileCabinetRecord>(this.fileCabinetService.FindByExperience(item.whereVal));
                    finded.Add(experiences);
                }
                else if (string.Equals(item.whereProp, "account", StringComparison.OrdinalIgnoreCase))
                {
                    var accounts = new List<FileCabinetRecord>(this.fileCabinetService.FindByAccount(item.whereVal));
                    finded.Add(accounts);
                }
                else if (string.Equals(item.whereProp, "gender", StringComparison.OrdinalIgnoreCase))
                {
                    var genders = new List<FileCabinetRecord>(this.fileCabinetService.FindByGender(item.whereVal));
                    finded.Add(genders);
                }
            }

            var updated = finded[0];
            foreach (var find in finded)
            {
                updated = this.Insert(updated, find);
            }

            foreach (var record in updated)
            {
                FileCabinetInputData data = this.CreateDataForEditing(record, set);
                this.fileCabinetService.EditRecord(record.Id, data);
            }
        }

        private (List<(string prop, string val)>, List<(string whereProp, string whereVal)>) Parse(string parameters)
        {
            var listNew = new List<(string, string)>();
            parameters = parameters.Substring(3);

            var arguments = parameters.Split("where");

            var newPropValue = arguments[0].Split(',');

            foreach (var item in newPropValue)
            {
                var itemArg = item.Split("=");
                listNew.Add((itemArg[0].Trim(' ', '\''), itemArg[1].Trim('\'', ' ')));
            }

            var listWhere = this.WhereParse(arguments[1]);

            return (listNew, listWhere);
        }

        private List<(string whereProp, string whereVal)> WhereParse(string parameters)
        {
            var arguments = parameters.Split("and");

            var listWhere = new List<(string, string)>();

            foreach (var arg in arguments)
            {
                var whereValues = arg.Split("=");
                listWhere.Add((whereValues[0].Trim(' ', '\''), whereValues[1].Trim('\'', ' ')));
            }

            return listWhere;
        }

        private FileCabinetInputData CreateDataForEditing(FileCabinetRecord record, List<(string prop, string value)> editParameters)
        {
            var data = new FileCabinetInputData(record.FirstName, record.LastName, record.DateOfBirth, record.Gender, record.Experience, record.Account);

            foreach (var item in editParameters)
            {
                if (string.Equals(item.prop, "firstName", StringComparison.OrdinalIgnoreCase))
                {
                    data.FirstName = item.value;
                }
                else if (string.Equals(item.prop, "lastName", StringComparison.OrdinalIgnoreCase))
                {
                    data.LastName = item.value;
                }
                else if (string.Equals(item.prop, "dateofbirth", StringComparison.OrdinalIgnoreCase))
                {
                    data.DateOfBirth = DateTime.Parse(item.value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.prop, "gender", StringComparison.OrdinalIgnoreCase))
                {
                    data.Gender = item.value[0];
                }
                else if (string.Equals(item.prop, "experience", StringComparison.OrdinalIgnoreCase))
                {
                    data.Experience = short.Parse(item.value, CultureInfo.InvariantCulture);
                }
                else if (string.Equals(item.prop, "account", StringComparison.OrdinalIgnoreCase))
                {
                    data.Account = decimal.Parse(item.value, CultureInfo.InvariantCulture);
                }
            }

            return data;
        }

        private List<FileCabinetRecord> Insert(List<FileCabinetRecord> lhs, List<FileCabinetRecord> rhs)
        {
            var result = new List<FileCabinetRecord>();
            foreach (var lhsItem in lhs)
            {
                foreach (var rhsItem in rhs)
                {
                    if (lhsItem.FirstName == rhsItem.FirstName &&
                        lhsItem.LastName == rhsItem.LastName &&
                        lhsItem.DateOfBirth == rhsItem.DateOfBirth &&
                        lhsItem.Experience == rhsItem.Experience &&
                        lhsItem.Account == rhsItem.Account &&
                        lhsItem.Gender == rhsItem.Gender)
                    {
                        result.Add(lhsItem);
                    }
                }
            }

            return result;
        }
    }
}
