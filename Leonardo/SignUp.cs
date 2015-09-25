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
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;
using Parse;

namespace Leonardo
{
    [Activity(Label = "Leonardo")]
    public class SignUp : Activity
    {

        bool _alreadyRegistered;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.SignUp);

                // Assign the button.
                Button submitBtn = FindViewById<Button>(Resource.Id.buttonSubmit);
                sumbitClick(submitBtn);
        
            }catch(Exception e){
                throw new Exception("Error : In SignUp.\n" + e.Message);
            }
        }

        /// <summary>
        ///     Submit button clicked, checks all details.
        ///     Registers if validated correctly
        /// </summary>
        /// <param name="submitBtn"></param>
        private async void sumbitClick(Button submitBtn)
        {
            submitBtn.Click += async (sender, e) =>
            {
                try{
                    //User user = 
                    await getUser();
                    // Something went wrong.
                    if (MainActivity.player.Instantiated == false){
                        StartActivity(typeof(MainActivity));
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Wrong Email Address!\nOr you already registered!");
                        callDialog.SetNeutralButton("Woops", delegate { });
                        callDialog.Show();
                    }else{ // User added.

                        //delPassUser del = new delPassUser(Leonardo.MainActivity.passUsername);
                        //del(user);
                        await addUserToParse();
                        //Toast.MakeText(this, User.CurrentUser.Name + " Was Added", ToastLength.Short).Show();
                        StartActivity(typeof(MainActivity));
                    }
                }catch (Exception ex){
                    throw new Exception("Error : Creation of a user.\n" + ex.Message);
                }
                
            };
        }
        /// <summary>
        ///     Receives the user data, 
        ///     If it's correct, he's signed in.
        ///     Else, Error dialog box.
        /// </summary>
        /// <returns></returns>
        private async Task<User> getUser()
        {
            try
            {
                var name = FindViewById<EditText>(Resource.Id.editText1);
                var email = FindViewById<EditText>(Resource.Id.editText2);
                var password = FindViewById<EditText>(Resource.Id.editText3);

                await alreadyRegistered(email.Text);
                if (_alreadyRegistered)
                {
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("User Already Exist!");
                    callDialog.SetNeutralButton("oops", delegate { });
                    callDialog.Show();
                    return null;
                }

                if (name.Text == "" || email.Text == "" || password.Text == "")
                {
                    return null;
                }

               // User.Create(name.Text, email.Text, password.Text);
                MainActivity.player.Name = name.Text;
                MainActivity.player.Email = email.Text;
                MainActivity.player.Password = password.Text;
                return null;
            }
            catch (FormatException)
            {
                return null;
            }
            catch (Exception e)
            {
                throw new Exception("Error : Creation of a user.\n" + e.Message);
            }
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
            await user.SaveAsync();//SaveAsync
        }

        /// <summary>
        ///     Should check in the database if the email already exists.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private async Task alreadyRegistered(string email)
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
        }


        public string Email { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }
    }
}