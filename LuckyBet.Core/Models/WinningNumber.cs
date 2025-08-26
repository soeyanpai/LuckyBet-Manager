using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyBet.Core.Models;

public class WinningNumber
{
    public long WinningNumberId { get; set; }
    public DateTime Date { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Number { get; set; } = string.Empty;
}
