using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LuckyBet.Core.Models;

public class Voucher
{
    public long VoucherId { get; set; }
    public string VoucherNumber { get; set; } = string.Empty;
    public string VoucherType { get; set; } = string.Empty; // "Customer" or "Upline"
    public long ContactId { get; set; }
    public string Session { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public long UserId { get; set; }

    public Contact? Contact { get; set; }
    public User? User { get; set; }
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}