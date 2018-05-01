using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using System.Timers;
using System;
using Android.Graphics;
using Android.Views;
using static Android.Widget.ActionMenuView;

namespace HorrventuresEconomy
{
    [Activity(Label = "HorrventuresEconomy", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {
        Timer timer;
        public TextView currency;
        public TextView cityState;
        LinearLayout currencyLayout;
        EconomyLogic logic;

        public ImageView alchemyImage;
        public ImageView jewelryImage;
        public FrameLayout smithImage;
        public FrameLayout palaceImage;
        public ImageView libraryImage;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            currency = FindViewById<TextView>(Resource.Id.currency);
            cityState = FindViewById<TextView>(Resource.Id.cityState);

            var alchemy = FindViewById<TextView>(Resource.Id.alchemy);
            var alchemyLevel = FindViewById<TextView>(Resource.Id.alchemyLevel);
            var jewelry = FindViewById<TextView>(Resource.Id.jewelry);
            var jewelryLevel = FindViewById<TextView>(Resource.Id.jewelryLevel);
            var smith = FindViewById<TextView>(Resource.Id.smith);
            var smithLevel = FindViewById<TextView>(Resource.Id.smithLevel);
            var palace = FindViewById<TextView>(Resource.Id.palace);
            var palaceLevel = FindViewById<TextView>(Resource.Id.palaceLevel);
            var library = FindViewById<TextView>(Resource.Id.library);
            var libraryLevel = FindViewById<TextView>(Resource.Id.libraryLevel);

            alchemyImage = FindViewById<ImageView>(Resource.Id.alchemyImage);
            jewelryImage = FindViewById<ImageView>(Resource.Id.jewelryImage);
            smithImage = FindViewById<FrameLayout>(Resource.Id.smithImage);
            palaceImage = FindViewById<FrameLayout>(Resource.Id.palaceImage);
            libraryImage = FindViewById<ImageView>(Resource.Id.libraryImage);

            SetImages();

            currency.TextSize = 30f;
            cityState.TextSize = 30f;
            alchemy.TextSize = 18f;
            alchemyLevel.TextSize = 18f;
            jewelry.TextSize = 18f;
            jewelryLevel.TextSize = 18f;
            smith.TextSize = 18f;
            smithLevel.TextSize = 18f;
            palace.TextSize = 18f;
            palaceLevel.TextSize = 18f;
            library.TextSize = 18f;
            libraryLevel.TextSize = 18f;

            currencyLayout = FindViewById<LinearLayout>(Resource.Id.currencyLayout);
            Button getMoneyButton = FindViewById<Button>(Resource.Id.getMoney);
            Button refreshMoney = FindViewById<Button>(Resource.Id.refreshMoney);
            var cityLayout = FindViewById<LinearLayout>(Resource.Id.cityLayout);


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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.ActionBarMenu, menu);

            var shareMenuItem = menu.FindItem(Resource.Id.shareMenuItem);

            //var shareActionProvider =
            //   (ShareActionProvider)shareMenuItem.ActionProvider;
            //shareActionProvider.SetShareIntent(CreateIntentMenuIntent());
            //Intent intent = new Intent(this, typeof(AdminPanelActivity));
            //hareMenuItem.SetIntent(intent);

            Button itemButton = (Button)shareMenuItem.ActionView;
            itemButton.Click += StartAdminPanel;

            return base.OnCreateOptionsMenu(menu);
        }

        private void StartAdminPanel(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AdminPanelActivity));
            StartActivity(intent);
        }

        public void SetImages()
        {
            alchemyImage.SetBackgroundColor(Color.White);
            alchemyImage.SetImageResource(Resource.Drawable.alchemy);
            jewelryImage.SetBackgroundColor(Color.White);
            jewelryImage.SetImageResource(Resource.Drawable.jewelry);
            smithImage.SetBackgroundResource(Resource.Drawable.smith);
            palaceImage.SetBackgroundResource(Resource.Drawable.palace);
            libraryImage.SetBackgroundColor(Color.White);
            libraryImage.SetImageResource(Resource.Drawable.library);
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
            RequestPassword request = new RequestPassword(this,
                new RequestPassword.RequestAction(GetMoneyActivityStart));
            request.GetPassword(RequestPassword.SecurityLevel.REG);
        }

        private void GetMoneyActivityStart(Context context)
        {
            var intent = new Intent(context, typeof(GetMoneyActivity));
            StartActivityForResult(intent, 100);
        }

        private void OnRefreshMoney(object sender, EventArgs e)
        {
            RequestPassword request = new RequestPassword(this,
                new RequestPassword.RequestAction(RefreshMoney));
            request.GetPassword(RequestPassword.SecurityLevel.REG);

        }

        private void RefreshMoney(Context context)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(context);
            alert.SetTitle("Подтвердите обнуление");
            alert.SetMessage("Вы действительно хотите обнулить казну?");
            alert.SetPositiveButton("Да", (senderAlert, args) =>
            {
                logic.RefreshCurrency();
                Toast.MakeText(context, "Начинаем с нуля!", ToastLength.Short).Show();
            });
            alert.SetNegativeButton("Отмена", (senderAlert, args) =>
            {
                Toast.MakeText(context, "Фуф!", ToastLength.Short).Show();
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

