namespace FileCabinetApp.CommandHandlers.CommandAgrsHandlers
{
    /// <summary>
    /// Class provide constant for work with command arguments.
    /// </summary>
    public static class CommandArgConstant
    {
        /// <summary>
        /// Validation rule in full form.
        /// </summary>
        public const string VALIDATIONRULE = "--validation-rules";

        /// <summary>
        /// Validation rule in short form.
        /// </summary>
        public const string VALIDATIONRULESHORT = "-v";

        /// <summary>
        /// Storage in full form.
        /// </summary>
        public const string STORAGE = "--storage";

        /// <summary>
        /// Storage in short form.
        /// </summary>
        public const string STORAGESHORT = "-s";

        /// <summary>
        /// Stopwatch(only full form).
        /// </summary>
        public const string STOPWATCH = "-use-stopwatch";

        /// <summary>
        /// Logger(only full form).
        /// </summary>
        public const string LOGGER = "-use-logger";

        /// <summary>
        /// Single commands.
        /// </summary>
        public static string[] SingleCommands = { STOPWATCH, LOGGER };
    }
}
