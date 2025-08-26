using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.UI.Parsers
{
    public class PowerStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            // Input က "P" (အကြီး/အသေး) ဒါမှမဟုတ် "**" ဖြစ်ရမယ်
            return !string.IsNullOrWhiteSpace(input) &&
                   (input.Equals("P", System.StringComparison.OrdinalIgnoreCase) || input.Equals("**"));
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount))
            {
                yield break; // Amount မှားနေရင် ဘာမှမလုပ်ပါ
            }

            // အပူး ၁၀ ကွက်လုံးကို ထုတ်ပေးပါ
            for (int i = 0; i <= 9; i++)
            {
                yield return new ParseResult
                {
                    Number = $"{i}{i}", // "00", "11", "22", ...
                    Amount = parsedAmount
                };
            }
        }
    }
}