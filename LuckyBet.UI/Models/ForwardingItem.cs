using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LuckyBet.UI.Models
{
    public class ForwardingItem : INotifyPropertyChanged
    {
        public string Number { get; set; } = string.Empty;
        public decimal OriginalOverAmount { get; set; }

        private decimal _forwardAmount;
        public decimal ForwardAmount
        {
            get => _forwardAmount;
            set
            {
                // --- Validation Logic အသစ် ---
                // 1. တန်ဖိုးက 0 ထက် ငယ်နေရင် 0 လို့ပဲ သတ်မှတ်ပါ (အနုတ် မဖြစ်စေရ)
                if (value < 0)
                {
                    _forwardAmount = 0;
                }
                // 2. တန်ဖိုးက မူလကျော်လွန်ငွေထက် ကြီးနေရင်၊ မူလကျော်လွန်ငွေကိုပဲ အများဆုံးအဖြစ် သတ်မှတ်ပါ
                else if (value > OriginalOverAmount)
                {
                    _forwardAmount = OriginalOverAmount;
                }
                else
                {
                    // အထက်ပါ ၂ ချက်လုံးနဲ့ မငြိမှ User ရိုက်ထည့်လိုက်တဲ့ တန်ဖိုးကို လက်ခံပါ
                    _forwardAmount = value;
                }

                // UI ကို အပြောင်းအလဲရှိကြောင်း အကြောင်းကြားပါ
                OnPropertyChanged();
                OnPropertyChanged(nameof(UnforwardedAmount));
            }
        }

        // ကျန်ငွေကို အလိုအလျောက် တွက်ချက်ပေးမယ့် Read-only property
        public decimal UnforwardedAmount => OriginalOverAmount - ForwardAmount;

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}