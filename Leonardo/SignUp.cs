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
        private bool problemOccured;    // if a problem is occured later on, variable is set to be true.
        private bool alreadyRegistered; // if the email is found to be registered, the value will be set true.
        // Sound variables.
        private static int STREAM_MUSIC = 0x00000003;
        SoundPool sp;
        int SoundPushButton;
        
        protected override void OnCreate(Bundle bundle)
        {
            try{
                // Activity initialization.
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.SignUp);

                // Sound initialization.
                Stream st = new Stream();
                sp = new SoundPool(1, st, 0);
                SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
                // A variable which is used afterwards in the class.
                problemOccured = false;

                // Assign view variables.
                Button signUpBtn = FindViewById<Button>(Resource.Id.signUpBtn);
                EditText name = FindViewById<EditText>(Resource.Id.editText1);
                EditText email = FindViewById<EditText>(Resource.Id.editText2);
                EditText password = FindViewById<EditText>(Resource.Id.editText3);
                
                // Submit button click.
                signUpClick(signUpBtn,name,email,password);
        
            }catch(Exception){
                showMessage("Something went wrong in Sign Up button.");
                Finish();
            }
        }

        /// <summary>
        ///     Sign up if all details are ok.
        /// </summary>
        /// <param name="signUpBtn"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private async void signUpClick(Button signUpBtn,EditText name,EditText email, EditText password)
        {
            signUpBtn.Click += async (sender, e) =>
            {
                try{
                    // play button click sound.
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);

                    // All details must me entered.
                    if (name.Text == "" || email.Text == "" || password.Text == ""){
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Please enter all details!");
                        callDialog.Show();           
                        return;
                    }
                    // Disable screen while the data is syncronized, so that you won't be able to 
                    //  click nor edit.
                    disableScreen(false,name,email,password ,signUpBtn);

                    await getUser(name,email,password);
                    // If an exception is thrown. let's end the task.
                    if (problemOccured){
                        Finish();
                        return;
                    }
                    // Details are satisfying.
                    // Entered email already exists.
                    if (alreadyRegistered){
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000);
                        showMessage("Current email address is already registered.");
                    }else{ // User added.
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await addUserToParse();
                        await Task.Delay(2000);
                        showMessage(MainActivity.player.Name + " Was Added");
                        StartActivity(typeof(Registered));
                    }
                    Finish();
                }catch (Exception){
                    showMessage("Unknown error has occured while trying to create the user.");
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
        ///     Receives the user data from params. 
        ///     If it's correct, he's signed in.
        ///     Else, Error occurs.
        /// </summary>
        /// <returns></returns>
        private async Task getUser(EditText name,EditText email,EditText password)
        {
            try{
                await checkIfEmailRegistered(email.Text);
               
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
        /// <summary>
        ///     Get's a string, and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        private void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

        /// <summary>
        ///     Adding the user to the "Users" table in Parse.
        /// </summary>
        /// <returns></returns>
        private async Task addUserToParse()
        {
            var user = new ParseObject("Users")
            {
                { "Name", MainActivity.player.Name },
                { "Email", MainActivity.player.Email},
                { "Password",MainActivity.player.Password } ,
            };
            user["Score"] = 0;
         
            await user.SaveAsync();//SaveAsync
        }

        /// <summary>
        ///     Should check in the database if the email already exists.
        ///     If it does - alreadyRegistered variable is set to true.
        ///     Else       - alreadyRegistered = false.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task checkIfEmailRegistered(string email)
        {
            try{
               
                //var query = ParseUser.Query.order", email);
                // ask for 10 in parse.
                // an option to show yourself too.
                //var query = ParseUser.Query.WhereEqualTo("email", email);
                var query = from Users in ParseObject.GetQuery("Users")
                            where Users.Get<string>("Email") == email
                            select Users;

                IEnumerable<ParseObject> results = await query.FindAsync();

                if (results.Count() == 0){
                    alreadyRegistered = false;

                }else{
                    alreadyRegistered = true;
                }
            }catch (FormatException){
                throw new FormatException();
            }
            
        }

    }
}