using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class BrakeStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || input.Length != 2) return false;

            bool isPrefixDigit = char.IsDigit(input[0]);
            char suffix = char.ToUpper(input[1]);
            bool isSuffixValid = suffix == 'B' || suffix == '-';

            return isPrefixDigit && isSuffixValid;
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            int brakeTarget = int.Parse(input[0].ToString()); // '1' from "1B"
            if (brakeTarget == 0) brakeTarget = 10; // 0B is 10 brake

            for (int i = 0; i <= 9; i++)
            {
                for (int j = i; j <= 9; j++) // j=i to avoid duplicates (e.g., 10 and 01)
                {
                    if ((i + j) % 10 == brakeTarget % 10)
                    {
                        string num1 = $"{i}{j}";
                        string num2 = $"{j}{i}";

                        yield return new ParseResult { Number = num1, Amount = parsedAmount };

                        if (num1 != num2)
                        {
                            yield return new ParseResult { Number = num2, Amount = parsedAmount };
                        }
                    }
                }
            }
        }
    }
}