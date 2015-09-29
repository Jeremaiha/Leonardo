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
using Android.Content.PM;

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Registered")]

    public class Registered : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Registered);

            showRegistered();

            Button signOutBtn = FindViewById<Button>(Resource.Id.signOutBtn);
            signOutClick(signOutBtn);
        }

        private void signOutClick(Button signOutBtn){
            signOutBtn.Click += (sender, e) =>{
                try{
                    string name = MainActivity.player.Name; 
                    clearUser();
                    showMessage(name + " Signed Out");
                    Finish();
//                    StartActivity(typeof(MainActivity));
                }catch (Exception){
                    showMessage("An error has occured while trying to sign out");
                }

            };
         }

        private void clearUser()
        {
            MainActivity.player.Email    = null;
            MainActivity.player.Password = null;
            MainActivity.player.Name     = null;

        }

        private void showRegistered()
        {
            TextView textView = FindViewById<TextView>(Resource.Id.registeredLabel);
            textView.Text = MainActivity.player.Name;
        }

        /// <summary>
        ///     Get's a string, and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        public void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

    }
}