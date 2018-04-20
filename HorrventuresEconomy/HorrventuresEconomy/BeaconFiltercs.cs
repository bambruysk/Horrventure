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
        private Dictionary<string, Beacon> beacons_db;
        private const string db_filename = "BeaconList.xml";

        private XmlWriter writer;
        private XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, Beacon>));

        private BluetoothDataLayer bluetooth = new BluetoothDataLayer();

        public BeaconFilter()
        {
           
            if (File.Exists(db_filename))
            {
                XmlReader reader = XmlReader.Create(db_filename);
                XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, Beacon>));
                beacons_db = (Dictionary<string, Beacon>) serializer.Deserialize(reader);
                reader.Close();
            }
            else
            {
                File.Create(db_filename);

                beacons_db = new Dictionary<string, Beacon>();

            }
            writer = XmlWriter.Create(db_filename);



        }

        private void SaveDB() => serializer.Serialize(writer, beacons_db);

        public void RegisterNewBeacon(Beacon beacon)
        {
            beacons_db.Add(beacon.Id, beacon);
        }

        public void RegisterNewBeacon (MyDeviceView device, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            beacons_db.Add(device.id.ToString(), new Beacon(device.id.ToString(), mulltiplier, incomePerMinute, beaconType));
        }

        public List<Beacon> GetActiveBeeacons()
        {
            List<Beacon> beacons = new List<Beacon>();
            List<MyDeviceView> devlist =  bluetooth.GetDeviceList();
            foreach (MyDeviceView device in devlist) {
                if (beacons_db.ContainsKey(device.id.ToString()))
                {
                    beacons.Add(beacons_db[device.id.ToString()]);
                }
            }
            return beacons;
        }
    }
}