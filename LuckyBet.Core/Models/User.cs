using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuckyBet.Core.Models;

public class User
{
    public long UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();
    public ICollection<UplineSale> UplineSales { get; set; } = new List<UplineSale>();
}
