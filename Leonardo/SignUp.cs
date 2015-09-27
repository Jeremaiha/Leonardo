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
using System.Threading;

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo")]
    public class SignUp : Activity
    {

        bool problemOccured;
        bool _alreadyRegistered;
        /*Sound*/
        public static int STREAM_MUSIC = 0x00000003;
        SoundPool sp;
        int SoundPushButton;
        /*Sound*/
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.SignUp);
                /*Sound*/
                Stream st = new Stream();
                sp = new SoundPool(1, st, 0);
                SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
                /*Sound*/

                problemOccured = false;
                // Assign the button.
                Button submitBtn = FindViewById<Button>(Resource.Id.buttonSubmit);
                EditText name = FindViewById<EditText>(Resource.Id.editText1);
                EditText email = FindViewById<EditText>(Resource.Id.editText2);
                EditText password = FindViewById<EditText>(Resource.Id.editText3);
                
                sumbitClick(submitBtn,name,email,password);
        
            }catch(Exception e){
                throw new Exception("Error : In SignUp.\n" + e.Message);
            }
        }

        /// <summary>
        ///     Submit button clicked, checks all details.
        ///     Registers if validated correctly
        /// </summary>
        /// <param name="submitBtn"></param>
        private async void sumbitClick(Button submitBtn,EditText name,EditText email, EditText password)
        {
            submitBtn.Click += async (sender, e) =>
            {
                try{
                    /*Sound*/
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);


                    // All details must me entered.
                    if (name.Text == "" || email.Text == "" || password.Text == ""){
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Please enter all details!");
                        callDialog.Show();           
                        return;
                    }

                    disableScreen(false,name,email,password ,submitBtn);
                    await getUser(name,email,password);
                    // An exception was thrown. let's end the task.
                    if (problemOccured){
                        Finish();
                        return;
                    }
                    // Something went wrong.
                    if (MainActivity.player.Instantiated == false){
                        StartActivity(typeof(MainActivity));
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Wrong Email Address!\nOr you already registered!");
                        callDialog.SetNeutralButton("Woops", delegate { });
                        callDialog.Show();
                    }else{ // User added.
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await addUserToParse();
                        await Task.Delay(2000);
                        Toast.MakeText(this, MainActivity.player.Name + " Was Added", ToastLength.Short).Show();
                        Finish();
                    }
                    disableScreen(true, name, email, password, submitBtn);
                }catch (Exception ex){
                    throw new Exception("Error : Creation of a user.\n" + ex.Message);
                }
                
            };
        }

        /// <summary>
        ///     By param flag, disables or enables the button and 3 textboxes.
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="btn"></param>
        private void disableScreen(bool flag,EditText e1,EditText e2, EditText e3,Button btn)
        {   
            e1.Enabled = flag;
            e2.Enabled = flag;
            e3.Enabled = flag;
            btn.Enabled = flag;
        }

        /// <summary>
        ///     Receives the user data, 
        ///     If it's correct, he's signed in.
        ///     Else, Error dialog box.
        /// </summary>
        /// <returns></returns>
        private async Task getUser(EditText name,EditText email,EditText password)
        {
            try{
                await alreadyRegistered(email.Text);
               
                MainActivity.player.Name = name.Text;
                MainActivity.player.Email = email.Text;
                MainActivity.player.Password = password.Text;

            }catch (FormatException){
                //throw new Exception("Error : Email address already exists.\n");
                showMessage("Undefined email address!");
                problemOccured = true;
            }catch (Exception){
                showMessage("Check your internet connection!");
                problemOccured = true;
            }
        }

        private void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

        private async Task addUserToParse()
        {
            // THIS SECTION DOESN"T WORK
            /*
            var user = new ParseUser();
            {
                Username = MainActivity.player.Name,
                Password = MainActivity.player.Password,
                Email = MainActivity.player.Email

            };
            /*

            // other fields can be set just like with ParseObject
            user["Score"] = 0;
            */
            var user = new ParseObject("Users")
            {
                { "Name", MainActivity.player.Name },
                { "Email", MainActivity.player.Email},
                { "Password",MainActivity.player.Password } ,
            };
            user["Score"] = "1";
         
            await user.SaveAsync();//SaveAsync
        }

        /// <summary>
        ///     Should check in the database if the email already exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task alreadyRegistered(string email)
        {
            try
            {
                //var query = ParseUser.Query.order", email);
                // ask for 10 in parse.
                // an option to show yourself too.
                var query = ParseUser.Query.WhereEqualTo("email", email);

                IEnumerable<ParseObject> results = await query.FindAsync();

                if (results.Count() == 0){
                    _alreadyRegistered = false;

                }else{
                    _alreadyRegistered = true;
                }
            }catch (FormatException){
                throw new FormatException();
            }
            
        }

    }
}