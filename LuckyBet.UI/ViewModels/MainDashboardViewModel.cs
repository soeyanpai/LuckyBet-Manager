using LuckyBet.Core.Models;
using LuckyBet.Core.Parsers.Models;
using LuckyBet.UI.Commands;
using LuckyBet.UI.Models;
using LuckyBet.UI.Parsers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LuckyBet.UI.ViewModels
{
    public class MainDashboardViewModel : INotifyPropertyChanged
    {
        private readonly ParserEngine _parserEngine;
        private readonly string _currentSessionMode;

        // --- Session Properties ---
        private DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set { _selectedDate = value; OnPropertyChanged(); UpdateSessionTitle(); }
        }

        private string _sessionTitle = string.Empty;
        public string SessionTitle
        {
            get => _sessionTitle;
            set { _sessionTitle = value; OnPropertyChanged(); }
        }

        private bool _isTwoDSessionVisible;
        public bool IsTwoDSessionVisible
        {
            get => _isTwoDSessionVisible;
            set { _isTwoDSessionVisible = value; OnPropertyChanged(); }
        }

        private bool _isThreeDSessionVisible;
        public bool IsThreeDSessionVisible
        {
            get => _isThreeDSessionVisible;
            set { _isThreeDSessionVisible = value; OnPropertyChanged(); }
        }

        // --- Limit Properties ---
        private decimal _limitAmount;
        public decimal LimitAmount
        {
            get => _limitAmount;
            set { _limitAmount = value; OnPropertyChanged(); OnPropertyChanged(nameof(WarningAmount)); }
        }
        public decimal WarningAmount => LimitAmount * 0.8m;

        private string _limitAmountInput = string.Empty;
        public string LimitAmountInput
        {
            get => _limitAmountInput;
            set { _limitAmountInput = value; OnPropertyChanged(); }
        }

        // --- Customer & Voucher Properties ---
        private List<Contact> _customerList = new();
        public List<Contact> CustomerList
        {
            get => _customerList;
            set { _customerList = value; OnPropertyChanged(); }
        }

        private Contact? _selectedCustomer;
        public Contact? SelectedCustomer
        {
            get => _selectedCustomer;
            set { _selectedCustomer = value; OnPropertyChanged(); }
        }

        private string _nextVoucherNumber = string.Empty;
        public string NextVoucherNumber
        {
            get => _nextVoucherNumber;
            set { _nextVoucherNumber = value; OnPropertyChanged(); }
        }

        // --- Manual Entry Properties ---
        private string _numberInput = string.Empty;
        public string NumberInput
        {
            get => _numberInput;
            set { _numberInput = value; OnPropertyChanged(); }
        }

        private string _amountInput = string.Empty;
        public string AmountInput
        {
            get => _amountInput;
            set { _amountInput = value; OnPropertyChanged(); }
        }

        // --- Staging & Total Board Properties ---
        public ObservableCollection<ParseResult> StagedEntries { get; set; }
        public ObservableCollection<TotalBoardItem> TotalBoardColumn1 { get; set; } = new();
        public ObservableCollection<TotalBoardItem> TotalBoardColumn2 { get; set; } = new();
        public ObservableCollection<TotalBoardItem> TotalBoardColumn3 { get; set; } = new();
        public ObservableCollection<TotalBoardItem> TotalBoardColumn4 { get; set; } = new();

        private decimal _grandTotalAmount;
        public decimal GrandTotalAmount
        {
            get => _grandTotalAmount;
            set { _grandTotalAmount = value; OnPropertyChanged(); }
        }

        // --- Over-Limit Panel Properties ---
        public ObservableCollection<TotalBoardItem> OverLimitItems { get; set; }
        public List<Contact> UplineList { get; set; } = new();
        private Contact? _selectedUpline;
        public Contact? SelectedUpline
        {
            get => _selectedUpline;
            set { _selectedUpline = value; OnPropertyChanged(); }
        }
        private decimal _totalOverLimitAmount;
        public decimal TotalOverLimitAmount
        {
            get => _totalOverLimitAmount;
            set { _totalOverLimitAmount = value; OnPropertyChanged(); }
        }


        // --- Commands ---
        public ICommand AddEntryCommand { get; }
        public ICommand RemoveEntryCommand { get; }
        public ICommand ClearEntriesCommand { get; }
        public ICommand SaveVoucherCommand { get; }
        public ICommand ForwardToUplineCommand { get; }
        public ICommand SetLimitCommand { get; }


        public MainDashboardViewModel(string session)
        {
            _parserEngine = new ParserEngine();
            _currentSessionMode = session;
            SelectedDate = DateTime.Today;

            // Initialize Collections
            StagedEntries = new();
            OverLimitItems = new();
            InitializeTotalBoard();
            StagedEntries.CollectionChanged += (s, e) => { /* Can be used for live staging summary later */ };

            // Initialize Commands
            AddEntryCommand = new RelayCommand(AddEntry);
            RemoveEntryCommand = new RelayCommand(RemoveEntry);
            ClearEntriesCommand = new RelayCommand(ClearEntries);
            SaveVoucherCommand = new RelayCommand(SaveVoucher);
            ForwardToUplineCommand = new RelayCommand(ForwardToUpline);
            SetLimitCommand = new RelayCommand(SetLimit);

            // Setup UI based on session
            UpdateSessionTitle();
            IsTwoDSessionVisible = session == "2D";
            IsThreeDSessionVisible = session == "3D";

            // Load initial data
            LoadDataOnStartup();
            GenerateNextVoucherNumber();
        }

        // --- Data Loading and Initialization ---
        private void LoadDataOnStartup()
        {
            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                // Load Settings
                var limitSetting = context.Settings.FirstOrDefault(s => s.SettingKey == "Limit2D");
                if (limitSetting != null && decimal.TryParse(limitSetting.SettingValue, out decimal dbLimit))
                {
                    LimitAmount = dbLimit;
                }
                else
                {
                    LimitAmount = 10000;
                }
                LimitAmountInput = LimitAmount.ToString("N0");

                // Load Contacts
                CustomerList = context.Contacts.Where(c => c.ContactType == "Customer").ToList();
                UplineList = context.Contacts.Where(c => c.ContactType == "Upline").ToList();

                // Load Committed Totals
                LoadCommittedTotals();
            }
        }

        private void InitializeTotalBoard()
        {
            var col1 = new List<TotalBoardItem>();
            var col2 = new List<TotalBoardItem>();
            var col3 = new List<TotalBoardItem>();
            var col4 = new List<TotalBoardItem>();

            for (int i = 0; i < 100; i++)
            {
                var newItem = new TotalBoardItem { Number = i.ToString("D2"), TotalAmount = 0 };
                if (i <= 24) col1.Add(newItem);
                else if (i <= 49) col2.Add(newItem);
                else if (i <= 74) col3.Add(newItem);
                else col4.Add(newItem);
            }

            TotalBoardColumn1 = new ObservableCollection<TotalBoardItem>(col1);
            TotalBoardColumn2 = new ObservableCollection<TotalBoardItem>(col2);
            TotalBoardColumn3 = new ObservableCollection<TotalBoardItem>(col3);
            TotalBoardColumn4 = new ObservableCollection<TotalBoardItem>(col4);
        }

        private void LoadCommittedTotals()
        {
            var allItems = TotalBoardColumn1.Concat(TotalBoardColumn2).Concat(TotalBoardColumn3).Concat(TotalBoardColumn4);
            foreach (var item in allItems) { item.TotalAmount = 0; }

            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                var committedSales = context.Sales.Where(s => s.Type == "2D").ToList();
                var groupedSales = committedSales.GroupBy(sale => sale.Number)
                    .Select(group => new { Number = group.Key, TotalAmount = group.Sum(item => item.Amount) });

                foreach (var group in groupedSales)
                {
                    var boardItem = allItems.FirstOrDefault(i => i.Number == group.Number);
                    if (boardItem != null)
                    {
                        boardItem.TotalAmount = group.TotalAmount;
                        boardItem.UpdateLimitStatus(LimitAmount, WarningAmount);
                    }
                }
            }
            UpdateGrandTotal();
            UpdateOverLimitList();
        }

        private void GenerateNextVoucherNumber()
        {
            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                var lastVoucher = context.Vouchers.OrderByDescending(v => v.VoucherId).FirstOrDefault();
                int nextNumber = 1;
                if (lastVoucher != null && int.TryParse(lastVoucher.VoucherNumber, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                    if (nextNumber > 999) nextNumber = 1;
                }
                NextVoucherNumber = $"Voucher No: {nextNumber:D3}";
            }
        }

        private void UpdateSessionTitle()
        {
            SessionTitle = $"{_currentSessionMode} Session - {SelectedDate:dd-MMMM-yyyy}";
        }

        private void UpdateGrandTotal()
        {
            var allItems = TotalBoardColumn1.Concat(TotalBoardColumn2).Concat(TotalBoardColumn3).Concat(TotalBoardColumn4);
            GrandTotalAmount = allItems.Sum(item => item.TotalAmount);
        }

        private void UpdateOverLimitList()
        {
            OverLimitItems.Clear();
            var allBoardItems = TotalBoardColumn1.Concat(TotalBoardColumn2).Concat(TotalBoardColumn3).Concat(TotalBoardColumn4);
            var overLimitNumbers = allBoardItems.Where(item => item.IsOverLimit);
            foreach (var item in overLimitNumbers)
            {
                var overAmount = item.TotalAmount - LimitAmount;
                if (overAmount > 0)
                {
                    OverLimitItems.Add(new TotalBoardItem { Number = item.Number, TotalAmount = overAmount });
                }
            }
            TotalOverLimitAmount = OverLimitItems.Sum(item => item.TotalAmount);
        }

        private void RecalculateAllLimitStatus()
        {
            var allItems = TotalBoardColumn1.Concat(TotalBoardColumn2).Concat(TotalBoardColumn3).Concat(TotalBoardColumn4);
            foreach (var item in allItems)
            {
                item.UpdateLimitStatus(LimitAmount, WarningAmount);
            }
            UpdateOverLimitList();
        }

        // --- Command Methods ---
        private void AddEntry(object? parameter)
        {
            if (string.IsNullOrWhiteSpace(NumberInput) || string.IsNullOrWhiteSpace(AmountInput))
            {
                return;
            }
            var results = _parserEngine.Process(NumberInput, AmountInput, _currentSessionMode);
            foreach (var result in results)
            {
                StagedEntries.Add(result);
            }
            NumberInput = string.Empty;
            AmountInput = string.Empty;
        }

        private void RemoveEntry(object? parameter)
        {
            if (parameter is ParseResult entryToRemove)
            {
                StagedEntries.Remove(entryToRemove);
            }
        }

        private void ClearEntries(object? parameter)
        {
            var result = MessageBox.Show("ဤဘောင်ချာရှိ ယာယီစာရင်းများအားလုံးကို အမှန်တကယ် ဖျက်သိမ်းမှာလား?",
                                         "အတည်ပြုပါ", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                StagedEntries.Clear();
            }
        }

        private void SaveVoucher(object? parameter)
        {
            // 1. Validation
            if (SelectedCustomer == null)
            {
                MessageBox.Show("ကျေးဇူးပြု၍ Customer တစ်ယောက်ကို ရွေးချယ်ပါ။", "အမှားအယွင်း", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!StagedEntries.Any())
            {
                MessageBox.Show("သိမ်းဆည်းရန် စာရင်းများ မရှိပါ။", "အသိပေးချက်", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // 2. Data Preparation & Confirmation
            string customerName = SelectedCustomer.Name;
            int totalEntries = StagedEntries.Count;
            decimal totalAmount = StagedEntries.Sum(e => e.Amount);
            string voucherNumberOnly = NextVoucherNumber.Split(':')[1].Trim();

            string message = $"{customerName} အတွက်\n" +
                             $"Voucher No: {voucherNumberOnly}\n" +
                             $"စာရင်းပေါင်း: {totalEntries} ကြောင်း\n" +
                             $"စုစုပေါင်းငွေ: {totalAmount:N0} ကျပ်\n\n" +
                             $"ဤဘောင်ချာကို အမှန်တကယ် သိမ်းဆည်းမှာလား?";

            var result = MessageBox.Show(message, "ဘောင်ချာကို အတည်ပြုပါ", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new LuckyBet.Data.LotteryDbContext())
                {
                    try
                    {
                        var newVoucher = new Voucher
                        {
                            VoucherType = "Customer",
                            ContactId = SelectedCustomer.ContactId,
                            VoucherNumber = voucherNumberOnly,
                            Date = SelectedDate, // Use the selected date
                            TotalAmount = totalAmount,
                            UserId = 1, // TODO: Get current logged-in user ID
                            Session = SessionTitle
                        };

                        foreach (var entry in StagedEntries)
                        {
                            newVoucher.Sales.Add(new Sale
                            {
                                Number = entry.Number,
                                Amount = entry.Amount,
                                Type = _currentSessionMode // "2D" or "3D"
                            });
                        }

                        context.Vouchers.Add(newVoucher);
                        context.SaveChanges();

                        // Update Live Total Board
                        foreach (var savedEntry in StagedEntries)
                        {
                            var allItems = TotalBoardColumn1.Concat(TotalBoardColumn2).Concat(TotalBoardColumn3).Concat(TotalBoardColumn4);
                            var boardItem = allItems.FirstOrDefault(i => i.Number == savedEntry.Number);
                            if (boardItem != null)
                            {
                                boardItem.TotalAmount += savedEntry.Amount;
                                boardItem.UpdateLimitStatus(LimitAmount, WarningAmount);
                            }
                        }

                        UpdateOverLimitList();
                        UpdateGrandTotal();

                        MessageBox.Show("ဘောင်ချာကို အောင်မြင်စွာ သိမ်းဆည်းပြီးပါပြီ။", "ပြီးမြောက်ပါသည်", MessageBoxButton.OK, MessageBoxImage.Information);

                        // Cleanup
                        StagedEntries.Clear();
                        GenerateNextVoucherNumber();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"သိမ်းဆည်းရာတွင် အမှားအယွင်းဖြစ်ပွားပါသည်:\n\n{ex.ToString()}", "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void ForwardToUpline(object? parameter)
        {
            if (!OverLimitItems.Any())
            {
                MessageBox.Show("လွှဲရန် ဘောင်ကျော်စာရင်းများ မရှိပါ။", "အသိပေးချက်", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var dialog = new ForwardingDialog();
            var dialogViewModel = new ForwardingViewModel(OverLimitItems, UplineList);
            dialog.DataContext = dialogViewModel;

            var result = dialog.ShowDialog();

            if (result == true)
            {                
                if (dialogViewModel == null) return;

                var forwardedItems = dialogViewModel.ForwardingItems.Where(i => i.ForwardAmount > 0).ToList();
                if (!forwardedItems.Any()) return;

                // Total Board ကို မနုတ်တော့ဘူး၊ ဒါပေမယ့် ဘောင်ကျော် status ကိုတော့ update လုပ်ရမယ်
                // ဒါပေမယ့် ဘောင်ကျော်စာရင်းက ရှင်းသွားပြီဖြစ်လို့၊ ဘာမှ update လုပ်စရာမလိုတော့ဘူး

                // UI ကို update လုပ်ပါ
                OverLimitItems.Clear();
                TotalOverLimitAmount = 0;
                SelectedUpline = null;

                // IMPORTANT: Recalculate the limit status for the entire board
                // ဒါမှ အနီရောင်တွေ ပျောက်べきပျောက်မှာပါ
                RecalculateAllLimitStatus();

                MessageBox.Show("အထက်ဒိုင်သို့ အောင်မြင်စွာ လွှဲပြီးပါပြီ။", "ပြီးမြောက်ပါသည်", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SetLimit(object? parameter)
        {
            if (decimal.TryParse(LimitAmountInput, out decimal newLimit))
            {
                LimitAmount = newLimit;
                RecalculateAllLimitStatus();
            }
            else
            {
                MessageBox.Show("ကျေးဇူးပြု၍ မှန်ကန်သော ကိန်းဂဏန်းတစ်ခု ထည့်ပါ။", "အမှားအယွင်း", MessageBoxButton.OK, MessageBoxImage.Error);
                LimitAmountInput = LimitAmount.ToString("N0");
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