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
using Android.Graphics;

namespace Leonardo
{

    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::Game")]
    public class Game : Activity
    {
        const int SIZE = 4;

        //  Sound variables.
        SoundPool sp,sp2,sp3;
        int _soundPushButton;
        int _singleRowOrColumn;
        int _gameOver;
        // End of sound variables.
        
        // OuterButton - Displays the next card.
        Card  outerImage;   
        ImageButton outerButton;
        
        //  All buttons. - MVC is separation.
        Card[,] buttonsArray;
        TableLayout gameBoard;          // Contains the image buttons.
        ImageButton[,] gameImgButtons;  // ImageButtons which are created in the code.

        //Amount of all cards(3 shapes, 4 cards and 3 colors)
        const int NUM_CARDS =  (3 * 4 * 3);
        const int NUM_PACKETS = 2;  // How many packets you want in the game.

        Card[] gameCards;   // Array of all possible cards
        int[] numOfCards;   //Amount of each card in the game
        int numberOfCards = NUM_CARDS * NUM_PACKETS; // Substract each time when Random function is activated
        int globalIndex;   // To know which delegate is active in current time

        GameRules gameRules; // Holds the game board, and all its rules.
        TextView score,cardsLeft; // Shows score and cards left in the game.
        
        /// <summary>
        ///     Responsible to initiate all variables, views and so on.
        ///     Calls the Simulate method.
        /// </summary>
        /// <param name="bundle"></param>
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                // Layout initialisation
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.Game);
            
                // Activate sound.
                loadSounds(); 
            
                //  Call the initiation of all buttons method.
                initiateAll();

                simulate();
            }catch(Exception){
                showMessage("Something went wrong while trying to play.");
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

        /// <summary>
        ///     Loads all sound variables.
        /// </summary>
        private void loadSounds()
        {
            Stream st = new Stream();
            sp = new SoundPool(1, st, 100);
            sp2 = new SoundPool(1, st, 100);
            sp3 = new SoundPool(1, st, 100);
            _soundPushButton = sp.Load(this, Resource.Raw.buttonDown, 1);
            _singleRowOrColumn = sp2.Load(this, Resource.Raw.singleRowOrColumn, 1);
            _gameOver = sp3.Load(this, Resource.Raw.gameOver, 1);

        }

        /// <summary>
        ///     Responsible to call all initiation methods.
        /// </summary>
        private void initiateAll(){
            try{
                initiateButtonsArray();
                initiateSetOfImages();

                gameRules = new GameRules(buttonsArray, gameImgButtons, SIZE);
                
                // Assign score and cards number.
                assignTextViews();

                // Table creation.
                initializeTableLayout();

               }catch (Exception){
                   showMessage("Error in creating the layout");
               }
          
        }

        /// <summary>
        ///     Assign the textviews of score and cards left.
        /// </summary>
        private void assignTextViews()
        {
            score = FindViewById<TextView>(Resource.Id.scoreTextView);
            cardsLeft = FindViewById<TextView>(Resource.Id.cardsLeftTextView);
        }

        /// <summary>
        ///     Create the table layout, and the image buttons,
        ///     everything is assigned accordingly to the screen size.
        /// </summary>
        private void initializeTableLayout()
        {
            var metrics = Resources.DisplayMetrics;
            var widthInDp = metrics.WidthPixels;
            var heightInDp = metrics.HeightPixels;
            gameBoard = (TableLayout)FindViewById(Resource.Id.boardTable);
            TableRow.LayoutParams layoutParams = new TableRow.LayoutParams((widthInDp / 4), (widthInDp / 4));
            TableRow tableRow1 = new TableRow(this);
            TableRow tableRow2 = new TableRow(this);
            TableRow tableRow3 = new TableRow(this);
            TableRow tableRow4 = new TableRow(this);

            bool flag = false;
            for (int i = 0; i < SIZE; i++)
            {

                flag = !flag;
                for (int j = 0; j < SIZE; j++)
                {
                    gameImgButtons[i, j].SetImageResource(Resource.Drawable.blank);
                    gameImgButtons[i, j].LayoutParameters = layoutParams;
                    if (flag)
                    {
                        if ((j % 2) == 0)
                        {
                            gameImgButtons[i, j].SetBackgroundColor(Color.Black);
                        }
                        else
                        {
                            gameImgButtons[i, j].SetBackgroundColor(Color.Gray);
                        }
                    }
                    else
                    {
                        if ((j % 2) != 0)
                        {
                            gameImgButtons[i, j].SetBackgroundColor(Color.Black);
                        }
                        else
                        {
                            gameImgButtons[i, j].SetBackgroundColor(Color.Gray);
                        }
                    }

                }
                tableRow1.AddView(gameImgButtons[0, i], i);
                tableRow2.AddView(gameImgButtons[1, i], i);
                tableRow3.AddView(gameImgButtons[2, i], i);
                tableRow4.AddView(gameImgButtons[3, i], i);
            }
            // Add rows to table
            gameBoard.AddView(tableRow1, 0);
            gameBoard.AddView(tableRow2, 1);
            gameBoard.AddView(tableRow3, 2);
            gameBoard.AddView(tableRow4, 3);
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
                for (int i = 0; i < NUM_CARDS; i++){
                    numOfCards[i] = 4 * NUM_PACKETS;
                }

                // Initialize the cards symbols,color and amount.
                // Order : Green , Red , Yellow.
                // Type  : Cherry, Mushroom , Strawberry.

                // Green
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("cherry", "green", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card( "mushroom", "green", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("straw", "green", i + 1);
                }

                // Red
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("cherry", "red", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("mushroom", "red", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("straw", "red", i + 1);
                }

                // Yellow
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("cherry", "yellow", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("mushroom", "yellow", i + 1);
                }
                for (int i = 0; i < 4; i++, localCnt++)
                {
                    gameCards[localCnt] = new Card("straw", "yellow", i + 1);
                }
            }
            catch (Exception e){
                showMessage("Error in assiging cards.");
            }
            
        }

        /// <summary>
        ///     Initiating the 2 dimentional array of buttons with a blank image.
        /// </summary>
        private void initiateButtonsArray(){
            try{
                // Allocate the array of cards, which contains the properties.
                buttonsArray = new Card[SIZE, SIZE];
                // Instantiate with white properties.
                for (int i = 0; i < SIZE; i++){
                    for (int j = 0; j < SIZE; j++){
                       buttonsArray[i, j] = new Card( "blank", "white", 0); 
                    }
                }
                
                // Initialize the button 
                outerImage = new Card("blank", "white", 0);
                outerButton = FindViewById<ImageButton>(Resource.Id.outerImgBtn);
                outerButton.SetImageResource(Resource.Drawable.blank);
               
                // Allocate the two dimensional array of image buttons.
                gameImgButtons = new ImageButton[SIZE, SIZE];

                //  Initialize Image buttons array.
                for (int i = 0; i < SIZE; i++){
                    for (int j = 0; j < SIZE; j++){
                        gameImgButtons[i, j] = new ImageButton(this);
                    }
                }

                // Set all images with the blank image.
                for (int i = 0; i < SIZE; i++){
                    for (int j = 0; j < SIZE; j++){
                        gameImgButtons[i, j].SetImageResource(Resource.Drawable.blank);
                    }
                }
            }
            catch (Exception){
                showMessage("Something went wrong in game initialization");
            }
            
    }

        /// <summary>
        ///     Is responsible for giving us the next random card from the possible cards.
        /// </summary>
        private async void randomNextCard(){
            try{
                //  Get random card.
                Random random = new Random();
                int randomNumber, currentScoreTaken;

                // Check if there are any cards left.
                if (numberOfCards != -1){ 
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

                // Got a card which I can use.
                // Decrease 1, used this card at this round.
                numOfCards[randomNumber] -= 1; 
                // Point to the specific card definition in the array of cards.
                globalIndex = randomNumber; 

                // Set the rest values of the outer button.
                outerImage.setWithoutImgButton(gameCards[randomNumber]);
                setImage(outerButton,outerImage.ToString());

                // Start all game rules validations.
                currentScoreTaken = gameRules.simulateAllRules();

                if (currentScoreTaken != 0){
                    // Sound activation
                    sp2.Play(_singleRowOrColumn, 1, 1, 0, 0, 1);
                }
                // Show score in the score text view.
                score.Text = (Int32.Parse(score.Text) + currentScoreTaken).ToString();
                // If next move is possible.
                checkNextMove();
            }catch (Exception){
                showMessage("Error in getting next card.");
            }    
        }


        /// <summary>
        ///     When the game finishes, data is stored in the database.
        ///     Goes back to the starting screen.
        /// </summary>
        private async Task gameOver(){
            try{
                // Activate sound/
                sp3.Play(_gameOver, 100, 100, 0, 0, 100);
                
                // Show a big message with the score the user got, and that the game is over now.
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Game Over.\nYour Score is : " + score.Text);
                callDialog.SetNeutralButton("OK", delegate {
                    base.OnBackPressed();
                });
                callDialog.Show();
                await saveScore();
            }catch (Exception){
                showMessage("Error : Finishing the game.\n");
            }
            
        }

        /// <summary>
        ///     Initiates a parse object, which is the current user
        ///     only then we can update the user's score.
        /// </summary>
        /// <returns></returns>
        private async Task saveScore()
        {
            try{
                // Query to get the current user.
                var query1 = from currentUser in ParseObject.GetQuery("Users")
                             where currentUser.Get<string>("Email") == MainActivity.player.Email
                             select currentUser;
                //  Activate query.
                IEnumerable<ParseObject> resultsCurrent = await query1.FindAsync();
                //  Take the value.
                var userVar = resultsCurrent.ElementAt(0);
                
                //  By object id, we can receive the parse object,
                //      and then update its score.
                string s = userVar.ObjectId;
                ParseQuery<ParseObject> query = ParseObject.GetQuery("Users");
                //  Get the parse object - for current user.
                ParseObject parseUser = await query.GetAsync(s);
                // Update the score.
                parseUser["Score"] = Int32.Parse(score.Text);
                
                //  Save changes.
                await parseUser.SaveAsync();
            }catch(Exception){
                showMessage("Error while trying to finish and update the score.");
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
                showMessage("Error in game simulation");
            }
        }
        /// <summary>
        ///     Check if the game board is playable.
        /// </summary>
        private async void checkNextMove()
        {
            int cnt = 0;
            for (int i = 0; i < SIZE; i++){
                for (int j = 0; j < SIZE; j++){
                    if (buttonsArray[i, j].Shape != "blank"){
                        cnt++;
                    }else{
                        break;
                    }
                }
            } 
            // if all bored is taken by cards.
            if (cnt == SIZE * SIZE){
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
                       // Closure, so we need new variables.
                       int ii = i, jj = j;
                       gameImgButtons[i, j].Click += (sender, e) =>
                       {
                           onCardClick(ii, jj, globalIndex);
                       };
                   }
               }
        
            }catch (Exception){
                showMessage("Error in clicking the button");
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
            // Activate sound
            sp.Play(_soundPushButton, 1, 1, 0, 0, 1);
            
            buttonsArray[i, j].setWithoutImgButton(outerImage);
            setImage(gameImgButtons[i, j], buttonsArray[i, j].ToString());
            gameImgButtons[i, j].Enabled = false; // Disable the button.
            // Copy configurations from outer button to [i,j] - needed for game rules.
            randomNextCard();
        }

        /// <summary>
        ///     Setting a button with her image view.
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