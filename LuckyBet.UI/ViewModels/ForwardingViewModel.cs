using LuckyBet.Core.Models;
using LuckyBet.UI.Commands;
using LuckyBet.UI.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LuckyBet.UI.ViewModels
{
    public class ForwardingViewModel : INotifyPropertyChanged
    {
        // --- Properties ---
        public ObservableCollection<ForwardingItem> ForwardingItems { get; set; }
        public List<Contact> UplineList { get; set; } = new List<Contact>();

        private Contact? _selectedUpline;
        public Contact? SelectedUpline
        {
            get => _selectedUpline;
            set { _selectedUpline = value; OnPropertyChanged(); }
        }

        // --- Commands ---
        public ICommand ConfirmForwardCommand { get; }

        public ForwardingViewModel(ObservableCollection<TotalBoardItem> overLimitItems, List<Contact> uplineList)
        {
            // MainDashboard ကနေ လက်ခံရရှိတဲ့ data တွေကို ပြင်ဆင်ပါ
            var items = overLimitItems.Select(item => new ForwardingItem
            {
                Number = item.Number,
                OriginalOverAmount = item.TotalAmount, // ဒါက ကျော်လွန်ငွေ အပေါင်းကိန်း အစစ်ပါ
                ForwardAmount = item.TotalAmount       // Default အနေနဲ့ အကုန်လွှဲမယ်လို့ သတ်မှတ်
            });
            ForwardingItems = new ObservableCollection<ForwardingItem>(items);
            UplineList = uplineList;

            // Initialize Commands
            ConfirmForwardCommand = new RelayCommand(ConfirmForward);
        }

        private void ConfirmForward(object? parameter)
        {
            // 1. Validation
            if (SelectedUpline == null)
            {
                MessageBox.Show("ကျေးဇူးပြု၍ အထက်ဒိုင်တစ်ယောက်ကို ရွေးချယ်ပါ။", "အသိပေးချက်", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // လွှဲမယ့် ပမာဏက 0 ထက်ကြီးတဲ့ စာရင်းတွေကိုပဲ ရွေးထုတ်ပါ
            var itemsToForward = ForwardingItems.Where(item => item.ForwardAmount > 0).ToList();

            if (!itemsToForward.Any())
            {
                MessageBox.Show("လွှဲပြောင်းရန် စာရင်းများ မရှိပါ။ (လွှဲမည့် ပမာဏတွင် 0 ထက်ကြီးသော တန်ဖိုးများ ထည့်ပါ)", "အသိပေးချက်", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 2. Database Saving Logic
            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                try
                {
                    decimal totalForwardAmount = itemsToForward.Sum(item => item.ForwardAmount);

                    var newUplineVoucher = new Voucher
                    {
                        VoucherType = "Upline",
                        ContactId = SelectedUpline.ContactId, // Use ID directly
                        VoucherNumber = "000", // TODO: Upline voucher numbering system
                        Date = DateTime.Now,
                        TotalAmount = totalForwardAmount,
                        UserId = 1, // TODO: Get current user
                        Session = "2D Upline Forward" // TODO: Get session from main dashboard
                    };

                    foreach (var item in itemsToForward)
                    {
                        newUplineVoucher.Sales.Add(new Sale
                        {
                            Number = item.Number,
                            Amount = item.ForwardAmount,
                            Type = "2D" // TODO: Update based on session
                        });
                    }
                    context.Vouchers.Add(newUplineVoucher);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"စာရင်းလွှဲရာတွင် Database အမှားအယွင်းဖြစ်ပွားပါသည်:\n\n{ex.ToString()}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return; // Error ဖြစ်ရင် ဆက်မလုပ်ပါနဲ့
                }
            }

            // 3. Close the dialog with a success result
            if (parameter is Window dialog)
            {
                dialog.DialogResult = true;
                dialog.Close();
            }
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}