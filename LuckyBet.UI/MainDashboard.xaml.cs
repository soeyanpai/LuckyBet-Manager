using LuckyBet.UI.ViewModels; // ViewModel ရဲ့ namespace ကို ချိတ်ဆက်ပါ
using System.Windows;

namespace LuckyBet.UI
{
    public partial class MainDashboard : Window
    {
        public MainDashboard(string session) // session parameter ကို လက်ခံရန် ပြင်ဆင်
        {
            InitializeComponent();

            // ViewModel ကိုတည်ဆောက်တဲ့အခါ session ကိုပါ လက်ဆင့်ကမ်းထည့်ပေးလိုက်ပါ
            this.DataContext = new MainDashboardViewModel(session);
        }

        private void NumberInputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // နှိပ်လိုက်တဲ့ ခလုတ်က 'Enter' ခလုတ် ဟုတ်မဟုတ် စစ်ဆေးပါ
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // ဟုတ်တယ်ဆိုရင်၊ Focus (Cursor) ကို AmountInputTextBox ဆီကို ရွှေ့ပေးပါ
                AmountInputTextBox.Focus();

                // Enter ခေါက်ရင် "ติ๊ง" ဆိုတဲ့ အသံမမြည်အောင် event ကို handled လုပ်လိုက်ပါ
                e.Handled = true;
            }
        }

        private void AmountInputTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // နှိပ်လိုက်တဲ့ ခလုတ်က 'Enter' ခလုတ် ဟုတ်မဟုတ် စစ်ဆေးပါ
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // ViewModel ကို အရင်ဆုံး ရယူပါ
                var viewModel = this.DataContext as ViewModels.MainDashboardViewModel;
                if (viewModel == null) return;

                // ViewModel ထဲက Command က အလုပ်လုပ်လို့ရတဲ့ အခြေအနေ ဟုတ်မဟုတ် စစ်ဆေးပါ
                if (viewModel.AddEntryCommand.CanExecute(null))
                {
                    // Command ကို Code ကနေ တိုက်ရိုက် အလုပ်လုပ်ခိုင်းလိုက်ပါ
                    viewModel.AddEntryCommand.Execute(null);
                }

                // Command အလုပ်လုပ်ပြီးသွားပြီဆိုတော့ Focus ကို Number Input ဆီပြန်ရွှေ့ပါ
                NumberInputTextBox.Focus();
                NumberInputTextBox.SelectAll(); // Textbox ထဲက စာတွေကို Select လုပ်ထားပေးပါ

                // Enter ခေါက်ရင် "ติ๊ง" ဆိုတဲ့ အသံမမြည်အောင် event ကို handled လုပ်လိုက်ပါ
                e.Handled = true;
            }
        }

    }
}