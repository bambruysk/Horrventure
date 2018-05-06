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
    
    static public class BeaconDB
    {
        
        static private Dictionary<string, Beacon> beacons_db;
        static public  string db_filename = "BeaconList.xml";

        static private BinaryFormatter serializer;


       static public Dictionary<string, Beacon> Beacons_db { get => beacons_db; }

        static   BeaconDB()
        {
            string fileDir = Application.Context.FilesDir.Path;
            db_filename = Path.Combine(fileDir, db_filename);
            serializer = new BinaryFormatter();
            beacons_db = new Dictionary<string, Beacon>();
            if (File.Exists(path: db_filename))
            {
                Upload();
            }
            else
            {
                using (FileStream fs = File.Create(db_filename, 1, FileOptions.RandomAccess))
                {
                    fs.Close();
                }
                
            }
        }



        static public Dictionary<string, Beacon> Upload()
        {
            beacons_db = new Dictionary<string, Beacon>();
            using (Stream reader = new FileStream(db_filename, FileMode.Open))
            {
                if (reader.IsDataAvailable())
                    beacons_db = (Dictionary<string, Beacon>)serializer.Deserialize(reader);
                reader.Close();
            }
            return beacons_db;
        }

        static public void SaveDB()
        {
            using (Stream writer = new FileStream(db_filename, FileMode.Open))
            {
                serializer.Serialize(writer, beacons_db);
                writer.Close();
            }
        }

        static public void SaveDB(List<Beacon> new_beacons)
        {
            beacons_db.Clear();
            foreach (var b in new_beacons)
            {
                beacons_db.Add(b.Id, b);
            }
            SaveDB();
        }

        static public void RegisterNewBeacon(Beacon beacon)
        {
            beacons_db.Add(beacon.Id, beacon);
            SaveDB();
        }

        static public void RegisterNewBeacon (MyDeviceView device, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            beacons_db.Add(device.id.ToString(), new Beacon(device.id.ToString(), mulltiplier, incomePerMinute, beaconType));
            SaveDB();
        }

        static public void RegisterNewBeacon(string id, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            beacons_db.Add(id, new Beacon(id, mulltiplier, incomePerMinute, beaconType));
            SaveDB();
        }

        static public bool Contains (Beacon beacon )
        {
            return beacons_db.ContainsKey(beacon.Id);
        }
        static public bool Contains (IDevice device)
        {
            return beacons_db.ContainsKey(device.Id.ToString());
        }

        static public bool Contains(string id)
        {
            return beacons_db.ContainsKey(id);
        }
        static public Beacon Get (string id)
        {
            return beacons_db[id];
        }
    }
}