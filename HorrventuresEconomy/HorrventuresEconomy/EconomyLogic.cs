using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace HorrventuresEconomy
{
    public class EconomyLogic : Java.Lang.Object
    {
        private double Currency;
        private Timer timer;
        public List<MyDeviceView> deviceList;
        private BluetoothDataLayer deviceData;
        private const double BEACONCOSTPERTIME = 1;

        private BeaconFilter beaconFilter = new BeaconFilter();

        public double currentIncome;

        public int ForgeLevel;
        public int PalaceLevel;
        public int AlchemyLevel;
        public int JewelryLevel;




        public EconomyLogic()
        {
            deviceList = new List<MyDeviceView>();
            deviceData = new BluetoothDataLayer();

            timer = new Timer
            {
                Interval = 10000
            };
            timer.Elapsed += OnTimerTick;
            timer.Start();
        }

        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            UpdateCurrency();
        }

        private void UpdateCityState()
        {
            double total_multiplier = 1;
            double total_income = 0;
            ForgeLevel = 0;
            AlchemyLevel = 0;
            PalaceLevel = 1;
            JewelryLevel = 0;
            List<Beacon> beacons = beaconFilter.GetActiveBeeacons();
            foreach (Beacon  beacon in beacons)
            {
                total_multiplier *= beacon.Mulltiplier;
                total_income += beacon.IncomePerMinute;
                switch (beacon.beaconType)
                {
                    case Beacon.BeaconType.ALCHEMY :
                        AlchemyLevel++;
                        break;
                    case Beacon.BeaconType.FORGE:
                        ForgeLevel++;
                        break;
                    case Beacon.BeaconType.JEWELRY:
                        JewelryLevel++;
                        break;
                    case Beacon.BeaconType.PALACE:
                        PalaceLevel++;
                        break;
                    default:
                        break;
                }
            }

            currentIncome = total_income * total_multiplier;


        }

        private void UpdateCurrency()
        {
            deviceList = deviceData.GetDeviceList();
            Currency += currentIncome;
        }




        public double GetCurrency() => Currency;

        public int GetIncomeRate()
        {
            if (deviceList.Count > 10)
            {
                return 5;
            }
            else if (deviceList.Count > 8)
            {
                return 4;
            }
            else if (deviceList.Count > 6)
            {
                return 3;
            }
            else if (deviceList.Count > 4)
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        public void RefreshCurrency()
        {
            Currency = 0;
        }

        public bool RetrieveMoney(int amount)
        {
            if (amount < Currency)
            {
                Currency -= amount;
                return true;
            }
            return false;
        }
    }
}