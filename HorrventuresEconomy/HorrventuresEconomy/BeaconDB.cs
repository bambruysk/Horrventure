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
        static public Dictionary<string, Beacon> Beacons_db;
        static public  string db_filename;
 
        static public void  Initialize()
        {
            db_filename = Path.Combine(Application.Context.FilesDir.Path, "BeaconList.xml");
            Beacons_db = new Dictionary<string, Beacon>();
            if (File.Exists(path: db_filename)) 
            {
                Upload();
            }


        }

        static public void Upload()
        {

            using (Stream reader = new FileStream(db_filename, FileMode.Open))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                
                Beacons_db = (Dictionary<string, Beacon>)serializer.Deserialize(reader);
                reader.Close();
            }
  
        }

        static public void SaveDB()
        {

            using (Stream writer = new FileStream(db_filename, FileMode.OpenOrCreate,FileAccess.Write))
            {
                BinaryFormatter serializer = new BinaryFormatter();
                serializer.Serialize(writer, Beacons_db);
                writer.Close();
            }
        }

        static public void SaveDB(List<Beacon> new_beacons)
        {
            Beacons_db.Clear();
            foreach (var b in new_beacons)
            {
                Beacons_db.TryAdd(b.Id, b);
            }
            SaveDB();
        }

        static public bool RegisterNewBeacon(Beacon beacon)
        {
            var result = Beacons_db.TryAdd(beacon.Id, beacon);
            SaveDB();
            return result;
        }

        static public bool RegisterNewBeacon (MyDeviceView device, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            var result = Beacons_db.TryAdd(device.id.ToString(), new Beacon(device.id.ToString(), mulltiplier, incomePerMinute, beaconType));
            SaveDB();
            return result;
        }

        static public bool RegisterNewBeacon(string id, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            Console.WriteLine("In Register New Beacon");
            var result = Beacons_db.TryAdd(id, new Beacon(id, mulltiplier, incomePerMinute, beaconType));
            SaveDB();
            return result;
        }

        static public bool Contains (Beacon beacon )
        {
            return Beacons_db.ContainsKey(beacon.Id);
        }
        static public bool Contains (IDevice device)
        {
            return Beacons_db.ContainsKey(device.Id.ToString());
        }

        static public bool Contains(string id)
        {
            return Beacons_db.ContainsKey(id);
        }
        static public Beacon Get (string id)
        {
            return Beacons_db[id];
        }
    }
}