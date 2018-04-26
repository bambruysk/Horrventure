using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.BLE.Abstractions.Contracts;
using System.IO;
namespace HorrventuresEconomy
{
    public class BeaconDB
    {
        private Dictionary<string, Beacon> beacons_db;
        private  string db_filename = "BeaconList.xml";



        private XmlSerializer serializer = new XmlSerializer(typeof(Dictionary<string, Beacon>));

        public string Db_filename { get => db_filename; set => db_filename = value; }

        public BeaconDB()
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
            

        }
        private void SaveDB()
        {

            XmlWriter writer = XmlWriter.Create(db_filename);
            serializer.Serialize(writer, beacons_db);
            writer.Close();
        }

        public void RegisterNewBeacon(Beacon beacon)
        {
            beacons_db.Add(beacon.Id, beacon);
            SaveDB();
        }

        public void RegisterNewBeacon (MyDeviceView device, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            beacons_db.Add(device.id.ToString(), new Beacon(device.id.ToString(), mulltiplier, incomePerMinute, beaconType));
            SaveDB();
        }

        public void RegisterNewBeacon(string id, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            beacons_db.Add(id, new Beacon(id, mulltiplier, incomePerMinute, beaconType));
            SaveDB();
        }

        public bool Contains (Beacon beacon )
        {
            return beacons_db.ContainsKey(beacon.Id);
        }
        public bool Contains (IDevice device)
        {
            return beacons_db.ContainsKey(device.Id.ToString());
        }

        public bool Contains(string id)
        {
            return beacons_db.ContainsKey(id);
        }
         public Beacon Get (string id)
        {
            return beacons_db[id];
        }
    }
}