using System.ComponentModel.DataAnnotations;

namespace LuckyBet.Core.Models
{
    public class Setting
    {
        [Key] // This is the Primary Key
        public string SettingKey { get; set; } = string.Empty;
        public string SettingValue { get; set; } = string.Empty;
    }
}