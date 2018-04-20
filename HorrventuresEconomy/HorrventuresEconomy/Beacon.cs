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


namespace HorrventuresEconomy
{
    [Serializable]
    public partial class Beacon
    {
        public string Id;
        public double Mulltiplier;
        public double IncomePerMinute;

        public BeaconType beaconType;

        public Beacon(string id, double mulltiplier, double incomePerMinute, Beacon.BeaconType beaconType)
        {
            Id = id;
            Mulltiplier = mulltiplier;
            IncomePerMinute = incomePerMinute;
            this.beaconType = beaconType;
        }

        //beaconType { get => beaconType; set => beaconType = value; }



    }
}