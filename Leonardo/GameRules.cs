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
        //TextView score;
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
            //score = newScore;
        }


        /// <summary>
        ///     Simulate all rules.
        /// </summary>
        /// <returns></returns>
        public int simulateAllRules()
        {
            try{
                int sum = 0;
                sum = checkColumns();
                sum += checkRows();
                return sum;
            }
            catch(Exception e){
                throw new Exception("Error : Simulating game rules.\n" + e.Message);
            }

        }
        /// <summary>
        ///     Returns total sum of all columns.
        /// </summary>
        /// <returns></returns>
        private int checkColumns()
        {
            try{
                int sum = 0;
                for (int i = 0; i < SIZE; i++){
                    sum += checkColumn(i);
                }
                return sum;
            }catch (Exception e){
                throw new Exception("Error : Checking columns.\n" + e.Message);
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
            if (columnSuccess > 0)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    gameBoard[j, i].ImageButton.SetImageResource(Resource.Drawable.blank);
                    gameBoard[j, i].Shape = "blank";
                    gameBoard[j, i].Color = "white";
                    gameBoard[j, i].Amount = 0;
                    gameBoard[j, i].ImageButton.Enabled = true;
                }
            }
            return sum;
        }

        /// <summary>
        ///     Returns total sum of all rows.
        /// </summary>
        /// <returns></returns>
        private int checkRows()
        {
            try{
                int sum = 0;
                for (int i = 0; i < SIZE; i++){
                    sum += checkRow(i);
                }
                return sum;
            }catch (Exception e){
                throw new Exception("Error : Checking rows.\n" + e.Message);
            }
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
            if (rowSuccess > 0)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    gameBoard[i, j].ImageButton.SetImageResource(Resource.Drawable.blank);
                    gameBoard[i, j].Shape = "blank";
                    gameBoard[i, j].Color = "white";
                    gameBoard[i, j].Amount = 0;
                    gameBoard[i, j].ImageButton.Enabled = true;
                }
            }
            return sum;
        }
    }
}