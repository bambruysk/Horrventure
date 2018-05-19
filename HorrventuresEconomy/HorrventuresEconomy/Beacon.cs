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
    public partial class HVBeacon : BaseBeacon
    {

        public double Mulltiplier;
        public double IncomePerMinute;

        public BeaconType beaconType;

        public HVBeacon(string id, double mulltiplier, double incomePerMinute, HVBeacon.BeaconType beacon_type)
        {
            Id = id;
            Mulltiplier = mulltiplier;
            IncomePerMinute = incomePerMinute;
            beaconType = beacon_type;
        }

        public override string ToString()
        {
            string BeaconTypeStr;
            switch (beaconType)
            {
                case BeaconType.ARTIFACT:
                    BeaconTypeStr = "Артефакт";
                    break;
                case BeaconType.FORGE:
                    BeaconTypeStr = "Кузница";
                    break;  
                case BeaconType.ALCHEMY:
                    BeaconTypeStr = "Алхимическая лаборатория";
                    break;
                case BeaconType.JEWELRY:
                    BeaconTypeStr = "Ювелирная матерская";
                    break;
                case BeaconType.PALACE:
                    BeaconTypeStr = "Дворец";
                    break;
                case BeaconType.LIBRAY:
                    BeaconTypeStr = "Библиотека";
                    break;
                default:
                    BeaconTypeStr = "Нет типа";
                    break;
            }
            return (String.Format("ID: {0}  Mult : {1} Income {2} Тип : {3}", Id, Mulltiplier,IncomePerMinute, BeaconTypeStr));
        }

    }
}