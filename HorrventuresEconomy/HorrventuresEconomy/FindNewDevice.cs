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

namespace HorrventuresEconomy
{
    [Activity(Label = "Find New Device")]
    public class FindNewDevice : Activity



    {
         private List <MyBleDevice> deviceList;

        private BeaconDB beaconDB;

        private IBluetoothLE ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter;
        private List<TableRow> tableRows;
        private ArrayAdapter<MyBleDevice> arrayAdapter;

        private ListView listViewDevices;

        private MyBleDevice selectedBleDevice;
        protected override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);
            
            SetContentView(Resource.Layout.FindDevicesLayout);


            beaconDB = new BeaconDB();

            ble = CrossBluetoothLE.Current;
            adapter = ble.Adapter;

            
            listViewDevices = FindViewById<ListView>(Resource.Id.FindDeviceListView);
            arrayAdapter = new ArrayAdapter<MyBleDevice>(this, Resource.Id.FindDeviceListView, deviceList);

            Button startScanButton = FindViewById<Button>(Resource.Id.ButtonScanNewDevice);

            listViewDevices.AddFooterView(startScanButton);

            startScanButton.Click += StartScan;

            adapter.ScanTimeoutElapsed += ScanIsDone;

            adapter.DeviceDiscovered += AddViewDevice;
            listViewDevices.ItemClick += AddNewDevice;

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            beaconDB.RegisterNewBeacon(selectedBleDevice.Device.Id.ToString(), 
                data.GetDoubleExtra("mult", 1.0),
                data.GetDoubleExtra("income", 0), 
                (Beacon.BeaconType)data.GetIntExtra("type", 0));
        }

        private void AddNewDevice(object sender, AdapterView.ItemClickEventArgs e)
        {
            selectedBleDevice=  deviceList[e.Position];
            Intent intent = new Intent(this, typeof( AddNewDeviceActivity));
            intent.PutExtra("MAC", selectedBleDevice.Device.Id.ToString());
            StartActivityForResult(intent, 0);
        }

        private void AddViewDevice(object sender, DeviceEventArgs e)
        {
            deviceList.Add(new MyBleDevice((Device)e.Device));
        }

        private void ScanIsDone(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void StartScan(object sender, EventArgs e)
        {
            deviceList.Clear();
            adapter.StartScanningForDevicesAsync();
        }

        private void UpdateView ()
        {

            foreach (var dev in adapter.DiscoveredDevices)
            {
                deviceList.Add(new MyBleDevice((Device)dev));
            }

        }



        // Create your application here
    }
    
}