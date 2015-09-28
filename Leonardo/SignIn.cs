using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;
namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo")]
    public class SignIn : Activity
    {

        /*Sound*/
        public static int STREAM_MUSIC = 0x00000003;
        SoundPool sp;
        int SoundPushButton;
        /*Sound*/
        //FacebookLogIn facebookLogIn;
        //Button facebook;
        public delegate void delegatePassUser(User dlgUser);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SignIn);

            // Sound
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
         
            // Assign activity variables.
            TextView email    = FindViewById<TextView>(Resource.Id.editText1);
            TextView password = FindViewById<TextView>(Resource.Id.editText2);
            Button signInBtn = FindViewById<Button>(Resource.Id.buttonSignIn);
            signInButton(signInBtn,email,password);

           

           // facebookLogIn = new FacebookLogIn();
           // facebook = FindViewById<Button>(Resource.Id.buttonFacebookLogIn);
            
         //   logInFacebookButton();
            var facebook = FindViewById<Button>(Resource.Id.buttonFacebookLogIn);
            facebook.Click += delegate { 
                LoginToFacebook(true);
                /*Sound*/
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                /*Sound*/
            };

         
        }
        /*
        private void logInFacebookButton(){
            facebook.Click += delegate { facebookLogIn.LoginToFacebook(true); };



        }
        */

        void LoginToFacebook(bool allowCancel)
        {



            var auth = new OAuth2Authenticator(
                   clientId: "877420372371465",
                   scope: "",
                   authorizeUrl: new Uri("https://m.facebook.com/dialog/oauth/"),
                   redirectUrl: new Uri("http://www.facebook.com/connect/login_success.html"));


            auth.AllowCancel = allowCancel;

            // If authorization succeeds or is canceled, .Completed will be fired.
            auth.Completed += (s, ee) =>
            {
                if (!ee.IsAuthenticated)
                {
                    var builder = new AlertDialog.Builder(this);
                    builder.SetMessage("Not Authenticated");
                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                    return;
                }

                // Now that we're logged in, make a OAuth2 request to get the user's info.
                var request = new OAuth2Request("GET", new Uri("https://graph.facebook.com/me"), null, ee.Account);
                request.GetResponseAsync().ContinueWith(t =>
                {
                    var builder = new AlertDialog.Builder(this);
                    if (t.IsFaulted)
                    {
                        builder.SetTitle("Error");
                        builder.SetMessage(t.Exception.Flatten().InnerException.ToString());
                    }
                    else if (t.IsCanceled)
                        builder.SetTitle("Task Canceled");
                    else
                    {
                        var obj = JsonValue.Parse(t.Result.GetResponseText());

                        builder.SetTitle("Logged in");
                        builder.SetMessage("Name: " + obj["name"]);
                    }

                    builder.SetPositiveButton("Ok", (o, e) => { });
                    builder.Create().Show();
                }, UIScheduler);
            };

            var intent = auth.GetUI(this);
            StartActivity(intent);
        }

        private static readonly TaskScheduler UIScheduler = TaskScheduler.FromCurrentSynchronizationContext();


        /// <summary>
        ///     Submit button clicked, checks all details.
        ///     Registers if validated correctly
        /// </summary>
        /// <param name="submitBtn"></param>
        private void signInButton(Button signinBtn,TextView email, TextView password)
        {
            signinBtn.Click += (sender, e) =>
            {
                /*Sound*/
                sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                /*Sound*/
                User user = getUser();
                if (user == null){
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Wrong Email Address!");
                    callDialog.SetNeutralButton("Ok I'll try again", delegate { });
                    callDialog.Show();
                }else{
                    Toast.MakeText(this, user.Name + " Registered", ToastLength.Short).Show();
                    StartActivity(typeof(MainActivity));
                }
            };
        }
        /// <summary>
        ///     Receives the user data, 
        ///     If it's correct, he's signed in.
        ///     Else, Error dialog box.
        /// </summary>
        /// <returns></returns>
        private User getUser()
        {
            try{
                var email = FindViewById<EditText>(Resource.Id.editTextEmail);
                var password = FindViewById<EditText>(Resource.Id.editTextPass);

                if(email.Text == "" || password.Text == ""){
                    return null;
                }
                
                return getUserFromParse(email.Text, password.Text);
                
            }
            catch (FormatException){
                return null;
            }
            catch (Exception e){
                throw new Exception("Error : Creation of a user.\n" + e.Message);
            }
        }

        private User getUserFromParse(string email,string pass){
            // CHECK FROM DATA BASE!
            return null;//new User("TEMP",email,pass);
        }



    }
}