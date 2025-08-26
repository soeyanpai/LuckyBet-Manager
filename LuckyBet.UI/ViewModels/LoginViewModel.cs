using LuckyBet.UI.Commands;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace LuckyBet.UI.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        // --- Properties ---
        public string Username { get; set; } = string.Empty;
        public string SelectedSession { get; set; } = "2D";

        private string _statusText = string.Empty;
        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        // --- Events ---
        public event EventHandler<string>? LoginSuccess;

        // --- Commands ---
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        public void Login(object? parameter)
        {
            // Note: Password is now passed as a parameter
            string? password = parameter as string;

            if (string.IsNullOrWhiteSpace(Username))
            {
                StatusText = "ကျေးဇူးပြု၍ Username ထည့်ပါ။";
                return;
            }
            if (string.IsNullOrEmpty(password))
            {
                StatusText = "ကျေးဇူးပြု၍ Password ထည့်ပါ။";
                return;
            }

            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                var userFromDb = context.Users.FirstOrDefault(u => u.Username == this.Username);
                if (userFromDb == null)
                {
                    StatusText = "Username သို့မဟုတ် Password မှားယွင်းနေပါသည်။";
                }
                else
                {
                    bool isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password, userFromDb.PasswordHash);
                    if (isPasswordCorrect)
                    {
                        StatusText = "Login အောင်မြင်ပါသည်!";
                        LoginSuccess?.Invoke(this, SelectedSession);
                    }
                    else
                    {
                        StatusText = "Username သို့မဟုတ် Password မှားယွင်းနေပါသည်။";
                    }
                }
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