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
using Parse;
namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo:Sign In")]
    public class SignIn : Activity
    {
        bool userExists;
        bool problemOccured; // if a problem will occure later on, variable will be set true.

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

            // Instantiation.
            problemOccured = false;

            // Sound
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
         
            // Assign activity variables.
            EditText email = FindViewById<EditText>(Resource.Id.editTextEmail);
            EditText password = FindViewById<EditText>(Resource.Id.editTextPass);
            Button signInBtn = FindViewById<Button>(Resource.Id.signInBtn);

            if (MainActivity.player.Email != null){
                email.Text = MainActivity.player.Email;
                password.Text = MainActivity.player.Password;
            }
          
            signInButton(signInBtn, email, password);
         

            
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
        ///     Sign In, if details are correct.
        /// </summary>
        /// <param name="signInBtn"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private async void signInButton(Button signInBtn, EditText email, EditText password)
        {
            signInBtn.Click += async (sender, e) =>
            {
                try{
                   
                    // play sound, because the button was clicked.
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);

                    
                    // All details must me entered.
                    if (email.Text == "" || password.Text == "")
                    {
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Please enter all details!");
                        callDialog.Show();
                        return;
                    }
                    // Disable screen while the data is syncronized, so that you won't be able to 
                    //  click nor edit.
                    disableScreen(false,email, password, signInBtn);

                    await getUser(email.Text, password.Text);
                  
                    // If an exception is thrown. let's end the task.
                    if (problemOccured){
                        Finish();
                        return;
                    }
                    // Details are satisfying.
                    // Entered email already exists.
                    if (userExists){ // User is logged.
                        //await Task.Delay(2000);
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000);
                        showMessage("Logged in as " + MainActivity.player.Name);
                        StartActivity(typeof(Registered));
                    }
                    else{ // Didn't find such user.    
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000);
                        showMessage("No such user found");
                    }
                    Finish();
                }
                catch (Exception)
                {
                    showMessage("Unknown error has occured while trying to create the user.");
                }

            };
        }

        /// <summary>
        ///     By param flag, disables or enables the button and 2 textboxes.
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="btn"></param>
        private void disableScreen(bool flag, EditText e1, EditText e2, Button btn)
        {
            e1.Enabled = flag;
            e2.Enabled = flag;
            btn.Enabled = flag;
        }
        
        /// <summary>
        ///     Receives the user data, 
        ///     If it's correct, he's signed in.
        ///     Else, Error dialog box.
        /// </summary>
        /// <returns></returns>
        private async Task getUser(string email, string password)
        {
            try{
                await checkIfUserRegistered(email,password);
                
            }
            catch (FormatException){
                showMessage("Undefined email address!");
                problemOccured = true;
            }
            catch (Exception){
                showMessage("Check your internet connection!");
                problemOccured = true;
            }
        }

        private async Task checkIfUserRegistered(string email,string pass)
        {
            try{
               
                var query = from Users in ParseObject.GetQuery("Users")
                            where Users.Get<string>("Email") == email
                            where Users.Get<string>("Password") == pass
                            select Users;

                IEnumerable<ParseObject> results = await query.FindAsync();

                if (results.Count() == 0){
                    userExists = false;

                }else{ //sign user if needed.
                    var name = results.ElementAt(0).Get<string>("Name");
                    setParseUser(name,email,pass);
                    userExists = true;
                }
            }
            catch (FormatException){
                throw new FormatException();
            }
        }

        private void setParseUser(string name,string email, string password)
        {
            MainActivity.player.Name = name;
            MainActivity.player.Email = email;
            MainActivity.player.Password = password;
        }

        private User getUserFromParse(string email,string pass){
            // CHECK FROM DATA BASE!
            return null;//new User("TEMP",email,pass);
        }

        /// <summary>
        ///     Get's a string, and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        private void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

    }
}