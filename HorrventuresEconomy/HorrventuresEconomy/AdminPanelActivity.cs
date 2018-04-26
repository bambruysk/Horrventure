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

namespace HorrventuresEconomy.Resources.layout
{


    [Activity(Label = "AdminPanelActivity")]
    public class AdminPanelActivity : Activity
    {

        private EditText editTextChangePswd;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.AdminPanel);

            RequestAdminPassword();

            Button buttonChangePswd = FindViewById<Button>(Resource.Id.editTextChangeRegPswd);
            editTextChangePswd = FindViewById<EditText>(Resource.Id.editTextChangeRegPswd);
            buttonChangePswd.Click += ButtonChangePswd_Click;

            Button buttonAddNewDeices = FindViewById<Button>(Resource.Id.adminPanelButtonAddNewDevices);
            buttonAddNewDeices.Click += delegate
            {
                Intent FindNewDevicesIntent = new Intent(this, typeof(FindNewDevice));
                StartActivity(FindNewDevicesIntent);
            };

            Button buttonFinish = FindViewById<Button>(Resource.Id.adminPanelFinish);
            buttonFinish.Click += (s,e) => { Finish(); };
        }

        private void RequestAdminPassword()
        {
           /// Add check pass here?
        }

        private void ButtonChangePswd_Click(object sender, EventArgs e)
        {
            var access_mamager = new AccessManager(this);
            access_mamager.ChangePassword(editTextChangePswd.Text);
        }
    }
}