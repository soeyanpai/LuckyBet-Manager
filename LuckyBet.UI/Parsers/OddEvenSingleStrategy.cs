using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class OddEvenSingleStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 2) return false;

            char first = char.ToUpper(input[0]);
            char second = char.ToUpper(input[1]);

            // S2 or 5S pattern
            bool isSPattern = (first == 'S' && char.IsDigit(second)) || (char.IsDigit(first) && second == 'S');
            // M4 or 6M pattern
            bool isMPattern = (first == 'M' && char.IsDigit(second)) || (char.IsDigit(first) && second == 'M');

            return isSPattern || isMPattern;
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            int[] evens = { 0, 2, 4, 6, 8 };
            int[] odds = { 1, 3, 5, 7, 9 };

            char first = char.ToUpper(input[0]);
            char second = char.ToUpper(input[1]);

            if (first == 'S') // S2 case
            {
                foreach (var d in evens) yield return new ParseResult { Number = $"{d}{second}", Amount = parsedAmount };
            }
            else if (second == 'S') // 5S case
            {
                foreach (var d in evens) yield return new ParseResult { Number = $"{first}{d}", Amount = parsedAmount };
            }
            else if (first == 'M') // M4 case
            {
                foreach (var d in odds) yield return new ParseResult { Number = $"{d}{second}", Amount = parsedAmount };
            }
            else // 6M case
            {
                foreach (var d in odds) yield return new ParseResult { Number = $"{first}{d}", Amount = parsedAmount };
            }
        }
    }
}