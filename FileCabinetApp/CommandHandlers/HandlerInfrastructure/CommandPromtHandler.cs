using System;
using System.Collections.Generic;
using System.Text;

namespace FileCabinetApp.CommandHandlers.HandlerInfrastructure
{
    /// <summary>
    /// Class Command promt handler.
    /// </summary>
    public static class CommandPromtHandler
    {
        private const int SubtokenLength = 3;
        private const double ThresholdWord = 0.5d;

        private const string Are = "are";
        private const string Is = "is";
        private static string[] commands =
        {
            "select",
            "update",
            "exit",
            "help",
            "delete",
            "import",
            "purge",
            "stat",
            "insert",
            "export",
            "create",
        };

        /// <summary>
        /// Get most simmular strings.
        /// </summary>
        /// <param name="incorrect">Incorrect string.</param>
        public static void GetTheMostSimular(string incorrect)
        {
            if (incorrect is null)
            {
                throw new ArgumentNullException($"{nameof(incorrect)} cannot be null.");
            }

            var sb = new StringBuilder();
            int count = 0;
            foreach (var command in commands)
            {
                if (IsSimular(command, incorrect))
                {
                    sb.Append($"{Environment.NewLine}{command}");
                    count++;
                }
            }

            if (count == 0)
            {
                return;
            }

            if (count == 1)
            {
                Console.WriteLine($"The most simmular command {Are}: {sb}");
            }
            else
            {
                Console.WriteLine($"The most simmular command {Is}: {sb}");
            }
        }

        private static bool IsSimular(string firstToken, string secondToken)
        {
            var equalSubtokensCount = 0;
            var usedTokens = new bool[secondToken.Length - SubtokenLength + 1];
            for (var i = 0; i < firstToken.Length - SubtokenLength + 1; ++i)
            {
                var subtokenFirst = firstToken.Substring(i, SubtokenLength);
                for (var j = 0; j < secondToken.Length - SubtokenLength + 1; ++j)
                {
                    if (!usedTokens[j])
                    {
                        var subtokenSecond = secondToken.Substring(j, SubtokenLength);
                        if (subtokenFirst.Equals(subtokenSecond, StringComparison.Ordinal))
                        {
                            equalSubtokensCount++;
                            usedTokens[j] = true;
                            break;
                        }
                    }
                }
            }

            var subtokenFirstCount = firstToken.Length - SubtokenLength + 1;
            var subtokenSecondCount = secondToken.Length - SubtokenLength + 1;

            var tanimoto = (1.0 * equalSubtokensCount) / (subtokenFirstCount + subtokenSecondCount - equalSubtokensCount);

            return tanimoto >= ThresholdWord;
        }
    }
}
