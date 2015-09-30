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
using Android.Media;

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::Registered")]

    public class Registered : Activity
    {
        // Sound variables.
        SoundPool sp;
        int SoundPushButton;

        /// <summary>
        ///     Responsible to : 
        ///         Initialize all buttons.
        ///         Show the logged in user.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try{
                // Activity initialization.
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Registered);

                // Shows the registered user in a textbox under the Leonardo logo.
                showRegistered();

                loadSound();

                // Assign all buttons action listeners.
                buttonsClicks();
            }catch (Exception){
                showMessage("An error has occured while trying to show the page");
            }
            
        }

        /// <summary>
        ///     Responsible to assign all buttons clicks (Action listeners).
        /// </summary>
        private void buttonsClicks()
        {
            try{
                // Play Button.
                ImageButton playImgBtn = FindViewById<ImageButton>(Resource.Id.playImgBtn);
                playButtonClick(playImgBtn);

                // Sign Out Button.
                Button signOutBtn = FindViewById<Button>(Resource.Id.signOutBtn);
                signOutClick(signOutBtn);

                // How To Play Button.
                Button how2playBtn = FindViewById<Button>(Resource.Id.how2playBtn);
                how2playBtnClick(how2playBtn);
            
                // Top 10 Button.
                Button Top10Button = FindViewById<Button>(Resource.Id.top10Btn);
                top10Click(Top10Button);
            }catch (Exception){
                showMessage("Unknown error has occured in buttons.");
            }
            
        }

        /// <summary>
        ///     A button which will open a new activity,
        ///         to show the top 10 in score.
        ///     Another functionallity is to see yourself in 
        ///         the score board.
        /// </summary>
        /// <param name="topBtn"></param>
        private void top10Click(Button topBtn)
        {
            try{
                topBtn.Click += delegate
                {
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.
                    StartActivity(typeof(TopScore));
                };
            }catch(Exception){
                showMessage("An error has occured while trying to open Top 10");
            }
            
        }

        /// <summary>
        ///     Opens a new activity, where you can
        ///         play, and your score will be updated in the data base.
        /// </summary>
        /// <param name="playImgBtn"></param>
        private void playButtonClick(ImageButton playImgBtn)
        {
             try{
                playImgBtn.Click += (sender, e) =>
                {
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.
                    StartActivity(typeof(Game));
                };
             }catch (Exception){
                 showMessage("An error has occured while trying to play");
             }
        }

        /// <summary>
        ///     Passes you back to the MainActivity,
        ///         and a log out is being done.
        /// </summary>
        /// <param name="signOutBtn"></param>
        private void signOutClick(Button signOutBtn)
        {
            try{
                signOutBtn.Click += (sender, e) =>
                {
                    clearUser();
                    showMessage(MainActivity.player.Name + " Signed Out");
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.

                    // Go back to the main activity,
                    // such that the Registered activity won't be in the stack anymore.
                    Finish(); 
                };
            }catch (Exception){
                showMessage("An error has occured while trying to sign out");
            }
         }
        private void how2playBtnClick(Button how2playBtn)
        {
            try{
                how2playBtn.Click += (sender, e) =>
                {
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.
                    StartActivity(typeof(HowToPlay));
                };
            }catch (Exception){
                showMessage("An error has occured while going to 'How to play' activity");
            }
        }

        /// <summary>
        ///     Loads the sound variables, 
        ///     which will later on activate the sound on a click.
        /// </summary>
        private void loadSound()
        {
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
        }

        /// <summary>
        ///     When the user signs out, 
        ///         we don't longer have a current user,
        ///         or at least we have a current user which is nobody (null).
        /// </summary>
        private void clearUser()
        {
            MainActivity.player.Email    = null;
            MainActivity.player.Password = null;
            MainActivity.player.Name     = null;
        }

        /// <summary>
        ///     Show the registered user in the top textbox.
        /// </summary>
        private void showRegistered()
        {
            TextView textView = FindViewById<TextView>(Resource.Id.registeredLabel);
            textView.Text = MainActivity.player.Name;
        }

        /// <summary>
        ///     Gets a string, and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        public void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

    }
}