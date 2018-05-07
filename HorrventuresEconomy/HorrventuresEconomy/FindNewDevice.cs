using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Plugin.BLE.Android;
using System.Collections.ObjectModel;

namespace HorrventuresEconomy
{
    [Activity(Label = "Find New Device")]
    public class FindNewDevice : Activity
    {
        private ObservableCollection<MyBleDevice> deviceList;

        private IBluetoothLE ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter ble_adapter;

        private  ArrayAdapter<string> devListAdapter;
        private MyBleDevice selectedBleDevice;
        private ListView devListView;
        private Button startScanButton;
        //TODO разобраться с этим блядским ListView
        private List<string> devListStr;
        protected override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.FindDevicesLayout);


            ble = CrossBluetoothLE.Current;
            ble_adapter = ble.Adapter;

            deviceList = new ObservableCollection<MyBleDevice>();
            devListStr = new List<string>();
            foreach (var dev in ble_adapter.DiscoveredDevices)
            {
                var found_dev = new MyBleDevice((Device)dev);
                deviceList.Add(found_dev);
                devListStr.Add(found_dev.ToString());
            }

            //for teset purpose
            devListStr.Add("test_dev");

            devListView = FindViewById<ListView>(Resource.Id.FindDeviceListView);

            devListAdapter = new ArrayAdapter<string>(this,
                Android.Resource.Layout.SimpleListItem1, devListStr);
        //    RefreshDevList();
            startScanButton = FindViewById<Button>(Resource.Id.ButtonScanNewDevice);
            startScanButton.Click += StartScanButton_Click;
            devListView.Adapter = devListAdapter;


            ble_adapter.ScanTimeoutElapsed += ScanIsDone;

            ble_adapter.DeviceDiscovered += AddViewDevice;
            devListView.ItemClick += AddNewDevice;

            ((ArrayAdapter<string>)devListAdapter).SetNotifyOnChange(true);

        }

        private void RefreshDevList()
        {
            devListStr.Clear();
           foreach (var dev in ble_adapter.DiscoveredDevices)
            {
                
                var found_dev = new MyBleDevice((Device)dev);
                devListAdapter.Add(found_dev.ToString());
                deviceList.Add(found_dev);
                devListStr.Add(found_dev.ToString());
            }
            devListAdapter.Clear();
            devListAdapter.AddAll(devListStr);
            ((BaseAdapter)devListAdapter).NotifyDataSetChanged();
        }

        private async void StartScanButton_Click(object sender, EventArgs e)
        {
            startScanButton.Clickable = false;

            await PerformScan();
            RefreshDevList();
            RefreshListView();
            startScanButton.Clickable = true;

        }

        private async System.Threading.Tasks.Task PerformScan()
        {
            if (!ble.Adapter.IsScanning)
            {
                await ble_adapter.StartScanningForDevicesAsync();
            }
        }

        private void RefreshListView()
        {
            ((BaseAdapter)devListAdapter).NotifyDataSetChanged();
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (requestCode == 300 && resultCode == Result.Ok)
            {
                BeaconDB.RegisterNewBeacon(selectedBleDevice.Device.Id.ToString(),
                    data.GetDoubleExtra("mult", 1.0),
                    data.GetDoubleExtra("income", 0),
                    (Beacon.BeaconType)data.GetIntExtra("type", 0));
            }
        }

        private void AddNewDevice(object sender, AdapterView.ItemClickEventArgs e)
        {
            selectedBleDevice = deviceList[e.Position];
            Intent intent = new Intent(this, typeof(AddNewDeviceActivity));
            
            intent.PutExtra("MAC", selectedBleDevice.Device.Id.ToString());
            intent.PutExtra("Mode", "ADD");
            //TODO: переписать requestCode в общиую талицу
            StartActivityForResult(intent, 300);
        }

        private void AddViewDevice(object sender, DeviceEventArgs e)
        {
            var found_dev = new MyBleDevice((Device)e.Device);
            Console.WriteLine("Finder found!");
            //TODO: Удалить
            Toast.MakeText(this, "новое устройство" + e.Device.Id.ToString(), ToastLength.Long).Show();
            deviceList.Add(found_dev);
            devListAdapter.Add(found_dev.ToString());
            
            RefreshDevList();
            RefreshListView();

        }

        private void ScanIsDone(object sender, EventArgs e)
        {
            RefreshDevList();
            RefreshListView();

        }



        // Create your application here
    }

}