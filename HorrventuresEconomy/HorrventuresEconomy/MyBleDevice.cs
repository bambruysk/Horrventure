using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;


using Android.Bluetooth;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Android;

namespace HorrventuresEconomy
{
    public class MyBleDevice 
    {
        public Device Device;

        public MyBleDevice(Device device)
        {
            this.Device = device;
        }


        public override string ToString()
        {
            string id_str = Device.Id.ToString();
            string id_chort_str = id_str.Substring(id_str.Length - 12);
            string name_str;
            if (this.Device.Name != null)
            {
                name_str = Device.Name.ToString();
            }
            else
            {
                name_str = "n/a";
            }
            return (id_chort_str + "   " + name_str);
        }
    }

}