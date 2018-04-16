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
    public class BluetoothDataLayer
    {
        private Timer timer;
        private IBluetoothLE ble;
        private Plugin.BLE.Abstractions.Contracts.IAdapter adapter;
        private List<MyDeviceView> deviceList;

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

            adapter.DeviceDiscovered += (s, a) =>
            {
                if (a.Device.Name.Contains("Beacon"))
                {
                    if (deviceList.Exists(d => d.id == a.Device.Id))
                    {
                        var dev = deviceList.Find(d => d.id == a.Device.Id);
                        dev.UpdateCountdown();
                    }
                    else
                    {
                        deviceList.Add(new MyDeviceView(a.Device));
                    }
                }
            };
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