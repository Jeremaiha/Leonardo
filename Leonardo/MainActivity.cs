using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Media;

using Parse;

namespace Leonardo
{
    [Activity(Label = "Leonardo", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        MediaPlayer soundPlayer;
        private TextView registeredUser;
        private static User currentUser;
        private static string registrationString = "Unregistered";
        private static int userScore = 0;
        /// <summary>
        ///     Delegate method, to pass from SignUp activity the user.
        /// </summary>
        /// <param name="name"></param>
        public static void passUsername(User user){
            currentUser = user;
            registrationString = user.Name;
        }

        public static void getScore(int score){
            userScore = score;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            soundPlayer = MediaPlayer.Create(this, Resource.Raw.clickInMenu);

            // If data was sent, the new user name is replaced.
            registeredUser = FindViewById<TextView>(Resource.Id.textView1);
            registeredUser.Text = registrationString;
         

            // Instantiate the events.
            buttonsClicks();

        }

        private void addUserToParse(){
            /*
            var query = ParseUser.GetQuery("User");
            query.WhereEqualTo("email", currentUser.Email);
            query.findInBackground(new FindCallback<ParseUser>() {
              public void done(List<ParseUser> objects, ParseException e) {
                if (e == null) {
                    // The query was successful.
                } else {
                    // Something went wrong.
                }
              }
            });
            */
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
                soundPlayer.Start();
                if (registeredUser.Text != "Unregistered"){
                    StartActivity(typeof(Game));
                }else{
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Please Sign In/Up!");
                    callDialog.SetNeutralButton("Ok", delegate { });
                    callDialog.Show();

                }
                
            };

            Button signInButton = FindViewById<Button>(Resource.Id.button1);
            signInButton.Click += delegate
            {
                soundPlayer.Start();
                StartActivity(typeof(SignIn));
            };

            Button signUpButton = FindViewById<Button>(Resource.Id.button2);
            signUpButton.Click += delegate
            {
                soundPlayer.Start();
                StartActivity(typeof(SignUp));
            };

        }

    }
}

