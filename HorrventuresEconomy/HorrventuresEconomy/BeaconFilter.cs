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
        private BeaconDB beaconsDB;

        private BluetoothDataLayer bluetooth;

        public BeaconFilter()
        {
            bluetooth = new BluetoothDataLayer();
            beaconsDB = new BeaconDB();

        }

        public List<Beacon> GetActiveBeeacons()
        {
            List<Beacon> beacons = new List<Beacon>();
            List<MyDeviceView> devlist =  bluetooth.GetDeviceList();
            foreach (MyDeviceView device in devlist) {
                if (beaconsDB.Contains(device.id.ToString()))
                {
                    beacons.Add(beaconsDB.Get(device.id.ToString()));
                }
            }
            return beacons;
        }
    }
}