using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Content;
using Android.Content.PM;

namespace GlassManager
{
    [Activity(Label = "GlassManager", MainLauncher = true, Icon = "@mipmap/icon")]
    public class MainActivity : Activity
    {


        private ListViewPannel list_pannel;
        private ListView listView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            //Lets Create the Notification Preference OBJ
            Notification_Preferences_Manager.attempt_load_in();


        }




    }
}

