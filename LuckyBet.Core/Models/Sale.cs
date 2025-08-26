using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyBet.Core.Models;

public class Sale
{
    public long SaleId { get; set; }
    public long VoucherId { get; set; }
    public string Number { get; set; } = string.Empty;
    public decimal Amount { get; set; } 
    public string Type { get; set; } = string.Empty;

    public Voucher? Voucher { get; set; } 
}