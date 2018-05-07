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

        private ISharedPreferences preferences;
        private ISharedPreferencesEditor prefEditor;
        static private string CURRENCY_KEY = "currency";
        static public int CycleTimeMs = 1000;

        public EconomyLogic()
        {
            // Так надо, я все потом перепишу
            bleDataLayer = new BluetoothDataLayer();
            beaconFilter = new BeaconFilter(bleDataLayer);
            deviceList = new List<MyDeviceView>();



            timer = new Timer
            {
                Interval = CycleTimeMs
            };
            timer.Elapsed += OnTimerTick;
            timer.Start();
            preferences = Application.Context.GetSharedPreferences("curr",0);
            prefEditor = preferences.Edit();
            Currency = preferences.GetFloat(CURRENCY_KEY, 0);

        }
        private void putCurrencyToPrefs()
        {
            prefEditor.PutFloat(CURRENCY_KEY, (float) Currency);
            prefEditor.Apply();
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
            bleDataLayer.ResumeScan();
        }


        private void OnTimerTick(object sender, ElapsedEventArgs e)
        {
            UpdateCurrency();
            UpdateCityState();
        }

        private void UpdateCityState()
        {
            double total_multiplier = 1;
            double total_income_per_hour = 0;
            ForgeLevel = 0;
            AlchemyLevel = 0;
            PalaceLevel = 1;
            JewelryLevel = 0;
            LibraryLevel = 0;
            List<Beacon> beacons = beaconFilter.GetActiveBeeacons();
            foreach (Beacon  beacon in beacons)
            {
                total_multiplier *= beacon.Mulltiplier;
                total_income_per_hour += beacon.IncomePerHour;
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
                    case Beacon.BeaconType.LIBRAY:
                        LibraryLevel++;
                        break;
                    default:
                        break;
                }
            }
           
            currentIncome = (total_income_per_hour/(3600f* 1000f))*CycleTimeMs * total_multiplier;


        }

        private void UpdateCurrency()
        {
            ///deviceList = deviceData.GetDeviceList();
            Currency += currentIncome;
            putCurrencyToPrefs();
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
            putCurrencyToPrefs();
        }

        public bool RetrieveMoney(int amount)
        {
            if (amount < Currency)
            {
                Currency -= amount;
                putCurrencyToPrefs();
                return true;
            }
            return false;
        }
    }
}