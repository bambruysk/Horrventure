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
using System.Runtime.Serialization.Formatters.Binary;

namespace HorrventuresEconomy
{
    [Serializable]
    public class BeaconDB
    {
        
        private Dictionary<string, Beacon> beacons_db;
        public  string db_filename = "BeaconList.xml";

        private BinaryFormatter serializer;

        private Beacon test_beacon;

        public Dictionary<string, Beacon> Beacons_db { get => beacons_db; }

        public BeaconDB()
        {
            string fileDir = Application.Context.FilesDir.Path;
            db_filename = Path.Combine(fileDir, db_filename);
            serializer = new BinaryFormatter();
            beacons_db = new Dictionary<string, Beacon>();
            if (File.Exists(path: db_filename))
            {
                using (Stream reader = new FileStream(db_filename, FileMode.Open))
                {
                    if (reader.IsDataAvailable())
                        beacons_db = (Dictionary<string, Beacon>)serializer.Deserialize(reader);
                    reader.Close();
                }
            }
            else
            {
                using (FileStream fs = File.Create(db_filename, 1, FileOptions.RandomAccess))
                {
                    fs.Close();
                }
                
            }
            // Test purpose
            test_beacon = new Beacon("test", 1, 1, Beacon.BeaconType.JEWELRY);
            beacons_db.Add("test", test_beacon);
            RegisterNewBeacon("01234", 1.0f, 0.1f, Beacon.BeaconType.ALCHEMY);
            RegisterNewBeacon("5678", 2.0f, 0.3f, Beacon.BeaconType.PALACE);

        }
        private void SaveDB()
        {

            Stream writer = new FileStream(db_filename, FileMode.Open);
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