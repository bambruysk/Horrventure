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
using Plugin.BLE;

namespace HorrventuresEconomy
{
    [Activity(Label = "DeviceDbViewActivity")]
    public class DeviceDbViewActivity : Activity
    {


        private int currentPosition;
        private List<Beacon> deviceDB;
        private ListView listView;
        private ArrayAdapter arrayAdapter;
        //private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.DeviceDBViewLayout);

            
            BeaconDB.Initialize();
            deviceDB = new List<Beacon>();
            ///TODO увесть в метод BeaconDB
            foreach (var beac in BeaconDB.Beacons_db)
            {
                deviceDB.Add(beac.Value);
            }

            listView = FindViewById<ListView>(Resource.Id.DevDBViewlistView);
            arrayAdapter = new ArrayAdapter (this,Android.Resource.Layout.SimpleListItem1, deviceDB);
            listView.ItemClick += EditDevice;
            //Button findDeicebutton = FindViewById<Button>(Resource.Id.DevDBFindNewDevice);
            listView.Adapter = arrayAdapter;
            Button saveButton = FindViewById<Button>(Resource.Id.DevDVSaveButton);
            Button addButton = FindViewById<Button>(Resource.Id.DevDBAddButton);

            //ListView.AddFooterView(findDeicebutton);
            // findDeicebutton.Click += StartFindNewDevce;
            
            saveButton.Click += SaveButton_Click;
            addButton.Click += AddButton_Click;
            ((ArrayAdapter)arrayAdapter).SetNotifyOnChange(true);
        }

        protected override void OnResume()
        {
            base.OnResume();
            BeaconDB.Initialize();
            BeaconDB.Upload();
            Console.WriteLine("Resumeactivitie!!" + this.ToString());
            deviceDB.Clear();
            foreach (var beac in BeaconDB.Beacons_db)
            {
                Console.WriteLine("Device Added " + beac.ToString());
                deviceDB.Add(beac.Value);
            }
            arrayAdapter.Clear();
            arrayAdapter.AddAll(deviceDB);
            ((BaseAdapter)arrayAdapter).NotifyDataSetChanged();
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            var ble = CrossBluetoothLE.Current;
            var ble_adapter = ble.Adapter;

            Intent intent = new Intent(this, typeof(FindNewDevice));
            StartActivity(intent);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            BeaconDB.SaveDB(deviceDB);
            //Toast.MakeText(this, "База данных маячков сохранена", ToastLength.Long); 

        }

        private void EditDevice(object sender, AdapterView.ItemClickEventArgs e)
        {
            Beacon beacon = deviceDB[e.Position];
            var alert = new AlertDialog.Builder(this);
            alert.SetTitle("Что сделать с устройством?")
            .SetCancelable(true)
            .SetPositiveButton("Редактировать", (senderAlert, args) =>
               {
                   StartEditDevice(e, beacon);
               })
               .SetNeutralButton("Удалить", (senderAlert, args) =>
               {
                   deviceDB.RemoveAt(e.Position);
                   BeaconDB.SaveDB(deviceDB);
                   ((BaseAdapter)arrayAdapter).NotifyDataSetChanged();
                   arrayAdapter.Clear();
                   arrayAdapter.AddAll(deviceDB);

               })
               .SetNegativeButton("Отмена", (senderAlert, args) =>
              {
    
              });
            Dialog dialog = alert.Create();
            dialog.Show();

        }

        private void StartEditDevice(AdapterView.ItemClickEventArgs e, Beacon beacon)
        {
            Intent intent = new Intent(this, typeof(AddNewDeviceActivity));
            intent.PutExtra("type", (int)beacon.beaconType);
            intent.PutExtra("mult", (Double)beacon.Mulltiplier);
            intent.PutExtra("income", (Double)beacon.IncomePerHour);
            intent.PutExtra("MAC", beacon.Id);
            intent.PutExtra("Position", e.Position);
            intent.PutExtra("Mode", "EDIT");
            currentPosition = e.Position;
            StartActivityForResult(intent, 200);
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == 200 && resultCode == Result.Ok)
            {
                Console.WriteLine("\nDEBOOUUU" + data.GetIntExtra("type", 2));
                deviceDB[currentPosition].beaconType =  
                    (Beacon.BeaconType) Enum.ToObject(typeof(Beacon.BeaconType), data.GetIntExtra("type", 2));
                deviceDB[currentPosition].IncomePerHour = (Double)data.GetDoubleExtra("income", 0);
                deviceDB[currentPosition].Mulltiplier = (Double)data.GetDoubleExtra("mult", 0);
                ((BaseAdapter)arrayAdapter).NotifyDataSetChanged();
            }
        }
    }
}