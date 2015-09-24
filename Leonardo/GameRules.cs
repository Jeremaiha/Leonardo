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
using Android.Media;

namespace Leonardo
{
    /// <summary>
    ///     Defining all game rules.
    /// </summary>
    public class GameRules
    {
        Card[,] gameBoard;
        int gameSuccess;
        int SIZE;

        /// <summary>
        ///     C'tor.
        /// </summary>
        /// <param name="arrayOfButtons"></param>
        /// <param name="newSize"></param>
        public GameRules(Card[,] arrayOfButtons, int newSize)
        {
            gameBoard = arrayOfButtons;
            SIZE = newSize;
            gameSuccess = 0;
        }


        /// <summary>
        ///     Simulate all rules.
        /// </summary>
        /// <returns></returns>
        public int simulateAllRules()
        {
            try{
                int sum = 0;
                gameSuccess = 0;
                sum = checkRowsColumns();
                return sum;
            }
            catch(Exception e){
                throw new Exception("Error : Simulating game rules.\n" + e.Message);
            }

        }
        
        /// <summary>
        ///     Check combos,rows and columns.
        /// </summary>
        /// <returns></returns>
        public int checkRowsColumns(){
            int rowCnt, columnCnt, finalSum=0;
            for (int i = 0; i < SIZE; i++ ){ // for each row.
                rowCnt = checkRow(i);                    
                for (int j = 0; j < SIZE; j++){
                    columnCnt = checkColumn(j);
                    // combo
                    if (rowCnt > 0 && columnCnt > 0){
                        blankRow(i);
                        blankColumn(j);
                        finalSum += (rowCnt + columnCnt) * 2;
                    }
                    columnCnt = 0;
                }// there was a successful row.
                if(rowCnt != 0){
                    blankRow(i);
                }
            }//check left columns without rows.
            for (int j = 0; j < SIZE; j++){ // for each column.
                columnCnt = checkColumn(j);
                if(columnCnt != 0){
                    blankColumn(j);
                    finalSum += columnCnt;
                }
            }
            return finalSum;
        }

        /// <summary>
        ///     Blank the i row.
        /// </summary>
        /// <param name="i"></param>
        private void blankRow(int i)
        {
            for (int k = 0; k < SIZE; k++){
                gameBoard[i, k].ImageButton.SetImageResource(Resource.Drawable.blank);
                gameBoard[i, k].Shape = "blank";
                gameBoard[i, k].Color = "white";
                gameBoard[i, k].Amount = 0;
                gameBoard[i, k].ImageButton.Enabled = true;
            }
        }

        /// <summary>
        ///     Blank the j column.
        /// </summary>
        /// <param name="j"></param>
        private void blankColumn(int j)
        {
            for (int k = 0; k < SIZE;k++ ){
                gameBoard[k, j].ImageButton.SetImageResource(Resource.Drawable.blank);
                gameBoard[k, j].Shape = "blank";
                gameBoard[k, j].Color = "white";
                gameBoard[k, j].Amount = 0;
                gameBoard[k, j].ImageButton.Enabled = true;
            }
        }

  
        /// <summary>
        ///     Check an individual column.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int checkColumn(int i)
        {
            int sum = 0, cnt = 0, columnSuccess = 0; //if it's at least 1, then we erase row. 3 is extra points.
            string tempStr;
            int tempInt;

            // Check if there is at least 1 blank

            for (int j = 0; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[j, i].Shape == "blank")
                {
                    // at least 1 blank exists.
                    return 0;
                }
            }

            // Check amount.
            tempInt = gameBoard[0, i].Amount; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[j, i].Amount == tempInt)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                columnSuccess += 1;
            }
            // Check color.
            cnt = 0;
            tempStr = gameBoard[0, i].Color; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[j, i].Color == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                columnSuccess += 1;
            }
            // Check shape.
            cnt = 0;
            tempStr = gameBoard[0, i].Shape; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[j, i].Shape == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                columnSuccess += 1;
            }

            // If bonus needed.
            if (columnSuccess == 3)
            {
                sum *= 2;
            }

            // If at least 1 row exists.
           /*
            if (columnSuccess > 0){
                gameSuccess++;
                for (int j = 0; j < SIZE; j++)
                {
                    gameBoard[j, i].ImageButton.SetImageResource(Resource.Drawable.blank);
                    gameBoard[j, i].Shape = "blank";
                    gameBoard[j, i].Color = "white";
                    gameBoard[j, i].Amount = 0;
                    gameBoard[j, i].ImageButton.Enabled = true;
                }
            }*/
            return sum;
        }

        /// <summary>
        ///     Check individual row.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private int checkRow(int i)
        {
            int sum = 0, cnt = 0, rowSuccess = 0; //if it's at least 1, then we erase row. 3 is extra points.
            string tempStr;
            int tempInt;

            // Check if there is at least 1 blank

            for (int j = 0; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[i, j].Shape == "blank")
                {
                    // at least 1 blank exists.
                    return 0;
                }
            }

            // Check amount.
            tempInt = gameBoard[i, 0].Amount; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[i, j].Amount == tempInt)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                rowSuccess += 1;
            }
            // Check color.
            cnt = 0;
            tempStr = gameBoard[i, 0].Color; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[i, j].Color == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                rowSuccess += 1;
            }
            // Check shape.
            cnt = 0;
            tempStr = gameBoard[i, 0].Shape; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[i, j].Shape == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            { //Successfull row.
                sum += 50;
                rowSuccess += 1;
            }

            // If bonus needed.
            if (rowSuccess == 3)
            {
                sum *= 2;
            }

            // If at least 1 row exists.
          /*  if (rowSuccess > 0)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    gameBoard[i, j].ImageButton.SetImageResource(Resource.Drawable.blank);
                    gameBoard[i, j].Shape = "blank";
                    gameBoard[i, j].Color = "white";
                    gameBoard[i, j].Amount = 0;
                    gameBoard[i, j].ImageButton.Enabled = true;
                }
            }*/
            return sum;
        }
    }
}