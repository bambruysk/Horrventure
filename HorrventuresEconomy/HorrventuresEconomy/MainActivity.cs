using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Timers;
using System;

namespace HorrventuresEconomy
{
    [Activity(Label = "HorrventuresEconomy", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Timer timer;
        public TextView currency;
        LinearLayout currencyLayout;
        EconomyLogic logic;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            currency = FindViewById<TextView>(Resource.Id.currency);
            currencyLayout = FindViewById<LinearLayout>(Resource.Id.currencyLayout);
            Button getMoneyButton = FindViewById<Button>(Resource.Id.getMoney);
            Button refreshMoney = FindViewById<Button>(Resource.Id.refreshMoney);
            var incomeLayout = FindViewById<LinearLayout>(Resource.Id.incomeLayout);

            getMoneyButton.Click += OnGetMoney;
            refreshMoney.Click += OnRefreshMoney;

            currency.Text = "Монет в казне: 0";
            timer = new Timer
            {
                Interval = 1000
            };
            timer.Elapsed += OnTimerTick;
            timer.Start();

            logic = LastNonConfigurationInstance as EconomyLogic;

            if (logic == null)
            {
                logic = new EconomyLogic();
            }
        }

        public override Java.Lang.Object OnRetainNonConfigurationInstance()
        {
            base.OnRetainNonConfigurationInstance();
            return logic;
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            RunOnUiThread(() => currency.Text = "Монет в казне: " + logic.GetCurrency());
        }

        private void OnGetMoney(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(GetMoneyActivity));

            StartActivityForResult(intent, 100);
        }

        private void OnRefreshMoney(object sender, EventArgs e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Подтвердите обнуление");
            alert.SetMessage("Вы действительно хотите обнулить казну?");
            alert.SetPositiveButton("Да", (senderAlert, args) =>
            {
                logic.RefreshCurrency();
                Toast.MakeText(this, "Начинаем с нуля!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Отмена", (senderAlert, args) =>
            {
                Toast.MakeText(this, "Фуф!", ToastLength.Short).Show();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 100 && resultCode == Result.Ok)
            {
                int amount = data.GetIntExtra("Amount", 0);
                RetrieveMoney(amount);
            }
        }

        private void RetrieveMoney(int amount)
        {
            if (logic.RetrieveMoney(amount))
            {
                Сongratulations(amount);
            }
            else
            {
                Alert();
            }
        }

        public void Alert()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Слишком много");
            builder.SetMessage("У вас нет столько денег!!!");
            builder.SetCancelable(false);
            builder.SetNegativeButton("Грусть", (senderAlert, args) =>
            {
                Toast.MakeText(this, "Это печально", ToastLength.Short).Show();
            });

            Dialog dialog = builder.Create();
            dialog.Show();
        }

        public void Сongratulations(int amount)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Поздравляем!!!");
            builder.SetMessage("Вы сняли " + amount + " монет");
            builder.SetCancelable(false);
            builder.SetNegativeButton("Ура!!!", (senderAlert, args) =>
            {
                Toast.MakeText(this, "Ура!!!", ToastLength.Short).Show();
            });

            Dialog dialog = builder.Create();
            dialog.Show();
        }
    }
}

