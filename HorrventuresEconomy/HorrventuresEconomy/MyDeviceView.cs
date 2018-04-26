using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Plugin.BLE.Abstractions.Contracts;

namespace HorrventuresEconomy
{
    public class MyDeviceView
    {
        public Guid id { get; }
        public int Countdown;

        public int RSSI;

//        public enum eDevType {FORGE, ALCHEMY, JEWELRY, PALACE, ARTIFACT}


        public MyDeviceView(IDevice device)
        {
            id = device.Id;
            Countdown = 5;
            RSSI = device.Rssi;

        }

        public void UpdateCountdown()
        {
            Countdown = 5;
        }
    }
}