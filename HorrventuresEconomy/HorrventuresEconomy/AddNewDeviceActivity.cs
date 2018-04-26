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
    [Activity(Label = "AddNewDeviceActivity")]
    public class AddNewDeviceActivity : Activity
    {
        private Beacon Beacon;

        public string Id;
        public double Mulltiplier;
        public double IncomePerMinute;

        public Beacon.BeaconType beaconType;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddDevice);
            Button button_save = FindViewById<Button>(Resource.Id.addDeviceButton);

            TextView name = FindViewById<TextView>(Resource.Id.addDeviceLabeText);
            name.Text = ParentActivityIntent.GetStringExtra("MAC");

            ListView ListBeaconTypes = FindViewById<ListView>(Resource.Id.listBeaconTypes);
            List<string> beaconTypes = new List<string>();
            foreach (Beacon.BeaconType b in Enum.GetValues(typeof(Beacon.BeaconType)))
            {
                beaconTypes.Add(b.ToString());
            }
            ArrayAdapter adapter = new ArrayAdapter<string>(this,
                Resource.Id.listBeaconTypes, beaconTypes);

            EditText editTextMult =
                FindViewById<EditText>(Resource.Id.addDeviceMultEditText);
            EditText editTextIncome =
                FindViewById<EditText>(Resource.Id.addDeviceIncomeEditText);

            button_save.Click += delegate
            {
                Intent myIntent = new Intent(this, typeof(FindNewDevice));
                myIntent.PutExtra("type", ListBeaconTypes.SelectedItemPosition);
                myIntent.PutExtra("mult", Double.Parse(editTextMult.Text));
                myIntent.PutExtra("income", Double.Parse(editTextIncome.Text));
                SetResult(Result.Ok, myIntent);
                Finish();
            };



            // Create your application here
        }

    }
}