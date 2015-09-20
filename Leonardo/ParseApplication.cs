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

using Parse;

namespace Leonardo
{
    [Application(Name = "leonardo.ParseApplication")]
    class ParseApplication : Application
    {
        public ParseApplication(IntPtr handle, JniHandleOwnership ownerShip): base(handle, ownerShip)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
            /*
            ParseClient.Initialize("YOUR APPLICATION ID", "YOUR .NET KEY");
            ParsePush.ParsePushNotificationReceived += ParsePush.DefaultParsePushNotificationReceivedHandler;
        */
            ParseClient.Initialize("bOXeIXCGqp6eAgKyrMqwohohDbJeqf0vWhIqc9Cz",
                           "NIs2OLWSOtQRfXfWOnz8xN8E9QOEG5mJQTFKXRwv");
            saveObject();
            ParsePush.ParsePushNotificationReceived += ParsePush.DefaultParsePushNotificationReceivedHandler;
            
        }
        public async void saveObject(){
            var testObject = new ParseObject("Users");
            testObject["Name"] = "Jeremy";
            testObject["Email"] = "something@gmail.com";
            testObject["Password"] = "123";
            await testObject.SaveAsync();
        }

    }
}