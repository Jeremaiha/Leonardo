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
using System.Net.Mail;
namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Sign In")]
    public class SignIn : Activity
    {
        bool userExists;     // indicate if the user exists or not.
        bool problemOccured; // if a problem will occure later on, variable will be set true.

        // Sound variables.
        SoundPool sp;
        int SoundPushButton;
     
        protected override void OnCreate(Bundle bundle)
        {
            try{
                // Initiate activity.
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.SignIn);

                // Instantiation.
                problemOccured = false;

                // Sound
                loadSound();

                // Assign activity variables.
                EditText email = FindViewById<EditText>(Resource.Id.editTextEmail);
                EditText password = FindViewById<EditText>(Resource.Id.editTextPass);
                Button signInBtn = FindViewById<Button>(Resource.Id.signInBtn);

                // If he logged before, data is entered automatiaclly in textboxes.
                checkAlreadyLogged(email, password);

                // Sign him in.
                signInButton(signInBtn, email, password);
            }catch(Exception){
                showMessage("Unknown error has occured in the sign in page");
            }
            
        }

        /// <summary>
        ///     If the user already logged in before, and returned back to the main page,
        ///         he can log in again with the saved values.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private static void checkAlreadyLogged(EditText email, EditText password)
        {
            if (MainActivity.player.Email != null){
                email.Text = MainActivity.player.Email;
                password.Text = MainActivity.player.Password;
            }

        }

        /// <summary>
        ///     Load all sound variables.
        /// </summary>
        private void loadSound()
        {
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
        }
        
        

        /// <summary>
        ///     Sign In, if details are correct.
        /// </summary>
        /// <param name="signInBtn"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private void signInButton(Button signInBtn, EditText email, EditText password)
        {
            signInBtn.Click += async (sender, e) =>
            {
                try{
                    // Activate sound.
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
   
                    // Check for empty details.
                    if (checkEmptyBoxes(email, password)) {
                        Finish();
                        return;
                    }
                    
                    // Disable screen while the data is syncronized, 
                    //  so that you won't be able to click nor edit.
                    disableScreen(false,email, password, signInBtn);

                    // Check the user with the Parse database.
                    await getUser(email.Text, password.Text);
                  
                    // If an exception is thrown. let's end the task.
                    if (problemOccured){
                        Finish();
                        return;
                    }
                    
                    // Log in as the user.
                    if (userExists){ // Log in the user.
                        //await Task.Delay(2000);
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000);
                        showMessage("Logged in as " + MainActivity.player.Name);
                        StartActivity(typeof(Registered));
                    }
                    else{ // Didn't find such user - problem.    
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000);
                        showMessage("No such user found");
                    }
                    Finish();
                }
                catch (Exception){
                    showMessage("Unknown error has occured while trying to create the user.");
                }
            };
        }

        private bool checkEmptyBoxes(EditText email, EditText password)
        {
            if (email.Text == "" || password.Text == "")
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Please enter all details!");
                callDialog.Show();
                return true;
            }
            return false;
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
        ///     Else, error message.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
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

        /// <summary>
        ///     Check the user with the Parse data base,
        ///     If it works, it will sign the user, and save its details in the player.
        ///     if not, a message will be shown accordingly.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="pass"></param>
        /// <returns></returns>
        private async Task checkIfUserRegistered(string email,string pass)
        {
            try{
                // Check if email is ok.
                MailAddress m = new MailAddress(email);

               // Query to check email and password are correct.
                var query = from Users in ParseObject.GetQuery("Users")
                            where Users.Get<string>("Email") == email
                            where Users.Get<string>("Password") == pass
                            select Users;

                IEnumerable<ParseObject> results = await query.FindAsync();
                // If objects are found, check accordingly.

                if (results.Count() == 0){
                    userExists = false;

                }else{ // Sign the user.
                    var name = results.ElementAt(0).Get<string>("Name");
                    setParseUser(name,email,pass);
                    userExists = true;
                }
            }catch (FormatException){
                throw new FormatException();
            }catch(Exception){
                throw new Exception();    
            }
        }

        /// <summary>
        ///     Set the current user(player) with the values of the signed in user.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private void setParseUser(string name,string email, string password)
        {
            MainActivity.player.Name = name;
            MainActivity.player.Email = email;
            MainActivity.player.Password = password;
        }

        /// <summary>
        ///     Gets a string, and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        private void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }

    }
}