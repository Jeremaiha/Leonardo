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
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Sign Up")]
    public class SignUp : Activity
    {
        private bool problemOccured;    // if a problem is occured later on, variable is set to be true.
        private bool alreadyRegistered; // if the email is found to be registered, the value will be set true.
       
        // Sound variables.
        SoundPool sp;
        int SoundPushButton;
        
        /// <summary>
        ///     Responsible for : 
        ///         Buttons initialization.
        ///         Get user information from text boxes.
        ///         Signing him up if everything goes well.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try{
                // Activity initialization.
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.SignUp);

                // Sound initialization.
                loadSound(); 
                problemOccured = false;

                // Assign view variables.
                Button signUpBtn  = FindViewById<Button>(Resource.Id.signUpBtn);
                EditText name     = FindViewById<EditText>(Resource.Id.editText1);
                EditText email    = FindViewById<EditText>(Resource.Id.editText2);
                EditText password = FindViewById<EditText>(Resource.Id.editText3);
                
                // Submit button click with view variables.
                signUpClick(signUpBtn,name,email,password);
            }catch(Exception){
                showMessage("Something went wrong in Sign Up button.");
                Finish();
            }
        }

        /// <summary>
        ///     Load sound in click.
        /// </summary>
        private void loadSound()
        {
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
        }

        /// <summary>
        ///     Sign Up the user if all details are correct.
        ///     If details are incorrect, a message will be shown about the problem.
        /// </summary>
        /// <param name="signUpBtn"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        private void signUpClick(Button signUpBtn,EditText name,EditText email, EditText password)
        {
            signUpBtn.Click += async (sender, e) =>
            {
                try{
                    // Activate sound.
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);

                    // All details must me entered.
                    if (checkEmptyBoxes(name, email, password)){ // 
                        return;
                    }

                    // Disable screen while the data is syncronized, 
                    //      so that you won't be able to click nor edit.
                    disableScreen(false,name,email,password ,signUpBtn);

                    // Check if user exists already, if not, assign the current user.
                    await getUser(name,email,password);
                    
                    // If an exception is thrown. let's end the task.
                    if (problemOccured){
                        Finish();
                        return;
                    }
                    
                    // Entered email already exists.
                    if (alreadyRegistered){
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await Task.Delay(2000); // So that we will see the loading (The thread is basically taken).
                        showMessage("Current email address is already registered.");
                    
                    }else{ // User will be added now.
                        var progessDialog = ProgressDialog.Show(this, "Please wait...", "Checking account info...", true);
                        await addUserToParse(); // Add the user to the Parse data base.
                        await Task.Delay(2000); // So that we will see the loading (The thread is basically taken).
                        
                        showMessage(MainActivity.player.Name + " Was Added");
                        StartActivity(typeof(Registered));
                    }
                    Finish();   // Anyways, finish the activity.
                }catch (Exception){
                    showMessage("Unknown error has occured while trying to create the user.");
                }
                
            };
        }

        /// <summary>
        ///     Check if at least one textbox is empty.
        ///     true if at least one is empty.
        ///     false if all contain details.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private bool checkEmptyBoxes(EditText name, EditText email, EditText password)
        {
            if (name.Text == "" || email.Text == "" || password.Text == "")
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Please enter all details!");
                callDialog.Show();
                return true;
            }
            return false;
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
        ///     If it's correct, he's signed up.
        ///     Else, Error occurs.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task getUser(EditText name,EditText email,EditText password)
        {
            try{
                await checkIfEmailRegistered(email.Text);
                
                // Assign the current user to the static player member.
                MainActivity.player.Name = name.Text;
                MainActivity.player.Email = email.Text;
                MainActivity.player.Password = password.Text;

            }catch (FormatException){
                showMessage("Undefined email address!");
                problemOccured = true;
            }catch (Exception){
                showMessage("Check your internet connection!");
                problemOccured = true;
            }
        }
        /// <summary>
        ///     Gets a string, and shows a basic toast message.
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
            // Define the parse object.
            var user = new ParseObject("Users")
            {
                { "Name", MainActivity.player.Name },
                { "Email", MainActivity.player.Email},
                { "Password",MainActivity.player.Password } ,
            };
            user["Score"] = 0;
            // Save the current object in parse.
            await user.SaveAsync();
        }

        /// <summary>
        ///     Check in the database if the email already exists.
        ///     If it does - alreadyRegistered variable is set to true.
        ///     Else       - alreadyRegistered = false.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task checkIfEmailRegistered(string email)
        {
            try{
                // A query to check if the email is registered.
                var query = from Users in ParseObject.GetQuery("Users")
                            where Users.Get<string>("Email") == email
                            select Users;
                // Activate query on parse data base.
                IEnumerable<ParseObject> results = await query.FindAsync();

                // Assign true/false if found any previous registrated emails or not.
                if (results.Count() == 0){
                    alreadyRegistered = false;

                }else{
                    alreadyRegistered = true;
                }
            }catch (FormatException){
                throw new FormatException();
            }catch (Exception){
                throw new Exception();
            }
            
        }

    }
}