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

        private User getUser(){
            try{
                var name = FindViewById<EditText>(Resource.Id.editText1);
                var email = FindViewById<EditText>(Resource.Id.editText2);
                var password = FindViewById<EditText>(Resource.Id.editText3);

                return new User(name.Text, email.Text, password.Text);        
            }
            catch (FormatException){
                return null;
            }catch (Exception e){
                throw new Exception("Error : Creation of a user.\n" + e.Message);
            }
        }

    }

}

