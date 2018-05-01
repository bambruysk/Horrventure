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
using Java.Lang;

namespace HorrventuresEconomy
{
    class ScannedDeviceAdapter : ArrayAdapter <MyBleDevice>
    {
        Context myContext;
        List<MyBleDevice> devices;



        public ScannedDeviceAdapter(Context myContext, List<MyBleDevice> devices):base (myContext, Resource.Id.foundDeviceView,devices)
        {
            this.myContext = myContext;
            this.devices = devices;
          SetNotifyOnChange(true);
        }

        public override void SetNotifyOnChange(bool notifyOnChange)
        {
            base.SetNotifyOnChange(notifyOnChange);
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //return base.GetView(position, convertView, parent);

            MyBleDevice device = devices[position];

            if (convertView == null)
            {
                convertView = LayoutInflater.FromContext(myContext)
                        .Inflate(Android.Resource.Layout.SimpleListItem2, null);
            }
            ((TextView)convertView.FindViewById <TextView>(Android.Resource.Id.Text1))
                    .SetText(device.Device.Name,TextView.BufferType.Normal);
            ((TextView)convertView.FindViewById<TextView>(Android.Resource.Id.Text2))
                    .SetText(device.Device.Id.ToString(), TextView.BufferType.Normal);
            return convertView;
        }

        public override void NotifyDataSetChanged()
        {
            base.NotifyDataSetChanged();
        }

        public override void Add(Java.Lang.Object @object)
        {
            base.Add(@object);
        }
    }
}