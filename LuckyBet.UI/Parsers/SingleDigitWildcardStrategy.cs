using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.UI.Parsers
{
    public class SingleDigitWildcardStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 2) return false;

            // ပထမက '*' ဒုတိယက ဂဏန်း (e.g., *5)
            bool isPrefixWildcard = input[0] == '*' && char.IsDigit(input[1]);
            // ပထမက ဂဏန်း ဒုတိယက '*' (e.g., 6*)
            bool isSuffixWildcard = char.IsDigit(input[0]) && input[1] == '*';

            return isPrefixWildcard || isSuffixWildcard;
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            if (input[0] == '*') // *N case
            {
                char fixedDigit = input[1];
                for (int i = 0; i <= 9; i++)
                {
                    yield return new ParseResult { Number = $"{i}{fixedDigit}", Amount = parsedAmount };
                }
            }
            else // N* case
            {
                char fixedDigit = input[0];
                for (int i = 0; i <= 9; i++)
                {
                    yield return new ParseResult { Number = $"{fixedDigit}{i}", Amount = parsedAmount };
                }
            }
        }
    }
}