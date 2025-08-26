using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LuckyBet.UI.Models
{
    public class TotalBoardItem : INotifyPropertyChanged
    {
        public string Number { get; set; } = string.Empty;

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                if (_totalAmount != value)
                {
                    _totalAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        // --- THE FIX IS HERE: Convert to Full Properties ---
        private bool _isOverLimit;
        public bool IsOverLimit
        {
            get => _isOverLimit;
            set
            {
                if (_isOverLimit != value)
                {
                    _isOverLimit = value;
                    OnPropertyChanged(); // Notify the UI when this value changes
                }
            }
        }

        private bool _isNearLimit;
        public bool IsNearLimit
        {
            get => _isNearLimit;
            set
            {
                if (_isNearLimit != value)
                {
                    _isNearLimit = value;
                    OnPropertyChanged(); // Notify the UI when this value changes
                }
            }
        }

        public bool IsNormal => !IsOverLimit && !IsNearLimit;

        public void UpdateLimitStatus(decimal limit, decimal warning)
        {
            // Set the properties directly. The setter of each property will now call OnPropertyChanged.
            IsOverLimit = TotalAmount >= limit && limit > 0;
            IsNearLimit = TotalAmount >= warning && TotalAmount < limit && limit > 0;
            OnPropertyChanged(nameof(IsNormal));
        }

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}