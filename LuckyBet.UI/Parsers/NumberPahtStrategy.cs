using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.UI.Parsers
{
    public class NumberPahtStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 2) return false;

            bool isPrefixDigit = char.IsDigit(input[0]);
            char suffix = char.ToUpper(input[1]);
            bool isSuffixValid = suffix == 'P' || suffix == '/';

            return isPrefixDigit && isSuffixValid;
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            char targetDigit = input[0]; // '1' from "1P"

            for (int i = 0; i <= 99; i++)
            {
                string number = i.ToString("D2"); // "00", "01", ... "99"
                if (number[0] == targetDigit || number[1] == targetDigit)
                {
                    yield return new ParseResult { Number = number, Amount = parsedAmount };
                }
            }
        }
    }
}