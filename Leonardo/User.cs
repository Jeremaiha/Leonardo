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
using System.Net.Mail;
using Parse;
namespace Leonardo
{
    public class User 
    {
        // All fields for a user.
        public int Score { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        private string email; 
        public string Email
        {
            get { return email; }
            set
            {
                try
                {
                    if(value == null){
                        email = null;
                        return;
                    }
                    if (IsValid(value)){
                        email = value;
                    }
                    else{
                        email = null;
                    }
                }catch(FormatException){
                    throw new FormatException();
                }
                
            }
        }
        private bool instantiated = false;
        public bool Instantiated
        {
            get { return instantiated; }
            set { instantiated = value; }
        }

        public ParseObject ParseCurrentUser { get; set; }

        // Singelton
        private static User currentUser;

        public static User CurrentUser
        {
            get
            {
                if(currentUser==null){
                    currentUser = new User(null, null, null);
                }
                return currentUser;
            }
        }

     
        /// <summary>
        ///     Singelton C'tor.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_email"></param>
        /// <param name="pass"></param>
        private User(string _name, string _email, string pass){
            try{
                if((instantiated==true) && (IsValid(_email) == false) ){
                    throw new FormatException();
                }
                Name = _name;
                email = _email;
                Password = pass;
                instantiated = true;
            }catch(FormatException){
                throw new FormatException();
            }

        }

        /// <summary>
        ///     Method to check the email.
        /// </summary>
        /// <param name="emailAddr"></param>
        /// <returns></returns>
        private bool IsValid(string emailAddr){
            try{
                MailAddress m = new MailAddress(emailAddr);
                return true;
            }
            catch (FormatException){
                throw new FormatException();
            }
        }
    }
}