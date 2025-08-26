// LuckyBet.UI/Parsers/DirectNumberStrategy.cs
using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class DirectNumberStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            return !string.IsNullOrWhiteSpace(input) && input.All(char.IsDigit);
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string mode)
        {
            // --- NEW VALIDATION LOGIC ---
            bool isValid = false;
            if (mode == "2D" && input.Length == 2)
            {
                isValid = true; // 2D Mode မှာ 2 လုံးဂဏန်းဆိုရင် OK
            }
            else if (mode == "3D" && input.Length == 3)
            {
                isValid = true; // 3D Mode မှာ 3 လုံးဂဏန်းဆိုရင် OK
            }

            if (isValid && decimal.TryParse(amount, out decimal parsedAmount))
            {
                yield return new ParseResult
                {
                    Number = input,
                    Amount = parsedAmount
                };
            }
        }
    }
}