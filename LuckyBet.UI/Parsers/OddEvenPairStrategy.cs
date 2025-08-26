using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class OddEvenPairStrategy : IParserStrategy
    {
        // Allowed shortcuts
        private readonly HashSet<string> _validShortcuts = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
        {
            "SS", "++", "MM", "--", "SM", "+-", "MS", "-+"
        };

        public bool IsApplicable(string input)
        {
            // User ရိုက်လိုက်တဲ့ input က ကျွန်တော်တို့ သတ်မှတ်ထားတဲ့ shortcut တွေထဲမှာ ပါရဲ့လား
            return !string.IsNullOrWhiteSpace(input) && _validShortcuts.Contains(input);
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            if (!decimal.TryParse(amount, out decimal parsedAmount))
            {
                yield break;
            }

            int[] evens = { 0, 2, 4, 6, 8 };
            int[] odds = { 1, 3, 5, 7, 9 };

            int[] firstDigitSet;
            int[] secondDigitSet;

            // User ရိုက်လိုက်တဲ့ shortcut အပေါ်မူတည်ပြီး ဘယ်ဂဏန်း set တွေကို သုံးမလဲ ဆုံးဖြတ်ပါ
            string normalizedInput = input.ToUpper();
            if (normalizedInput == "SS" || normalizedInput == "++")
            {
                firstDigitSet = evens;
                secondDigitSet = evens;
            }
            else if (normalizedInput == "MM" || normalizedInput == "--")
            {
                firstDigitSet = odds;
                secondDigitSet = odds;
            }
            else if (normalizedInput == "SM" || normalizedInput == "+-")
            {
                firstDigitSet = evens;
                secondDigitSet = odds;
            }
            else // MS or -+
            {
                firstDigitSet = odds;
                secondDigitSet = evens;
            }

            // သက်ဆိုင်ရာ set တွေကို သုံးပြီး ဂဏန်း ၂၅ ကွက် ထုတ်ပေးပါ
            foreach (var firstDigit in firstDigitSet)
            {
                foreach (var secondDigit in secondDigitSet)
                {
                    yield return new ParseResult
                    {
                        Number = $"{firstDigit}{secondDigit}",
                        Amount = parsedAmount
                    };
                }
            }
        }
    }
}