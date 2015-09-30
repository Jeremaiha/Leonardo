using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Leonardo
{
     [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::How To Play")]
    public class HowToPlay : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.HowToPlay);
            // Create your application here
            ImageButton playImgBtn = FindViewById<ImageButton>(Resource.Id.playImgBtn);
            playButtonClick(playImgBtn);

        }
        private void playButtonClick(ImageButton playImgBtn)
        {
            playImgBtn.Click += (sender, e) =>
            {
                try
                {
                    StartActivity(typeof(Game));
                }
                catch (Exception)
                {
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Unknown error has occured while trying to show the 'Game'page");
                    callDialog.Show();
                }

            };
        }
    }
}