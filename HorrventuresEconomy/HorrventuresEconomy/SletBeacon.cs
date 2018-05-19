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
    public partial class SletBeacon :BaseBeacon
    {
        public Owner owner;
        public SletBeacon ( string id, SletBeacon.Owner owner)
        {
            Id = id;
            this.owner = owner; 
        }
        public override string ToString()
        {
            return (String.Format("ID : {0} Command :{1}", Id, owner.ToString()));
        }

    }      
}