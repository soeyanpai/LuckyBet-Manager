using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class RoundStrategy : IParserStrategy
    {
        public bool IsApplicable(string input)
        {
            // Input က null မဖြစ်ရဘူး၊ အရှည် ၃ လုံး (e.g., 12R) ဒါမှမဟုတ် (12*) ဖြစ်ရမယ်။
            if (string.IsNullOrWhiteSpace(input) || input.Length != 3)
            {
                return false;
            }

            // ရှေ့ဆုံး ၂ လုံးက ဂဏန်းဖြစ်ရမယ်။
            bool isPrefixNumber = char.IsDigit(input[0]) && char.IsDigit(input[1]);

            // နောက်ဆုံးတစ်လုံးက 'R' (အကြီး/အသေး) ဒါမှမဟုတ် '*' ဖြစ်ရမယ်။
            char suffix = char.ToUpper(input[2]);
            bool isSuffixValid = suffix == 'R' || suffix == '*';

            return isPrefixNumber && isSuffixValid;
        }

        public IEnumerable<ParseResult> Parse(string input, string amount, string _mode)
        {
            // decimal.TryParse ကို သုံးပြီး amount string ကို ဂဏန်းအဖြစ် ပြောင်းကြည့်ပါ
            // ဒါက ပြောင်းလို့မရရင် error မတက်ဘဲ false ဆိုတဲ့ အဖြေကိုပဲ ပြန်ပေးပါတယ်
            bool TryParseAmount(string amtStr, out decimal parsedAmount)
            {
                return decimal.TryParse(amtStr, out parsedAmount);
            }

            string numberPart = input.Substring(0, 2);
            string reversedNumberPart = new string(numberPart.Reverse().ToArray());

            var amounts = amount.Split('/');

            // ပထမ amount ကို ပြောင်းကြည့်ပါ၊ မအောင်မြင်ရင် ဒီ Strategy က အလုပ်မလုပ်တော့ပါ
            if (!TryParseAmount(amounts[0], out decimal amount1))
            {
                yield break; // 'yield break' means stop and return an empty list
            }

            // ဒုတိယ amount ရှိခဲ့ရင် သူ့ကိုလည်း ပြောင်းကြည့်ပါ၊ မအောင်မြင်ရင် ပထမ amount ကိုပဲ သုံးပါ
            decimal amount2 = amount1;
            if (amounts.Length > 1 && TryParseAmount(amounts[1], out decimal parsedAmount2))
            {
                amount2 = parsedAmount2;
            }

            yield return new ParseResult { Number = numberPart, Amount = amount1 };

            if (numberPart != reversedNumberPart)
            {
                yield return new ParseResult { Number = reversedNumberPart, Amount = amount2 };
            }
        }
    }
}