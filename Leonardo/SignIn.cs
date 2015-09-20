using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Leonardo
{
    [Activity(Label = "Leonardo")]
    public class SignIn : Activity
    {
        public delegate void delegatePassUser(User dlgUser);

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.SignIn);

            Button signinBtn = FindViewById<Button>(Resource.Id.buttonSignIn);
            signInButton(signinBtn);
        }

        /// <summary>
        ///     Submit button clicked, checks all details.
        ///     Registers if validated correctly
        /// </summary>
        /// <param name="submitBtn"></param>
        private void signInButton(Button signinBtn)
        {
            signinBtn.Click += (sender, e) =>
            {
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

                return getUserFromParse(email.Text,password.Text);
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
            return new User("TEMP",email,pass);
        }
    }
}