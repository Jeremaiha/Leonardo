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


        // An array of delegates to hold all cards initialization.
        const int NUM_CARDS = 3;
        public delegate void MethodsDelegates(ImageButton btnImg,int index);
        public MethodsDelegates[] cardsMethods = new MethodsDelegates[NUM_CARDS];

        //List<Action<ImageButton, int>> methods = new List<Action<ImageButton, int>>();

        int globalIndex;

        private void initiateAll(){
            initiateButtonsArray();
            initiateSetOfImages();
            //Game g = new Game();
            //Add all methods to the delegates array.
          //  g.cardsMethods[0] += new Game.MethodsDelegates(greenCherry2);
         
            // SAVE DELEGATE
            initiateDelegates();
            //cardsMethods[0] += new Game.MethodsDelegates(greenCherry2);
        }

        /// <summary>
        ///     In this method we will instantiate the array of delegates.
        ///     The sake of this function is to create an array of delegates which
        ///     will help us later on choosing the currect method for a specific card.
        ///     Specified order : 
        ///         * Green
        ///         * Red
        ///         * Blue
        ///         * Yellow.
        /// </summary>
        private void initiateDelegates() { 
            // Green
            cardsMethods[0] += new Game.MethodsDelegates(greenCherry2);
            cardsMethods[1] += new Game.MethodsDelegates(greenCherry3);

            // Red
            cardsMethods[2] += new Game.MethodsDelegates(redMushroom1);

            // Blue

            // Yellow

        }

        /// <summary>
        ///     Create an array of all images.
        /// </summary>
        private void initiateSetOfImages(){
            gameImages = new Card[3];

        
            gameImages[0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton17), "Mushroom", "Red", 1);
            gameImages[0].ImageButton.SetImageResource(Resource.Drawable.red_mushroom_1);

            gameImages[1] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton17), "Cherry", "Green", 3);
            gameImages[1].ImageButton.SetImageResource(Resource.Drawable.green_cherry_3);

            gameImages[2] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton17), "Cherry", "Green", 2);
            gameImages[2].ImageButton.SetImageResource(Resource.Drawable.green_cherry_2);

            

        }

        /// <summary>
        ///     Initiating the 2 dimentional array of buttons with a blank image.
        /// </summary>
        private void initiateButtonsArray(){
            buttonsArray = new Card[SIZE, SIZE];
        
            /*
            int cnt = 1;
            bool flag = false;
            for (int i = 0; i < SIZE; i++){
                for (int j = 0; j < SIZE; j++){
                    ImageButton btnImage = new ImageButton();
                    int btn = Resources.GetIdentifier("imageButton" + cnt.ToString(), "drawable", this.PackageName);
                    btnImage.SetImageResource(btn);
                    buttonsArray[i, j] = new Card(btnImage, "blank", "white", 0);
                }
            }
            */
               
            buttonsArray[0, 0] = new Card(FindViewById<ImageButton>(Resource.Id.imageButton1), "blank", "white", 0);
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

        private void redMushroom1(ImageButton btn,int index)
        {       
            btn.SetImageResource(Resources.GetIdentifier("imageButton" + index.ToString(), "drawable", this.PackageName));
            btn.SetImageResource(Resource.Drawable.red_mushroom_1);

        }

        private void greenCherry3(ImageButton btn,int index)
        {            
            btn.SetImageResource(Resources.GetIdentifier("imageButton" + index.ToString(), "drawable", this.PackageName));
            btn.SetImageResource(Resource.Drawable.green_cherry_3);
           
        }

        private void greenCherry2(ImageButton btn,int index)
        {
            btn.SetImageResource(Resources.GetIdentifier("imageButton" + index.ToString(), "drawable", this.PackageName));
            btn.SetImageResource(Resource.Drawable.green_cherry_2);

        }

        private void randomNextCard(){
            
            Random random = new Random();
            int randomNumber = random.Next(0, 3);
            globalIndex = randomNumber;


            cardsMethods[randomNumber](gameImages[randomNumber].ImageButton, 17);

        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            // Layout initialisation
            SetContentView(Resource.Layout.Game);

            //  Call the initiation of all buttons method.
            initiateAll();
            randomNextCard();

            ImageButton tempButton = FindViewById<ImageButton>(Resource.Id.imageButton14);

            buttonsArray[3,1].ImageButton.Click += (sender, e) => {
                cardsMethods[globalIndex](buttonsArray[3,1].ImageButton,14);
                //buttonsArray[3, 1].ImageButton.SetImageResource();
                //tempButton.SetImageResource(Resource.Drawable.green_cherry_3);
            };
            
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
        ///     Empty constructor.
        /// </summary>
        public Card(){
            imageButton = null;
            shape = "";
            color = "";
            amount = 0;
        }

        /// <summary>
        ///     Full constructor.
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