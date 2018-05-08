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
    partial class RequestPassword
    {
        private AlertDialog.Builder alert;
        private  Context myContext;
        private View password;
        public delegate void RequestAction(Context context);

        public RequestAction Action;
        public RequestPassword(Context context, RequestAction action)
        {
            myContext = context;
            Action = action;
            LayoutInflater myInflater = LayoutInflater.From(context);
            password = myInflater.Inflate(Resource.Layout.InsertPassword, null);
            alert = new AlertDialog.Builder(context);
            alert.SetView(password);
        }
        public RequestPassword(Context context)
        {
            myContext = context;
            LayoutInflater myInflater = LayoutInflater.From(context);
            password = myInflater.Inflate(Resource.Layout.InsertPassword, null);
            alert = new AlertDialog.Builder(context);
            alert.SetView(password);
        }


        public bool GetPassword(RequestPassword.SecurityLevel securityLevel)
        {
            var userInput = password.FindViewById<EditText>(Resource.Id.passwordInput);
            alert.SetCancelable(false);
             bool checkPasswordResult = false;
            alert.SetPositiveButton("Ok", delegate
            {
                var accessManager = new AccessManager(myContext);
               
                switch (securityLevel)
                {
                    case SecurityLevel.USER:
                        checkPasswordResult = true;
                        break;
                    case SecurityLevel.REG:
                        checkPasswordResult = accessManager.CheckRegPass(userInput.Text);
                        break;
                    case SecurityLevel.ADMIN:
                        checkPasswordResult = accessManager.CheckAdminPass(userInput.Text);
                        break;
                }

                if (checkPasswordResult)
                {
                    if (Action != null)
                        Action.Invoke(myContext);
                }
                else
                {
                    Toast.MakeText(myContext, "Неправильный пароль", ToastLength.Short).Show();
                    alert.Dispose();
                }
            });
            alert.SetNegativeButton("Отмена", delegate
            {
                alert.Dispose();
            });
            Dialog dialog = alert.Create();
            dialog.Show();
            return checkPasswordResult;
        }
    }
}