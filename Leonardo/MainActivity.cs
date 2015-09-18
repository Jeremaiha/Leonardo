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

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

//            soundPlayer = MediaPlayer.Create(this,Resource.Raw.clickInMenu);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            ImageButton imageButton = FindViewById<ImageButton>(Resource.Id.imageButton1);
            imageButton.Click += delegate {
                //soundPlayer.Start();
                StartActivity(typeof(Game));
            };

            Button signInButton = FindViewById<Button>(Resource.Id.button1);
            signInButton.Click += delegate {
  //              soundPlayer.Start();
                StartActivity(typeof(SignIn));
            };

            Button signUpButton = FindViewById<Button>(Resource.Id.button2);
            signUpButton.Click += delegate{
                StartActivity(typeof(SignUp));
            };

        }
    }
}

