using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.Data
{
    /// <summary>
    /// class CashedData contains cashed data using for memoization.
    /// </summary>
    public static class CashedData
    {
        /// <summary>
        /// Date cache.
        /// </summary>
        public static readonly Dictionary<DateTime, IEnumerable<FileCabinetRecord>> DateOfBirthCashe =
            new Dictionary<DateTime, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// FirstName cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> FirstNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// LastName cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> LastNameCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Experience cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> ExperienceCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Account cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> AccountCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Gender cache.
        /// </summary>
        public static readonly Dictionary<string, IEnumerable<FileCabinetRecord>> GenderCashe =
            new Dictionary<string, IEnumerable<FileCabinetRecord>>();

        /// <summary>
        /// Clear all cache.
        /// </summary>
        public static void ClearCashe()
        {
            DateOfBirthCashe.Clear();
            FirstNameCashe.Clear();
            LastNameCashe.Clear();
            ExperienceCashe.Clear();
            AccountCashe.Clear();
            GenderCashe.Clear();
        }
    }
}
