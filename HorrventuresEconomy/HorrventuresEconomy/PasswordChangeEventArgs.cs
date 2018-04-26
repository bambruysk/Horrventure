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
    public class PasswordChangeEventArgs
    {
        public string passwd;

        public PasswordChangeEventArgs(string passwd)
        {
            this.passwd = passwd;
        }
    }
}