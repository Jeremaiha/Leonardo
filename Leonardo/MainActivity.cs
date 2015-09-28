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
        public static User player;
        /*Sound*/
        public static int STREAM_MUSIC = 0x00000003;
        SoundPool sp;
        int SoundPushButton;
        /*Sound*/
        private TextView registeredUser;
        private static string registrationString = "Unregistered";
        private static int userScore = 0;

  
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Main);
                /*Sound*/
                Stream st = new Stream();
                sp = new SoundPool(1, st, 0);
                SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
                /*Sound*/
                player = User.CurrentUser;

              

                // If data was sent, the new user name is replaced.
          //      registeredUser = FindViewById<TextView>(Resource.Id.textView1);
            //    registeredUser.Text = registrationString;


                // Instantiate the events.
                buttonsClicks();
               
            
            }catch(Exception e){
                throw new Exception("Error : In MainActivity.\n" + e.Message);
            }
            
        }
        /*
        protected override void OnResume()
        {
            if (player.Instantiated == false)
            {
                StartActivity(typeof(Registered));
            }
            
        }

        */
        /// <summary>
        ///     Buttons clicks events initialization.
        /// </summary>
        private void buttonsClicks(){

            // Get our button from the layout resource,
            // and attach an event to it
          /*  ImageButton imageButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            imageButton.Click += delegate
            {
           
                sp.Play(SoundPushButton, 1,1,0,0,1);
                if (registeredUser.Text == "Unregistered"){
                    StartActivity(typeof(Game));
                }else{
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Please Sign In/Up!");
                    callDialog.SetNeutralButton("Ok", delegate { });
                    callDialog.Show();

                }
                
            };
        */
            Button signInButton = FindViewById<Button>(Resource.Id.button1);
            signInButton.Click += delegate
            {
                /*Sound*/
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                /*Sound*/
                StartActivity(typeof(SignIn));
            };

            Button signUpButton = FindViewById<Button>(Resource.Id.button2);
            signUpButton.Click += delegate
            {
                /*Sound*/
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                /*Sound*/
                StartActivity(typeof(SignUp));
            };

            Button Top10Button = FindViewById<Button>(Resource.Id.button3);
            Top10Button.Click += delegate
            {
                /*Sound*/
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                /*Sound*/
                StartActivity(typeof(TopScore));
            };

        }

    }
}

