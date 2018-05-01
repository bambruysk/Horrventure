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
    public class FindNewDevice : ListActivity



    {
         private ObservableCollection <MyBleDevice> deviceList;

        private BeaconDB beaconDB;

        private IBluetoothLE ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter;




    private MyBleDevice selectedBleDevice;
        protected override void OnCreate(Bundle savedInstanceState)

        {
            base.OnCreate(savedInstanceState);
            
            //SetContentView(Resource.Layout.FindDevicesLayout);

           deviceList = new ObservableCollection <MyBleDevice>();

            //for teset purpose
            
            beaconDB = new BeaconDB();

            ble = CrossBluetoothLE.Current;
            adapter = ble.Adapter;

            foreach (var dev in adapter.DiscoveredDevices)
            {
                deviceList.Add(new MyBleDevice((Device) dev));
            }
          


            adapter.StartScanningForDevicesAsync();
            
            
            //ListView = FindViewById<ListView>(Resource.Id.FindDeviceListView);
            //ListAdapter = new ScannedDeviceAdapter(this ,deviceList);
            ListAdapter = new ArrayAdapter<MyBleDevice>(this,
                Resource.Layout.FindDevicesLayout,Resource.Id.foundDeviceView, deviceList);
            
            ///Button startScanButton = FindViewById<Button>(Resource.Id.ButtonScanNewDevice);
            
            //ListView.AddFooterView(startScanButton);
           // ListView.LongClick += StartScan;

            ///startScanButton.Click 

            adapter.ScanTimeoutElapsed += ScanIsDone;

            adapter.DeviceDiscovered += AddViewDevice;
            ListView.ItemClick += AddNewDevice;
            
            ((ArrayAdapter<MyBleDevice>)ListAdapter).SetNotifyOnChange(true);


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
            Toast.MakeText(this, 
                ("Найдено устройство" + (new MyBleDevice((Device)e.Device).ToString())),
               ToastLength.Short).Show();

            ((ArrayAdapter<MyBleDevice>)ListAdapter).NotifyDataSetChanged();
            

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
            Toast.MakeText(this, "Сканирование завершено", ToastLength.Short).Show();
            // PIZDEZ
            //  ListAdapter = new ArrayAdapter<MyBleDevice>(this,Resource.Layout.FindDevicesLayout, Resource.Id.foundDeviceView, deviceList);

        }



        // Create your application here
    }
    
}