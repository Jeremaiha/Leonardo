using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

namespace Leonardo
{
    [Activity(Label = "Leonardo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
       // MediaPlayer soundPlayer;
        private TextView registeredUser;
        private static string registrationString = "Unregistered";
        /// <summary>
        ///     Delegate method, to pass from SignUp activity the user.
        /// </summary>
        /// <param name="name"></param>
        public static void passUsername(string name){
            registrationString = name;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            
            // If data was sent, the new user name is replaced.
            registeredUser = FindViewById<TextView>(Resource.Id.textView1);
            registeredUser.Text = registrationString;
         
//            soundPlayer = MediaPlayer.Create(this,Resource.Raw.clickInMenu);

            // Instantiate the events.
            buttonsClicks();

        }

        /// <summary>
        ///     Buttons clicks events initialization.
        /// </summary>
        private void buttonsClicks(){

            // Get our button from the layout resource,
            // and attach an event to it
            ImageButton imageButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            imageButton.Click += delegate
            {
                //soundPlayer.Start();
                // Was changed.
                if (registeredUser.Text != "Unregistered"){
                    StartActivity(typeof(Game));
                }
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Please Sign In/Up!");
                callDialog.SetNeutralButton("Ok", delegate { });
                callDialog.Show();

            };

            Button signInButton = FindViewById<Button>(Resource.Id.button1);
            signInButton.Click += delegate
            {
                //              soundPlayer.Start();
                StartActivity(typeof(SignIn));
            };

            Button signUpButton = FindViewById<Button>(Resource.Id.button2);
            signUpButton.Click += delegate
            {
                StartActivity(typeof(SignUp));
            };

        }

    }
}

