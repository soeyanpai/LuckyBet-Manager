using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.UI.Parsers
{
    public class NyikoStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            // Input က "NK" (အကြီး/အသေး) ဖြစ်ရမယ်
            return !string.IsNullOrWhiteSpace(input) &&
                   input.Equals("NK", System.StringComparison.OrdinalIgnoreCase);
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount))
            {
                yield break;
            }

            // ညီကို ဂဏန်းအတွဲများ (၂၀ ကွက်)
            string[] nyikoNumbers = {
                "01", "12", "23", "34", "45", "56", "67", "78", "89", "90",
                "10", "21", "32", "43", "54", "65", "76", "87", "98", "09"
            };

            foreach (var number in nyikoNumbers)
            {
                yield return new ParseResult
                {
                    Number = number,
                    Amount = parsedAmount
                };
            }
        }
    }
}