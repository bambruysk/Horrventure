using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.Xml.Serialization;
using System.Xml;

namespace HorrventuresEconomy
{
    public class BeaconFilter
    {
        

        private BluetoothDataLayer bluetooth;

        public BeaconFilter(BluetoothDataLayer bluetooth)
        {
            this.bluetooth = bluetooth;
            

        }

        public List<Beacon> GetActiveBeeacons()
        {
            List<Beacon> beacons = new List<Beacon>();

            List<MyDeviceView> devlist =  bluetooth.GetDeviceList();
            BeaconDB.Upload();
            foreach (MyDeviceView device in devlist) {
                if (BeaconDB.Contains(device.id.ToString()))
                {
                    beacons.Add(BeaconDB.Get(device.id.ToString()));
                }
            }
            return beacons;
        }
    }
}