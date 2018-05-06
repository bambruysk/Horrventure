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
   //     private Beacon Beacon;

        public string Id;
        public double Mulltiplier;
        public double IncomePerMinute;

        private int positionFromParent;

        public Beacon.BeaconType beaconType;
        private ListView ListBeaconTypes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.AddDevice);
            Button button_save = FindViewById<Button>(Resource.Id.addDeviceButton);

            TextView name = FindViewById<TextView>(Resource.Id.addDeviceLabeText);
            name.Text = Intent.GetStringExtra("MAC");

            List<string> beaconTypes = new List<string>();
            foreach (Beacon.BeaconType b in Enum.GetValues(typeof(Beacon.BeaconType)))
            {
                Console.WriteLine("Test type " + b.ToString());
                beaconTypes.Add(b.ToString());
            }
            ArrayAdapter adapter = new ArrayAdapter<string>(this,
                 Android.Resource.Layout.SimpleListItemSingleChoice, beaconTypes);
            ListBeaconTypes = FindViewById<ListView>(Resource.Id.listBeaconTypes);
            ListBeaconTypes.Adapter = adapter;

            EditText editTextMult =
                FindViewById<EditText>(Resource.Id.addDeviceMultEditText);
            EditText editTextIncome =
                FindViewById<EditText>(Resource.Id.addDeviceIncomeEditText);
           
            if (Intent.GetStringExtra("Mode")== "EDIT")
            {
                
                SetTitle(Resource.String.Beacon_edit_mode);
                editTextMult.Text =
                    Intent.GetDoubleExtra("mult",1.0f).ToString();
                   
                editTextIncome.Text =
                    Intent.GetDoubleExtra("income", 0.0f).ToString();
                ListBeaconTypes.SetSelection(
                    Intent.GetIntExtra("type", 0)
                    );
                positionFromParent = Intent.GetIntExtra("Position", 0);
            }
            else if (Intent.GetStringExtra("Mode") == "ADD"){
                SetTitle(Resource.String.Beacon_edit_mode);
                editTextMult.Text = (1.0f).ToString();
                editTextIncome.Text = (0.0f).ToString();
            };

            button_save.Click += delegate
            {
                Intent myIntent = new Intent(this, typeof(FindNewDevice));
                myIntent.PutExtra("type", ListBeaconTypes.CheckedItemPosition);

                Double mult = 0;
                if (!Double.TryParse(editTextMult.Text,out  mult))
                {

                    //Toast.MakeText(this, "Multiplier format error", ToastLength.Long);
                    return;
                }
                myIntent.PutExtra("mult", mult);
                Double income = 0;
                if (!Double.TryParse(editTextIncome.Text, out income))
                {
                    //Toast.MakeText(this, "Income format error", ToastLength.Long);
                    return;
                }
                myIntent.PutExtra("mult", mult);
                myIntent.PutExtra("income", income);
                myIntent.PutExtra("Position", positionFromParent);

                SetResult(Result.Ok, myIntent);
                Finish();
            };
        }

    }
}