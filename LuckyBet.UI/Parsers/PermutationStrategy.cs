using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class PermutationStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            if (string.IsNullOrWhiteSpace(input) || !input.EndsWith(".")) return false;

            string numberPart = input.TrimEnd('.'); // Trim all dots first
            if (string.IsNullOrEmpty(numberPart)) return false;

            // After trimming dots, the rest must be digits
            return numberPart.All(char.IsDigit);
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount)) yield break;

            bool includeSame = input.EndsWith("..");
            string numberPart = input.TrimEnd('.');

            // ".." case မှာ မူလ ဂဏန်းတွေကို တိုက်ရိုက်သုံးပါ (e.g., from "112..")
            var digitPool = includeSame ? numberPart.ToCharArray() : numberPart.ToCharArray().Distinct();

            foreach (var d1 in digitPool)
            {
                foreach (var d2 in digitPool)
                {
                    // "." case မှာ အပူးတွေကို skip လုပ်ပါ
                    if (!includeSame && d1 == d2)
                    {
                        continue;
                    }
                    yield return new ParseResult { Number = $"{d1}{d2}", Amount = parsedAmount };
                }
            }
        }
    }
}