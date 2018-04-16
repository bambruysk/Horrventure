using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HorrventuresEconomy
{
    [Activity(Label = "GetMoney")]
    public class GetMoneyActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.GetMoney);

            Button submit = FindViewById<Button>(Resource.Id.submit);
            Button cancel = FindViewById<Button>(Resource.Id.cancel);

            cancel.Click += OnCancelClick;
            submit.Click += OnSubmitClick;
        }

        private void OnSubmitClick(object sender, EventArgs e)
        {
            if (int.TryParse(FindViewById<EditText>(Resource.Id.moneyInput).Text, out int amount))
            {
                var intent = new Intent();
                intent.PutExtra("Amount", amount);
                SetResult(Result.Ok, intent);
                Finish();
            }
            else
            {
                Alert();
            }
        }

        private void OnCancelClick(object sender, EventArgs e)
        {
            Finish();
        }

        public void Alert()
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetTitle("Некорректные данные");
            builder.SetMessage("Вы ввели не число.");
            builder.SetCancelable(false);
            builder.SetNegativeButton("Cancel", (senderAlert, args) => {
                Toast.MakeText(this, "Cancelled!", ToastLength.Short).Show();
            });

            Dialog dialog = builder.Create();
            dialog.Show();
        }
    }
}