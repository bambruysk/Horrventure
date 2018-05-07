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

namespace HorrventuresEconomy
{
    // TODO change to static or singletone
    public class BluetoothDataLayer
    {
        private Timer timer;
        private IBluetoothLE ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter;
        private List<MyDeviceView> deviceList;

        public int MinRssi;


        public BluetoothDataLayer()
        {

            timer = new Timer
            {
                Interval = 15000
            };
            timer.Elapsed += OnTimerTick;
            timer.Start();

            ble = CrossBluetoothLE.Current;
            adapter = ble.Adapter;
            deviceList = new List<MyDeviceView>();

            MinRssi = -100;

            adapter.DeviceDiscovered += (s, a) =>
            {
                Console.WriteLine("DeviceFound");
                if (a.Device.Rssi >= MinRssi)
                {
                    if (deviceList.Exists(d => d.id == a.Device.Id))
                    {

                        var dev = deviceList.Find(d => d.id == a.Device.Id);
                        dev.UpdateCountdown();
                        dev.RSSI = a.Device.Rssi;

                    }
                    else
                    {
                        deviceList.Add(new MyDeviceView(a.Device));
                    }
                }
            };
        }
        public void StopScan()
        {
            timer.Stop();
        }
        private async void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            if (!ble.Adapter.IsScanning)
            {
                await adapter.StartScanningForDevicesAsync();
            }
            ProcessDevices();
        }

        


        public List<MyDeviceView> GetDeviceList()
        {
            return deviceList;
        }

        private void ProcessDevices()
        {
            foreach (var device in deviceList)
            {
                device.Countdown--;
            }
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (deviceList[i].Countdown == 0)
                {
                    deviceList.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}