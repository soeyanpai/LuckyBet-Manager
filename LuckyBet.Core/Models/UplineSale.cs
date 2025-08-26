using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyBet.Core.Models;

public class UplineSale
{
    public long UplineSaleId { get; set; }
    public long ContactId { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Session { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public long UserId { get; set; }

    public Contact? Contact { get; set; }    
    public User? User { get; set; }
}
