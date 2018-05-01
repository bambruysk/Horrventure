using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    [Activity(Label = "DeviceDbViewActivity")]
    public class DeviceDbViewActivity : ListActivity
    {
        private BeaconDB beaconDB;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);



            beaconDB = new BeaconDB();
            ObservableCollection<Beacon> deviceDB = new ObservableCollection<Beacon>();
            foreach (var beac in beaconDB.Beacons_db)
            {
                deviceDB.Add(beac.Value);
            }

            ListAdapter = new ArrayAdapter<Beacon>(this, Resource.Layout.DeviceDBViewLayout, Resource.Id.DevDBTextView, deviceDB);
            //Button findDeicebutton = FindViewById<Button>(Resource.Id.DevDBFindNewDevice);

            //ListView.AddFooterView(findDeicebutton);
           // findDeicebutton.Click += StartFindNewDevce;
          
        }

        private void StartFindNewDevce(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(FindNewDevice));
            StartActivity(intent);
        }

        
    }
}