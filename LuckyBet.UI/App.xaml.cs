using System.Configuration;
using System.Data;
using System.Windows;

namespace LuckyBet.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // ဒီ function တစ်ခုလုံးက class App ရဲ့ { ... } ကြားထဲမှာ ရှိနေရပါမယ်။
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // --- User အသစ်ထည့်သွင်းရန် Code အသစ် ---
            // using statement က DbContext ကို အလုပ်ပြီးတာနဲ့ အလိုအလျောက်ပိတ်ပေးပါတယ်။
            using (var context = new LuckyBet.Data.LotteryDbContext())
            {
                // Database ကို တည်ဆောက်ပြီးကြောင်း သေချာစေပါတယ်
                context.Database.EnsureCreated();

                // Database ထဲမှာ User တစ်ယောက်မှမရှိဘူးဆိုရင်...
                if (!context.Users.Any())
                {
                    // 'admin' ဆိုတဲ့ User အသစ်တစ်ယောက်ကို ဖန်တီးပါ
                    var adminUser = new LuckyBet.Core.Models.User
                    {
                        Username = "admin",
                        // '123' ဆိုတဲ့ password ကို လုံခြုံတဲ့ Hash အဖြစ်ပြောင်းလဲလိုက်ပါတယ်
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("123"),
                        Role = "Admin"
                    };
                    // ဖန်တီးထားတဲ့ User ကို Database ထဲထည့်ဖို့ မှတ်ထားလိုက်ပါတယ်
                    context.Users.Add(adminUser);
                    // အပြောင်းအလဲအားလုံးကို Database ထဲမှာ တကယ်သိမ်းဆည်းလိုက်ပါတယ်
                    context.SaveChanges();
                }

                // Database ထဲမှာ Customer တစ်ယောက်မှမရှိဘူးဆိုရင်...
                if (!context.Contacts.Any(c => c.ContactType == "Customer"))
                {
                    // စမ်းသပ်ဖို့ Customer (၃) ယောက်ကို ထည့်ပါ
                    context.Contacts.Add(new LuckyBet.Core.Models.Contact { Name = "ကိုအောင်", ContactType = "Customer", CommissionRate = 0.15m, PayoutRate2D = 85, PayoutRate3D = 700 });
                    context.Contacts.Add(new LuckyBet.Core.Models.Contact { Name = "မအေး", ContactType = "Customer", CommissionRate = 0.15m, PayoutRate2D = 85, PayoutRate3D = 700 });
                    context.Contacts.Add(new LuckyBet.Core.Models.Contact { Name = "Upline-1", ContactType = "Upline", CommissionRate = 0, PayoutRate2D = 90, PayoutRate3D = 750 });

                    // အပြောင်းအလဲအားလုံးကို Database ထဲမှာ တကယ်သိမ်းဆည်းလိုက်ပါတယ်
                    context.SaveChanges();
                }

            }
            // --- User အသစ်ထည့်သွင်းရန် Code အဆုံး ---

            // LoginWindow ကို အရင်ဆုံးဖွင့်ပါ (မူလအတိုင်း)
            var loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }
    }
}