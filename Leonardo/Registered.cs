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
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::Registered")]

    public class Registered : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Registered);

                showRegistered();

                Button signOutBtn = FindViewById<Button>(Resource.Id.signOutBtn);
                signOutClick(signOutBtn);


                Button how2playBtn = FindViewById<Button>(Resource.Id.how2playBtn);
                how2playBtnClick(how2playBtn);


                ImageButton playImgBtn = FindViewById<ImageButton>(Resource.Id.playImgBtn);
                playButtonClick(playImgBtn);

                Button Top10Button = FindViewById<Button>(Resource.Id.top10Btn);
                Top10Button.Click += delegate
                {
                    StartActivity(typeof(TopScore));
                };
            }
            catch (Exception)
            {
                showMessage("An error has occured while trying to show the page");
            }
            
        }

        private void playButtonClick(ImageButton playImgBtn)
        {
            playImgBtn.Click += (sender, e) =>
            {
                try{
                    StartActivity(typeof(Game));
                }
                catch (Exception){
                    showMessage("An error has occured while trying to play");
                }

            };
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
        private void how2playBtnClick(Button how2playBtn)
        {
            how2playBtn.Click += (sender, e) =>
            {
                try
                {
                    StartActivity(typeof(HowToPlay));
                }
                catch (Exception)
                {
                    showMessage("An error has occured while going to 'How to play' activity");
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