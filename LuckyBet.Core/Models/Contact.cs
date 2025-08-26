using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace LuckyBet.Core.Models; // <-- လိပ်စာကို ဒီလိုပုံစံပြောင်းလိုက်ပါတယ်

public class Contact // <-- 'internal' ကို 'public' လို့ပြောင်းလိုက်ပါတယ်
{
    public long ContactId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactType { get; set; } = string.Empty; // "Customer" or "Upline"
    public string? Phone { get; set; }
    public decimal CommissionRate { get; set; }
    public int PayoutRate2D { get; set; }
    public int PayoutRate3D { get; set; }

    // Navigation Properties
    public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    public ICollection<UplineSale> UplineSales { get; set; } = new List<UplineSale>();
}