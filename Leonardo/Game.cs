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
        Card outerImageButton;
        Card[,] buttonsArray;
        Card[] gameImages;

        private void initiateAll(){
            initiateButtonsArray();
            initiateSetOfImages();
        }

        /// <summary>
        ///     Create an array of all images.
        /// </summary>
        private void initiateSetOfImages(){
            gameImages = new Card[SIZE];

           
        }

        /// <summary>
        ///     Initiating the 2 dimentional array of buttons with a blank image.
        /// </summary>
        private void initiateButtonsArray(){
            buttonsArray = new Card[SIZE, SIZE];
            
            buttonsArray[0, 0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton1),"blank","white",0);
            buttonsArray[0, 1] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton2), "blank", "white", 0);
            buttonsArray[0, 2] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton3), "blank", "white", 0);
            buttonsArray[0, 3] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton4), "blank", "white", 0);

            buttonsArray[1, 0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton5), "blank", "white", 0);
            buttonsArray[1, 1] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton6), "blank", "white", 0);
            buttonsArray[1, 2] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton7), "blank", "white", 0);
            buttonsArray[1, 3] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton8), "blank", "white", 0);

            buttonsArray[2, 0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton9), "blank", "white", 0);
            buttonsArray[2, 1] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton10), "blank", "white", 0);
            buttonsArray[2, 2] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton11), "blank", "white", 0);
            buttonsArray[2, 3] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton12), "blank", "white", 0);

            buttonsArray[3, 0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton13), "blank", "white", 0);
            buttonsArray[3, 1] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton14), "blank", "white", 0);
            buttonsArray[3, 2] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton15), "blank", "white", 0);
            buttonsArray[3, 3] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton16), "blank", "white", 0);
        
            for(int i=0;i<SIZE;i++){
                for(int j=0;j<SIZE;j++){
                    buttonsArray[i, j].ImageButton.SetImageResource(Resource.Drawable.blank);        
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
    /// <summary>
    ///     Card class which is a card container.
    ///     Contains the imageButton,Color,Shape and Amount.
    /// </summary>
    public class Card
    {
        // Define fields.
        ImageButton imageButton;
        string shape;
        string color;
        int amount;

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="ib">ImageButton </param>
        /// <param name="shp">Shape</param>
        /// <param name="clr">Color</param>
        /// <param name="amnt">Amount</param>
        public Card(ImageButton ib,string shp, string clr, int amnt){
            imageButton = ib;
            shape = shp;
            color = clr;
            amount = amnt;
        }

        // Define properties.
        public ImageButton ImageButton{
            get { return imageButton; }
            set { imageButton = value;}
        }
        public string Shape{
            get{ return shape; }
            set{ shape = value;}
        }
        public string Color{
            get{ return color; }
            set{ color = value;}
        }
        public int Amount{
            get{ return amount; }
            set{ amount = value;}
        }


    }
}