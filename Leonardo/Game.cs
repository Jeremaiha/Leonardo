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
        HashSet<Resource.Drawable> gameImages;

        private void initiateAll(){
            initiateButtonsArray();
            initiateSetOfImages();
        }

        private void initiateSetOfImages(){
            gameImages = new HashSet<Resource.Drawable>();
           // var temp = Resource.Drawable;

          //  gameImages.Add(temp);
        }

        /// <summary>
        ///     Initiating the 2 dimentional array of buttons with a blank image.
        /// </summary>
        private void initiateButtonsArray(){
            buttonsArray = new ImageButton[SIZE, SIZE];
            
            buttonsArray[0, 0] = FindViewById<ImageButton>(Resource.Id.imageButton1);
            buttonsArray[0, 1] = FindViewById<ImageButton>(Resource.Id.imageButton2);
            buttonsArray[0, 2] = FindViewById<ImageButton>(Resource.Id.imageButton3);
            buttonsArray[0, 3] = FindViewById<ImageButton>(Resource.Id.imageButton4);

            buttonsArray[1, 0] = FindViewById<ImageButton>(Resource.Id.imageButton5);
            buttonsArray[1, 1] = FindViewById<ImageButton>(Resource.Id.imageButton6);
            buttonsArray[1, 2] = FindViewById<ImageButton>(Resource.Id.imageButton7);
            buttonsArray[1, 3] = FindViewById<ImageButton>(Resource.Id.imageButton8);

            buttonsArray[2, 0] = FindViewById<ImageButton>(Resource.Id.imageButton9);
            buttonsArray[2, 1] = FindViewById<ImageButton>(Resource.Id.imageButton10);
            buttonsArray[2, 2] = FindViewById<ImageButton>(Resource.Id.imageButton11);
            buttonsArray[2, 3] = FindViewById<ImageButton>(Resource.Id.imageButton12);

            buttonsArray[3, 0] = FindViewById<ImageButton>(Resource.Id.imageButton13);
            buttonsArray[3, 1] = FindViewById<ImageButton>(Resource.Id.imageButton14);
            buttonsArray[3, 2] = FindViewById<ImageButton>(Resource.Id.imageButton15);
            buttonsArray[3, 3] = FindViewById<ImageButton>(Resource.Id.imageButton16);
        
            for(int i=0;i<SIZE;i++){
                for(int j=0;j<SIZE;j++){
                    buttonsArray[i, j].SetImageResource(Resource.Drawable.blank);        
                }
            }
    }

        private void randomNextCard(){

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            // Layout initialisation
            SetContentView(Resource.Layout.Game);

            //  Call the initiation of all buttons method.
            initiateAll();

//            outerImageButton = FindViewById<ImageButton>(Resource.Id.imageButton17);
  //          outerImageButton.SetImageResource(Resource.Drawable.red_mushroom_1);
            
            //tempButton = FindViewById<ImageButton>(Resource.Id.imageButton14);

/*            tempButton.Click += (sender, e) => {
                tempButton.SetImageResource(Resource.Drawable.green_cherry_3);
            };
            */
        }



    }


}