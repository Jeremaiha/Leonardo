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
using System.Threading.Tasks;
using Parse;

namespace Leonardo
{
    [Application(Name = "leonardo.ParseApplication")]
    class ParseApplication : Application
    {
        public ParseApplication(IntPtr handle, JniHandleOwnership ownerShip): base(handle, ownerShip){}

        /// <summary>
        ///     Responsible for the Parse database initialization.
        /// </summary>
        public override void OnCreate()
        {
            try{
                base.OnCreate();

                ParseClient.Initialize("bOXeIXCGqp6eAgKyrMqwohohDbJeqf0vWhIqc9Cz",
                               "NIs2OLWSOtQRfXfWOnz8xN8E9QOEG5mJQTFKXRwv");
                ParsePush.ParsePushNotificationReceived += ParsePush.DefaultParsePushNotificationReceivedHandler;

            }catch (Exception){
                showMessage("Error connecting to Parse.");
            }

        }
        /// <summary>
        ///     Gets a string and shows a basic toast message.
        /// </summary>
        /// <param name="s"></param>
        public void showMessage(string s)
        {
            Toast.MakeText(this, s, ToastLength.Long).Show();
        }
        

    }
}