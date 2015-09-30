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
    /// <summary>
    ///     The HowToPlay.axml, which is connected with this activity,
    ///     holds strings under "Strings.xml", this is a way in Android,
    ///     so that language replacement will be much easier, and instead
    ///     of holding the whole text in the layout, you just connect it 
    ///     to the specific @rule (for instance).
    ///     
    ///     styles.xml - Is holding all text definitions, such that you, again,
    ///     you won't be holding all of it in the layout itself.
    /// </summary>
     [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::How To Play")]
    public class HowToPlay : Activity
    {
        /// <summary>
        ///     Responsible to show us all game rules,
        ///     and a play button in the end if you already want to play!
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try{
                // Start activity.
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.HowToPlay);

                //  Play button in the end of the page.
                ImageButton playImgBtn = FindViewById<ImageButton>(Resource.Id.playImgBtn);
                playButtonClick(playImgBtn);
            }catch (Exception){
                showMessage("An error occured in the how to play button");
                Finish();
            }
        }

        /// <summary>
        ///     If the play button is clicked, starts the game.
        /// </summary>
        /// <param name="playImgBtn"></param>
        private void playButtonClick(ImageButton playImgBtn)
        {
            playImgBtn.Click += (sender, e) =>
            {
                try{
                    StartActivity(typeof(Game));
                }
                catch (Exception){
                    showMessage("Unknown error has occured while trying to show the 'Game'page");
                }
            };
        }

        /// <summary>
        ///     Gets a string and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        public void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }
    }
}