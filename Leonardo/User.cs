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
                if(IsValid(value)){
                    email = value;
                }else{
                    email = null;
                }
            }
        }

        /// <summary>
        ///     C'tor.
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_email"></param>
        /// <param name="pass"></param>
        public User(string _name, string _email, string pass){
            try{
                if(IsValid(_email) == false){
                    throw new FormatException();
                }
                Name = _name;
                email = _email;
                Password = pass;
            }catch(FormatException){
                throw new FormatException();
            }

        }

        public User(User newUser)
        {
            Score = newUser.Score;
            email = newUser.email;
            Password = newUser.Password;
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
                return false;
            }
        }
    }
}