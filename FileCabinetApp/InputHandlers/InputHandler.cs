using System;
using System.Globalization;

namespace FileCabinetApp.InputHandlers
{
    /// <summary>
    /// Class Input handlers.
    /// </summary>
    public static class InputHandler
    {
        #region Converters

        /// <summary>
        /// String converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, string>> StringConvrter = input =>
        {
            return new Tuple<bool, string, string>(true, input, input);
        };

        /// <summary>
        /// DateOfBirth converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, DateTime>> DateConverter = input =>
        {
            DateTime date;
            bool isValid = DateTime.TryParse(input, new CultureInfo("en-US"), DateTimeStyles.None, out date);

            return new Tuple<bool, string, DateTime>(isValid, input, date);
        };

        /// <summary>
        /// Experience converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, short>> ExperienceConverter = input =>
        {
            short experience;
            bool isValid = short.TryParse(input, out experience);

            return new Tuple<bool, string, short>(isValid, input, experience);
        };

        /// <summary>
        /// Account converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, decimal>> AccountConverter = input =>
        {
            decimal account;
            bool isValid = decimal.TryParse(input, out account);

            return new Tuple<bool, string, decimal>(isValid, input, account);
        };

        /// <summary>
        /// Gender converter.
        /// </summary>
        public static Func<string, Tuple<bool, string, char>> GenderConverter = input =>
        {
            char gender;
            bool isValid = char.TryParse(input, out gender);

            return new Tuple<bool, string, char>(isValid, input, gender);
        };

        #endregion

        #region Validators

        /// <summary>
        /// FirstName validator.
        /// </summary>
        public static Func<string, Tuple<bool, string>> FirstNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        /// <summary>
        /// LastName validator.
        /// </summary>
        public static Func<string, Tuple<bool, string>> LastNameValidator = input =>
        {
            bool isValid = !(string.IsNullOrWhiteSpace(input) || input.Length < 2 || input.Length > 60);
            return new Tuple<bool, string>(isValid, input);
        };

        /// <summary>
        /// DateOfBirth validator.
        /// </summary>
        public static Func<DateTime, Tuple<bool, string>> DateOfBirthValidator = date =>
        {
            bool isValid = !(date < new DateTime(1950, 1, 1) || date > DateTime.Now);
            return new Tuple<bool, string>(isValid, date.ToString());
        };

        /// <summary>
        /// Experience validator.
        /// </summary>
        public static Func<short, Tuple<bool, string>> ExperienceValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        /// <summary>
        /// Account validator.
        /// </summary>
        public static Func<decimal, Tuple<bool, string>> AccountValidator = input =>
        {
            bool isValid = input >= 0;
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        /// <summary>
        /// Gender validator.
        /// </summary>
        public static Func<char, Tuple<bool, string>> GenderValidator = input =>
        {
            bool isValid = input == 'm' || input == 'f' || input == 'M' || input == 'F';
            return new Tuple<bool, string>(isValid, input.ToString());
        };

        #endregion

        /// <summary>
        /// Read input data.
        /// </summary>
        /// <typeparam name="T">Data.</typeparam>
        /// <param name="converter">Data converter delegate.</param>
        /// <param name="validator">Data validator delegate.</param>
        /// <returns>Validated data.</returns>
        public static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }
    }
}
