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
        Card  outerImage;   // Image Button 17
        Card[,] buttonsArray;
        
        // An array of delegates to hold all cards initialization.
        const int NUM_CARDS =  3 * 4 * 3;//Amount of all cards(3 shapes, 4 amounts of all shape and 3 colors)
        public delegate void MethodsDelegates(ImageButton btnImg);
        public MethodsDelegates[] cardsMethods = new MethodsDelegates[NUM_CARDS];

        Card[] gameCards;// Array of all possible cards
        int[] numOfCards;//Amount of each card in the game
        int numberOfCards = NUM_CARDS; //Substact each time when Random function is activated
        int globalIndex;//To know which delegate is active in current time

        private void initiateAll(){
            initiateButtonsArray();
            initiateSetOfImages();
         
            // SAVE DELEGATE
            initiateDelegates();
            //Will be re

        }

        /// <summary>
        ///     In this method we will instantiate the array of delegates.
        ///     The sake of this function is to create an array of delegates which
        ///     will help us later on choosing the currect method for a specific card.
        ///     Specified order : 
        ///         * Green
        ///         * Red
        ///         * Yellow.
        /// </summary>
        private void initiateDelegates() { 
            // Green
            cardsMethods[0] += new Game.MethodsDelegates(greenCherry1);
            cardsMethods[1] += new Game.MethodsDelegates(greenCherry2);
            cardsMethods[2] += new Game.MethodsDelegates(greenCherry3);
            cardsMethods[3] += new Game.MethodsDelegates(greenCherry4);

            cardsMethods[4] += new Game.MethodsDelegates(greenMushroom1);
            cardsMethods[5] += new Game.MethodsDelegates(greenMushroom2);
            cardsMethods[6] += new Game.MethodsDelegates(greenMushroom3);
            cardsMethods[7] += new Game.MethodsDelegates(greenMushroom4);

            cardsMethods[8] += new Game.MethodsDelegates(greenStrawberry1);
            cardsMethods[9] += new Game.MethodsDelegates(greenStrawberry2);
            cardsMethods[10] += new Game.MethodsDelegates(greenStrawberry3);
            cardsMethods[11] += new Game.MethodsDelegates(greenStrawberry4);

            // Red
            cardsMethods[12] += new Game.MethodsDelegates(redCherry1);
            cardsMethods[13] += new Game.MethodsDelegates(redCherry2);
            cardsMethods[14] += new Game.MethodsDelegates(redCherry3);
            cardsMethods[15] += new Game.MethodsDelegates(redCherry4);

            cardsMethods[16] += new Game.MethodsDelegates(redMushroom1);
            cardsMethods[17] += new Game.MethodsDelegates(redMushroom2);
            cardsMethods[18] += new Game.MethodsDelegates(redMushroom3);
            cardsMethods[19] += new Game.MethodsDelegates(redMushroom4);

            cardsMethods[20] += new Game.MethodsDelegates(redStrawberry1);
            cardsMethods[21] += new Game.MethodsDelegates(redStrawberry2);
            cardsMethods[22] += new Game.MethodsDelegates(redStrawberry3);
            cardsMethods[23] += new Game.MethodsDelegates(redStrawberry4);

            // Yellow
            cardsMethods[24] += new Game.MethodsDelegates(yellowCherry1);
            cardsMethods[25] += new Game.MethodsDelegates(yellowCherry2);
            cardsMethods[26] += new Game.MethodsDelegates(yellowCherry3);
            cardsMethods[27] += new Game.MethodsDelegates(yellowCherry4);

            cardsMethods[28] += new Game.MethodsDelegates(yellowMushroom1);
            cardsMethods[29] += new Game.MethodsDelegates(yellowMushroom2);
            cardsMethods[30] += new Game.MethodsDelegates(yellowMushroom3);
            cardsMethods[31] += new Game.MethodsDelegates(yellowMushroom4);

            cardsMethods[32] += new Game.MethodsDelegates(yellowStrawberry1);
            cardsMethods[33] += new Game.MethodsDelegates(yellowStrawberry2);
            cardsMethods[34] += new Game.MethodsDelegates(yellowStrawberry3);
            cardsMethods[35] += new Game.MethodsDelegates(yellowStrawberry4);

        }

        /// <summary>
        ///     Create an array of all images.
        ///     A help array will exist, which will be holding the counting of each card in.
        /// </summary>
        private void initiateSetOfImages(){
            int localCnt = 0;

            // Initialize all game cards.
            gameCards =  new Card[NUM_CARDS];
            numOfCards = new int [NUM_CARDS];

            // Initialize amount.
            for (int i = 0; i < NUM_CARDS; i++ ){
                numOfCards[i] = 4;
            }

            // Initialize the cards symbols,color and amount.
            // Order : Green , Red , Yellow.
            // Type  : Cherry, Mushroom , Strawberry.

            // Green
            for (int i = 0; i < 4; i++,localCnt++){
                gameCards[localCnt] = new Card(null,"cherry","green",i+1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "mushroom", "green", i+1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "stawberry", "green", i + 1);
            }

            // Red
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "cherry", "red", i + 1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "mushroom", "red", i + 1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "stawberry", "red", i + 1);
            }

            // Yellow
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "cherry", "yellow", i + 1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "mushroom", "yellow", i + 1);
            }
            for (int i = 0; i < 4; i++, localCnt++){
                gameCards[localCnt] = new Card(null, "stawberry", "yellow", i + 1);
            }
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

            outerImage = new Card(FindViewById<ImageButton>(Resource.Id.imageButton17), "blank", "white", 0);

            for(int i=0;i<SIZE;i++){
                for(int j=0;j<SIZE;j++){
                    buttonsArray[i, j].ImageButton.SetImageResource(Resource.Drawable.blank);        
                }
            }

    }

        private void randomNextCard(){
            
            Random random = new Random();
            int randomNumber;
            if(numberOfCards != 0){
                numberOfCards--;
            }else{
                // Game ended, show scores.
                return;
            }
               
            // Untill I get a card which I can use, keep doing random.
            do{
                randomNumber = random.Next(0, NUM_CARDS);
            }while(numOfCards[randomNumber] == 0);
            numOfCards[randomNumber] -= 1; // Decrease 1, used this card at this round.
            globalIndex = randomNumber;
            // Call the delegate method.
            cardsMethods[randomNumber](outerImage.ImageButton);
            // Set the rest values of the 17'th button.
            outerImage.Shape  = gameCards[randomNumber].Shape;
            outerImage.Amount = gameCards[randomNumber].Amount;
            outerImage.Color  = gameCards[randomNumber].Color;
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Create your application here
            // Layout initialisation
            SetContentView(Resource.Layout.Game);

            //  Call the initiation of all buttons method.
            initiateAll();
            
            simulate();       
        }

        private void simulate()
        {
            randomNextCard();
            buttonsClicks();
        }
        /// <summary>
        ///     When a button is clicked, then an action listenr appears.
        ///     This method is responsible for each button.
        /// </summary>
        private void buttonsClicks()
        {
            /*
            for (int i = 0; i < SIZE; i++){
                for (int j = 0; j < SIZE; j++){
                    buttonsArray[i, j].ImageButton.Click += (sender, e) =>
                    {
                        cardsMethods[globalIndex](buttonsArray[i, j].ImageButton);
                        randomNextCard();
                        buttonsArray[i, j].ImageButton.Enabled = false;
                    };
                }
            }
            */
            buttonsArray[0, 0].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[0, 0].ImageButton);
                randomNextCard();
                buttonsArray[0, 0].ImageButton.Enabled = false;
            };
            buttonsArray[0, 1].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[0, 1].ImageButton);
                randomNextCard();
                buttonsArray[0, 1].ImageButton.Enabled = false;
            };
            buttonsArray[0, 2].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[0, 2].ImageButton);
                randomNextCard();
                buttonsArray[0, 2].ImageButton.Enabled = false;
            };
            buttonsArray[0, 3].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[0, 3].ImageButton);
                randomNextCard();
                buttonsArray[0, 3].ImageButton.Enabled = false;
            };
            buttonsArray[1, 0].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[1, 0].ImageButton);
                randomNextCard();
                buttonsArray[1, 0].ImageButton.Enabled = false;
            };
            buttonsArray[1, 1].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[1, 1].ImageButton);
                randomNextCard();
                buttonsArray[1, 1].ImageButton.Enabled = false;
            };
            buttonsArray[1, 2].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[1, 2].ImageButton);
                randomNextCard();
                buttonsArray[1, 2].ImageButton.Enabled = false;
            };
            buttonsArray[1, 3].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[1, 3].ImageButton);
                randomNextCard();
                buttonsArray[1, 3].ImageButton.Enabled = false;
            };
            buttonsArray[2, 0].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[2, 0].ImageButton);
                randomNextCard();
                buttonsArray[2, 0].ImageButton.Enabled = false;
            };
            buttonsArray[2, 1].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[2, 1].ImageButton);
                randomNextCard();
                buttonsArray[2, 1].ImageButton.Enabled = false;
            };
            buttonsArray[2, 2].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[2, 2].ImageButton);
                randomNextCard();
                buttonsArray[2, 2].ImageButton.Enabled = false;
            };
            buttonsArray[2, 3].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[2, 3].ImageButton);
                randomNextCard();
                buttonsArray[2, 3].ImageButton.Enabled = false;
            };
            buttonsArray[3, 0].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[3, 0].ImageButton);
                randomNextCard();
                buttonsArray[3, 0].ImageButton.Enabled = false;
            };
            buttonsArray[3, 1].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[3, 1].ImageButton);
                randomNextCard();
                buttonsArray[3, 1].ImageButton.Enabled = false;
            };
            buttonsArray[3, 2].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[3, 2].ImageButton);
                randomNextCard();
                buttonsArray[3, 2].ImageButton.Enabled = false;
            };
            buttonsArray[3, 3].ImageButton.Click += (sender, e) =>
            {
                cardsMethods[globalIndex](buttonsArray[3, 3].ImageButton);
                randomNextCard();
                buttonsArray[3, 3].ImageButton.Enabled = false;
            };
        }

        // Beginning of all type methods.
        // Green methods : 
        private void greenCherry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_cherry_1); }
        private void greenCherry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_cherry_2); }
        private void greenCherry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_cherry_3); }
        private void greenCherry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_cherry_4); }

        private void greenMushroom1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_mushroom_1); }
        private void greenMushroom2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_mushroom_2); }
        private void greenMushroom3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_mushroom_3); }
        private void greenMushroom4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_mushroom_4); }

        private void greenStrawberry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_straw_1); }
        private void greenStrawberry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_straw_2); }
        private void greenStrawberry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_straw_3); }
        private void greenStrawberry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.green_straw_4); }

        // Red methods : 
        private void redCherry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_cherry_1); }
        private void redCherry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_cherry_2); }
        private void redCherry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_cherry_3); }
        private void redCherry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_cherry_4); }

        private void redMushroom1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_mushroom_1); }
        private void redMushroom2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_mushroom_2); }
        private void redMushroom3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_mushroom_3); }
        private void redMushroom4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_mushroom_4); }

        private void redStrawberry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_straw_1); }
        private void redStrawberry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_straw_2); }
        private void redStrawberry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_straw_3); }
        private void redStrawberry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.red_straw_4); }

        // Yellow methods : 
        private void yellowCherry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_cherry_1); }
        private void yellowCherry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_cherry_2); }
        private void yellowCherry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_cherry_3); }
        private void yellowCherry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_cherry_4); }

        private void yellowMushroom1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_mushroom_1); }
        private void yellowMushroom2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_mushroom_2); }
        private void yellowMushroom3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_mushroom_3); }
        private void yellowMushroom4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_mushroom_4); }

        private void yellowStrawberry1(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_straw_1); }
        private void yellowStrawberry2(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_straw_2); }
        private void yellowStrawberry3(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_straw_3); }
        private void yellowStrawberry4(ImageButton btn)
        { btn.SetImageResource(Resource.Drawable.yellow_straw_4); }

        // End of delegates methods, all possible game cards.


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