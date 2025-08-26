using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.UI.Parsers
{
    public class SpecificPairStrategy : IParserStrategy
    {
        private readonly Dictionary<string, string[]> _shortcutMap =
            new Dictionary<string, string[]>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "SP", new[] { "00", "22", "44", "66", "88" } },
            { "+*", new[] { "00", "22", "44", "66", "88" } },
            { "MP", new[] { "11", "33", "55", "77", "99" } },
            { "-*", new[] { "11", "33", "55", "77", "99" } },
            { "W", new[] { "05", "50", "16", "61", "27", "72", "38", "83", "49", "94" } },
            { "N", new[] { "07", "70", "18", "81", "24", "42", "35", "53", "69", "96" } }
        };

        public bool IsApplicable(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && _shortcutMap.ContainsKey(input);
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            if (_shortcutMap.TryGetValue(input, out var numbers))
            {
                foreach (var number in numbers)
                {
                    yield return new ParseResult { Number = number, Amount = parsedAmount };
                }
            }
        }
    }
}