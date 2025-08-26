using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LuckyBet.Core.Parsers.Models
{
    public class ParseResult
    {
        public string Number { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}