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
        public object nativeDevice;

        public MyDeviceView(IDevice device)
        {
            id = device.Id;
            Countdown = 5;
            object nativeDevice = device.NativeDevice;
        }

        public void UpdateCountdown()
        {
            Countdown = 5;
        }
    }
}