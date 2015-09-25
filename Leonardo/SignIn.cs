using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;
namespace Leonardo
{
    [Activity(Label = "Leonardo")]
    public class SignIn : Activity
    {

        MediaPlayer soundPlayer;
        //FacebookLogIn facebookLogIn;
        //Button facebook;
        public delegate void delegatePassUser(User dlgUser);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SignIn);

            Button signinBtn = FindViewById<Button>(Resource.Id.buttonSignIn);
            signInButton(signinBtn);

            soundPlayer = MediaPlayer.Create(this, Resource.Raw.clickInMenu);

           // facebookLogIn = new FacebookLogIn();
           // facebook = FindViewById<Button>(Resource.Id.buttonFacebookLogIn);
            
         //   logInFacebookButton();
            var facebook = FindViewById<Button>(Resource.Id.buttonFacebookLogIn);
            facebook.Click += delegate { 
                LoginToFacebook(true);
                soundPlayer.Start();
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
        private void signInButton(Button signinBtn)
        {
            signinBtn.Click += (sender, e) =>
            {
                soundPlayer.Start();
                User user = getUser();
                if (user == null){
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Wrong Email Address!");
                    callDialog.SetNeutralButton("Ok I'll try again", delegate { });
                    callDialog.Show();
                }else{
                    Toast.MakeText(this, user.Name + " Registered", ToastLength.Short).Show();
                    delegatePassUser del = new delegatePassUser(Leonardo.MainActivity.passUsername);
                    del(user);
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