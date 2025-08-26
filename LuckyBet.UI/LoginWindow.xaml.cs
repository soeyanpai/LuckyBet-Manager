using LuckyBet.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LuckyBet.UI
{
    public partial class LoginWindow : Window
    {
        // ViewModel အတွက် private field တစ်ခု ကြေညာပါတယ်
        private readonly LoginViewModel _viewModel;
        public LoginWindow()
        {
            InitializeComponent();

            _viewModel = new LoginViewModel();

            // --- ခေါင်းလောင်းသံကို နားထောင်ရန် စာရင်းပေးသွင်းခြင်း ---
            _viewModel.LoginSuccess += OnLoginSuccess; // OnLoginSuccess ဆိုတဲ့ method ကို ခေါ်ပေးပါလို့ ပြောတာ

            this.DataContext = _viewModel;
        }

        // --- ခေါင်းလောင်းသံကြားရင် ဒီ Method ကို အလိုအလျောက် ခေါ်ပါလိမ့်မယ် ---
        private void OnLoginSuccess(object? sender, string session)
        {
            // MainDashboard ကိုဖွင့်တဲ့အခါ session ကိုပါ လက်ဆင့်ကမ်းထည့်ပေးလိုက်ပါ
            var dashboard = new MainDashboard(session);
            dashboard.Show();

            this.Close();
        }

        // Login Button ကိုနှိပ်ရင် ViewModel ထဲက Login method ကို ခေါ်သုံးပါမယ်
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // ဘယ် RadioButton ကို ရွေးထားလဲ စစ်ဆေးပြီး ViewModel ကို တန်ဖိုးပေးပါ
            if (TwoDRadioButton.IsChecked == true)
            {
                _viewModel.SelectedSession = "2D";
            }
            else
            {
                _viewModel.SelectedSession = "3D";
            }

            _viewModel.Login(PasswordBox.Password);
        }

        // Window ကို Drag လုပ်လို့ရအောင်လုပ်တဲ့ Method
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        // Minimize Button အတွက် Method
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        // Close Button အတွက် Method
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}