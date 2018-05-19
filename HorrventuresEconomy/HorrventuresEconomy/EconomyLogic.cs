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
        //private BluetoothDataLayer deviceData;
        private const double BEACONCOSTPERTIME = 1;

        private BeaconFilter beaconFilter;
        private BluetoothDataLayer bleDataLayer;

        public double currentIncome;
        // Combine to struct/class/colloctions
        public int ForgeLevel;
        public int PalaceLevel;
        public int AlchemyLevel;
        public int JewelryLevel;
        public int LibraryLevel;




        public EconomyLogic()
        {
            // Так надо, я все потом перепишу
            bleDataLayer = new BluetoothDataLayer();
            beaconFilter = new BeaconFilter(bleDataLayer);
            deviceList = new List<MyDeviceView>();
            

            timer = new Timer
            {
                Interval = 10000
            };
            timer.Elapsed += OnTimerTick;
            timer.Start();
        }
        /// <summary>
        /// Запуск экономики. Сбрасывает все в ноль.
        /// </summary>
        public void Start()
        {
            timer.Start();
            RefreshCurrency();


        }
        public void Pause()
        {
            timer.Stop();
            bleDataLayer.StopScan();

        }

        public void Resume()
        {
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
            List<HVBeacon> beacons = beaconFilter.GetActiveBeeacons();
            foreach (HVBeacon  beacon in beacons)
            {
                total_multiplier *= beacon.Mulltiplier;
                total_income += beacon.IncomePerMinute;
                switch (beacon.beaconType)
                {
                    case HVBeacon.BeaconType.ALCHEMY :
                        AlchemyLevel++;
                        break;
                    case HVBeacon.BeaconType.FORGE:
                        ForgeLevel++;
                        break;
                    case HVBeacon.BeaconType.JEWELRY:
                        JewelryLevel++;
                        break;
                    case HVBeacon.BeaconType.PALACE:
                        PalaceLevel++;
                        break;
                    case HVBeacon.BeaconType.LIBRAY:
                        LibraryLevel++;
                        break;
                    default:
                        break;
                }
            }

            currentIncome = total_income * total_multiplier;


        }

        private void UpdateCurrency()
        {
            ///deviceList = deviceData.GetDeviceList();
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