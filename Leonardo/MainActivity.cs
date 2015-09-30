using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

using Parse;

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // A static user, which will later on contain the current user which 
        //  logged on. Will help us to update his score too.
        public static User player;

        // Sound variables.
        SoundPool sp;
        int SoundPushButton;
        
        /// <summary>
        ///     Responsible to initialize : 
        ///     View.
        ///     Sound clicks.
        ///     Buttons clicks (Action Listeners). 
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Main);

                // Sound initialization.
                loadSound();

                // Point to the current user.     
                player = User.CurrentUser;

                // Instantiate the events.
                buttonsClicks();
            
            }catch(Exception){
                showMessage("Error : In MainActivity.\n");
                Finish();
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
        ///     Assign action listeners to the buttons : 
        ///         Sign In - To sign in.
        ///         Sign Up - To sign up.
        /// </summary>
        private void buttonsClicks(){

            Button signInButton = FindViewById<Button>(Resource.Id.button1);
            signInButton.Click += delegate
            {
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.
                StartActivity(typeof(SignIn));
            };

            Button signUpButton = FindViewById<Button>(Resource.Id.button2);
            signUpButton.Click += delegate
            {
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1); // Activate sound.
                StartActivity(typeof(SignUp));
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

