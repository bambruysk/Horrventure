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
    class AccessManager
    {

        private const string REG_PASSWD_KEY = "uigickuf";


        public enum AccessMode { PLAYER, MASTER, ADMIN, TECH }

        private ISharedPreferences prefs;
        private ISharedPreferencesEditor pref_edit;
        Context MyContext;

        private string  AdminPswd;

        public AccessManager(Context context)
        {
            AdminPswd = "14091982";
            MyContext = context;

            prefs = context.GetSharedPreferences("prefs",0);
            pref_edit = prefs.Edit();

        }

        public void ChangePassword(string new_pass)
        {
            pref_edit.PutString(REG_PASSWD_KEY, new_pass);
            pref_edit.Apply();
        }


        public bool CheckRegPass(string pass)
        {
            string get_pass = prefs.GetString(REG_PASSWD_KEY, "pass_not_set"); 
            if (get_pass == "pass_not_set"){
                Toast.MakeText(MyContext, "Пароль не усановлен. Задайте пароль в настройках",ToastLength.Long);
                return false;
            }  else
            {
                return (get_pass == pass);
            }
        }
        public bool CheckAdminPass(string pass) => (pass == AdminPswd);
    }
}