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
    [Activity(Label = "Game")]
    public class Game : Activity
    {
        const int SIZE = 4;
        ImageButton outerImageButton;
        ImageButton[,] buttonsArray;


        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            // Layout initialisation
            SetContentView(Resource.Layout.Game);

            outerImageButton = FindViewById<ImageButton>(Resource.Id.imageButton17);
            outerImageButton.SetImageResource(Resource.Drawable.red_mushroom_1);
            
            //tempButton = FindViewById<ImageButton>(Resource.Id.imageButton14);

/*            tempButton.Click += (sender, e) => {
                tempButton.SetImageResource(Resource.Drawable.green_cherry_3);
            };
            */
        }



    }
}