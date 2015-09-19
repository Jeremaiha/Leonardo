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
    public class SignUp : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            SetContentView(Resource.Layout.SignUp);
            Button submitBtn = FindViewById<Button>(Resource.Id.buttonSubmit);
            sumbitClick(submitBtn);
        }

        /// <summary>
        ///     Submit button clicked, checks all details.
        ///     Registers if validated correctly
        /// </summary>
        /// <param name="submitBtn"></param>
        private void sumbitClick(Button submitBtn){
            submitBtn.Click += (sender, e) =>{
                User user = getUser();
                if (user == null){
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Wrong Email Address!");
                    callDialog.SetNeutralButton("Woops", delegate{});
                    callDialog.Show();
                }else{
                    Toast.MakeText(this,user.Name+" Was Added",ToastLength.Short).Show();
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
        private User getUser(){
            try{
                var name = FindViewById<EditText>(Resource.Id.editText1);
                var email = FindViewById<EditText>(Resource.Id.editText2);
                var password = FindViewById<EditText>(Resource.Id.editText3);

                if (alreadyRegistered(email.Text)){
                    
                }

                return new User(name.Text, email.Text, password.Text);        
            }
            catch (FormatException){
                return null;
            }catch (Exception e){
                throw new Exception("Error : Creation of a user.\n" + e.Message);
            }
        }

        /// <summary>
        ///     Should check in the database if the email already exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool alreadyRegistered(string email){
            return false;
        }

    }

}

