using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;

namespace LuckyBet.Core.Parsers
{
    public interface IParserStrategy
    {
        // ဒီ strategy က ဘယ်လို shortcut မျိုးကို တာဝန်ယူမှာလဲ (e.g., "R", "*", "/")
        bool IsApplicable(string input);

        // တကယ် အလုပ်လုပ်မယ့် method
        IEnumerable<ParseResult> Parse(string input, string amount, string mode);
    }
}
