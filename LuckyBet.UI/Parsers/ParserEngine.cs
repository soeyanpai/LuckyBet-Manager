using LuckyBet.Core.Parsers;
using LuckyBet.Core.Parsers.Models;
using System.Collections.Generic;
using System.Linq;

namespace LuckyBet.UI.Parsers
{
    public class ParserEngine
    {
        private readonly List<IParserStrategy> _strategies;

        public ParserEngine()
        {
            _strategies = new List<IParserStrategy>
    {
        // Specific (no numbers or complex rules)
        new SpecificPairStrategy(),  // <-- SP, MP, W, N
        new OddEvenPairStrategy(),
        new PowerStrategy(),
        new NyikoStrategy(),

        // More Specific (number + symbol)
        new PermutationStrategy(), // <-- 123.
        new OddEvenSingleStrategy(), // <-- S2, 5S
        new SingleDigitWildcardStrategy(),
        new NumberPahtStrategy(),
        new BrakeStrategy(),
        new RoundStrategy(), // Round should be after other 2-char + symbol strategies
        
        // General (all digits)
        new DirectNumberStrategy()
    };
        }

        public IEnumerable<ParseResult> Process(string numberInput, string amountInput, string mode)
        {
            var strategy = _strategies.FirstOrDefault(s => s.IsApplicable(numberInput));
            if (strategy != null)
            {
                return strategy.Parse(numberInput, amountInput, mode);
            }
            return Enumerable.Empty<ParseResult>();
        }

    }
}