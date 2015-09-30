using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Content.PM;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Media;
using Parse;
using System.Threading.Tasks;

namespace Leonardo
{

    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::Game")]
    public class Game : Activity
    {
        const int SIZE = 4;
        /*Sound*/
        SoundPool sp,sp2,sp3;
        int _soundPushButton;
        int _singleRowOrColumn;
        int _gameOver;
        /*Sound*/
        Card  outerImage;   // Image Button 17
        Card[,] buttonsArray;
        
        // An array of delegates to hold all cards initialization.
        const int NUM_CARDS =  (3 * 4 * 3);//Amount of all cards(3 shapes, 4 cards and 3 colors)
        const int NUM_PACKETS = 2;

        Card[] gameCards;// Array of all possible cards
        int[] numOfCards;//Amount of each card in the game
        int numberOfCards = NUM_CARDS * NUM_PACKETS; // Substract each time when Random function is activated
        int globalIndex;// To know which delegate is active in current time

        GameRules gameRules; // Holds the game board, and all its rules.
        TextView score,cardsLeft;
       

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            /*Sound*/
            Stream st = new Stream();
            sp = new SoundPool(1, st, 100);
            sp2 = new SoundPool(1, st, 100);
            sp3 = new SoundPool(1,st,100);
            _soundPushButton = sp.Load(this, Resource.Raw.buttonDown, 1);
            _singleRowOrColumn = sp2.Load(this, Resource.Raw.singleRowOrColumn, 1);
            _gameOver = sp3.Load(this, Resource.Raw.gameOver, 1);
            /*Sound*/
            // Create your application here
            // Layout initialisation
            SetContentView(Resource.Layout.Game);
            //  Call the initiation of all buttons method.
            initiateAll();

            simulate();
        }

        /// <summary>
        ///     Responsible to call all initiation methods.
        /// </summary>
        private void initiateAll(){
            try{
                initiateButtonsArray();
                initiateSetOfImages();
         

                gameRules = new GameRules(buttonsArray,SIZE);
                score = FindViewById<TextView>(Resource.Id.scoreTextView);
                cardsLeft = FindViewById<TextView>(Resource.Id.cardsLeftTextView);
            }catch (Exception e){
                throw new Exception("Error : Initiating everything.\n" + e.Message);
            }
          
        }

        /// <summary>
        ///     Create an array of all images.
        ///     A help array will exist, which will be holding the counting of each card in.
        /// </summary>
        private void initiateSetOfImages(){
            try{
                int localCnt = 0;

                // Initialize all game cards.
                gameCards = new Card[NUM_CARDS];
                numOfCards = new int[NUM_CARDS];

                // Initialize amount  -  Responsible for amount of cards per type.
                for (int i = 0; i < NUM_CARDS; i++)
                {
                    numOfCards[i] = 4 * NUM_PACKETS;
                }

                // Initialize the cards symbols,color and amount.
                // Order : Green , Red , Yellow.
                // Type  : Cherry, Mushroom , Strawberry.

                // Green
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "cherry", "green", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "mushroom", "green", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "straw", "green", i + 1);
                }

                // Red
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "cherry", "red", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "mushroom", "red", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "straw", "red", i + 1);
                }

                // Yellow
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "cherry", "yellow", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "mushroom", "yellow", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card(null, "straw", "yellow", i + 1);
                }
            }
            catch (Exception e){
                throw new Exception("Error : Initiating images.\n" + e.Message);
            }
            
        }

        /// <summary>
        ///     Initiating the 2 dimentional array of buttons with a blank image.
        /// </summary>
        private void initiateButtonsArray(){
            try{

                buttonsArray = new Card[SIZE, SIZE];
                for (int i = 0; i < SIZE; i++){
                    for (int j = 0; j < SIZE; j++){
                        ImageButton btnImage = new ImageButton(this);
                        var s = "imageButton" + (i * SIZE + j +1);
                        int rid = Resources.GetIdentifier(s, "id", this.PackageName);
                        buttonsArray[i, j] = new Card(FindViewById<ImageButton>(rid), "blank", "white", 0);
            
                    }
                }
            
                outerImage = new Card(FindViewById<ImageButton>(Resource.Id.imageButton17), "blank", "white", 0);
                
                for (int i = 0; i < SIZE; i++){
                    for (int j = 0; j < SIZE; j++){
                        buttonsArray[i, j].ImageButton.SetImageResource(Resource.Drawable.blank);
                    }
                }

            }
            catch (Exception e){
                throw new Exception("Error : Initiating all buttons.\n" + e.Message);
            }
            
    }

        /// <summary>
        ///     Is responsible for giving us the next random card from the possible cards.
        /// </summary>
        private async void randomNextCard(){
            try{
                Random random = new Random();
                int randomNumber;
                if (numberOfCards != -1){ // last card to be calculated also.
                    cardsLeft.Text = numberOfCards.ToString(); 
                    numberOfCards--;
                }else{
                    // Game ended, show scores.
                    await gameOver();
                    return;
                }

                // Untill I get a card which I can use, keep doing random.
                do{
                    randomNumber = random.Next(0, NUM_CARDS);
                } while (numOfCards[randomNumber] == 0);

                numOfCards[randomNumber] -= 1; // Decrease 1, used this card at this round.
                globalIndex = randomNumber;

                // Set the rest values of the 17'th button.
                outerImage.setWithoutImgButton(gameCards[randomNumber]);
                setImage(outerImage.ImageButton,outerImage.ToString());

                // Start all game rules validations.
                randomNumber = gameRules.simulateAllRules();
                if (randomNumber != 0){
                    /*Sound*/
                    sp2.Play(_singleRowOrColumn, 1, 1, 0, 0, 1);
                    /*Sound*/
                }
                score.Text = (Int32.Parse(score.Text) + randomNumber).ToString();
                checkNextMove();
            }catch (Exception e){
                throw new Exception("Error : Next random card.\n" + e.Message);
            }
            
            
        }


        /// <summary>
        ///     When the game finishes, data is stored in the database.
        ///     Goes back to the starting screen.
        /// </summary>
        private async Task gameOver(){
            try{
                /*Sound*/

                sp3.Play(_gameOver, 100, 100, 0, 0, 100);
                /*Sound*/
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Game Over.\nYour Score is : " + score.Text);
                callDialog.SetNeutralButton("OK", delegate {
                    base.OnBackPressed();
                });
                callDialog.Show();
                await saveScore();
            }catch (Exception e){
                Toast.MakeText(this, "Error : Finishing the game.\n", ToastLength.Long).Show();
            
            }
            
        }

        private async Task saveScore()
        {
            /*
            var privateNote = new ParseObject("Users");
            
            privateNote["Score"] = Int32.Parse(score.Text);
            await privateNote.SaveAsync();
        
             */
            /*
            var query1 = from currentUser in ParseObject.GetQuery("Users")
                         where currentUser.Get<string>("Email").Equals(MainActivity.player.Email)
                         
                         select currentUser;
            */

           // IEnumerable<ParseObject> resultsCurrent = await queryCurrentPayer.FindAsync();
           /*
            ParseObject parseUser = new ParseObject("Users");
            parseUser.Remove(MainActivity.player.Email);
            await parseUser.SaveAsync();

            var user = new ParseObject("Users")
            {
                { "Name", MainActivity.player.Name },
                { "Email", MainActivity.player.Email},
                { "Password",MainActivity.player.Password } ,
            };
            user["Score"] = Int32.Parse(score.Text);
            await user.SaveAsync();
        */
            try
            {
                var query1 = from currentUser in ParseObject.GetQuery("Users")
                             where currentUser.Get<string>("Email") == MainActivity.player.Email
                             select currentUser;
                IEnumerable<ParseObject> resultsCurrent = await query1.FindAsync();
                var ElementTop = resultsCurrent.ElementAt(0);
                
              // string s = ElementTop.Get<string>("objectId");
                string s = ElementTop.ObjectId;
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Users");
                ParseObject parseUser = await query.GetAsync(s);
                parseUser["Score"] = Int32.Parse(score.Text);
                await parseUser.SaveAsync();
            }catch(Exception){
                Toast.MakeText(this, "Error : Finishing the game.\n", ToastLength.Long).Show();
            }

            }
   
        /// <summary>
        ///     Simulates for the first time the random card.
        ///     then action listeners for the buttons are called.
        /// </summary>
        private void simulate()
        {
            try{
                randomNextCard();
                buttonsClicks();

                checkNextMove();
            }catch (Exception e){
                throw new Exception("Error : Game Simulation.\n" + e.Message);
            }
        }
        /// <summary>
        ///     Check if the game board is playable.
        /// </summary>
        private async void checkNextMove()
        {
            int cnt = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (buttonsArray[i, j].Shape != "blank")
                    {
                        cnt++;
                    }
                    else
                    {
                        break;
                    }
                }
            }// if all bored is taken by cards.
            if (cnt == SIZE * SIZE)
            {
                await gameOver();
            }
        }

        /// <summary>
        ///     When a button is clicked, then an action listenr appears.
        ///     This method is responsible for each button.
        /// </summary>
        private void buttonsClicks()
        {
            try{
                
               for (int i = 0; i < SIZE; i++){
                   for (int j = 0; j < SIZE; j++) {
                       int ii = i, jj = j;
                       buttonsArray[i, j].ImageButton.Click += (sender, e) =>
                       {
                           onCardClick(ii, jj, globalIndex);
                       };
                   }
               }
        
            }
            catch (Exception e){
                throw new Exception("Error : Buttons click listener.\n" + e.Message);
            }
           
        }
        
        /// <summary>
        ///     When a button is clicked, activating all it's needs.
        ///     Setting from the outer image to the current button.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        /// <param name="delegateIndex"></param>
        private void onCardClick(int i,int j, int delegateIndex)
        {
            /*Sound*/
            sp.Play(_soundPushButton, 1, 1, 0, 0, 1);
            /*Sound*/
            buttonsArray[i, j].setWithoutImgButton(outerImage);
            setImage(buttonsArray[i, j].ImageButton, buttonsArray[i, j].ToString());
            buttonsArray[i, j].ImageButton.Enabled = false; // Disable the button.
            // Copy configurations from button 17 to [i,j] - needed for game rules.
            randomNextCard();
        }

        /// <summary>
        ///     Setting a button with her view image.
        /// </summary>
        /// <param name="btn"></param>
        /// <param name="shape"></param>
        private void setImage(ImageButton btn,string shape)
        {
            int drawbleid = Resources.GetIdentifier(shape , "drawable", this.PackageName);
            btn.SetImageResource(drawbleid);
        }
    }

}