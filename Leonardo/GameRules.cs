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
        ImageButton[,] gameImages;
        int SIZE;

        /// <summary>
        ///     C'tor.
        /// </summary>
        /// <param name="arrayOfButtons"></param>
        /// <param name="newSize"></param>
        public GameRules(Card[,] arrayOfButtons,ImageButton[,] arrayOfImgs,int newSize){
            gameBoard = arrayOfButtons;
            gameImages = arrayOfImgs;
            SIZE = newSize;
        }

        /// <summary>
        ///     Simulate all rules.
        /// </summary>
        /// <returns></returns>
        public int simulateAllRules()
        {
            try{
                int sum = 0;
                sum = checkRowsColumnsDiagonals();
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
        public int checkRowsColumnsDiagonals(){
            // Variables.
            int rowCnt, columnCnt,right2leftDiagonal,left2rightDiagonal ,finalSum=0;
            bool temp = false;

            // Assign values for diagonals.
            right2leftDiagonal = checkRight2LeftDiagonal();
            left2rightDiagonal = checkLeft2RightDiagonal();

            // Check combos.
            for (int i = 0; i < SIZE; i++ ){ // for each row.
                rowCnt = checkRow(i);                    
                for (int j = 0; j < SIZE; j++){ // for each column
                    columnCnt = checkColumn(j);
                    // Full Triapod
                    if (rowCnt > 0 && columnCnt > 0 && left2rightDiagonal > 0){
                        blankRow(i);
                        blankColumn(j);
                        blankLeft2RightDiagonal();
                        finalSum += (rowCnt + columnCnt + left2rightDiagonal) * 2;
                        left2rightDiagonal = 0;
                    }// Full triapod
                    else if (rowCnt > 0 && columnCnt > 0 && right2leftDiagonal > 0){
                        blankRow(i);
                        blankColumn(j);
                        blankRight2LeftDiagonal();
                        finalSum += (rowCnt + columnCnt + right2leftDiagonal) * 2;
                        right2leftDiagonal = 0;
                    //  Column and a row.
                    }else if (rowCnt > 0 && columnCnt > 0){
                        blankRow(i);
                        blankColumn(j);
                        finalSum += (rowCnt + columnCnt) * 2;
                    }
                    columnCnt = 0;
                }// there was a successful row.
                if(rowCnt != 0){
                    finalSum += (rowCnt);
                    temp = true;
                    blankRow(i);
                }
            }
      
            // Check left columns, without rows.
            for (int j = 0; j < SIZE; j++){ // for each column.
                columnCnt = checkColumn(j);
                if(columnCnt != 0){
                    blankColumn(j);
                    finalSum += columnCnt;
                }
            }
            // End of checking.

            // Check if any diagonals are left after everything.
            if (right2leftDiagonal > 0){
                // if there was a row and a diagonal.
                if(temp){
                    finalSum += (right2leftDiagonal * 2);
                }else{
                    finalSum += (right2leftDiagonal);
                }
                blankRight2LeftDiagonal();
            }
            if (left2rightDiagonal > 0){
                // if there was a row and a diagonal.
                if (temp){
                    finalSum += (left2rightDiagonal * 2);
                }else{
                    finalSum += (left2rightDiagonal);
                } blankLeft2RightDiagonal();
            }
            // End of checking the left diagonals.

            return finalSum;
        }

        /// <summary>
        ///     Blank the i row.
        /// </summary>
        /// <param name="i"></param>
        private void blankRow(int i)
        {
            for (int k = 0; k < SIZE; k++){
                blankOnIandJ(i, k);
            }
        }

        /// <summary>
        ///     Blank the j column.
        /// </summary>
        /// <param name="j"></param>
        private void blankColumn(int j)
        {
            for (int k = 0; k < SIZE;k++ ){
                blankOnIandJ(k, j);
            }
        }

        /// <summary>
        ///     Blank the diagonal from the upper right to the left.
        /// </summary>
        private void blankRight2LeftDiagonal()
        {
            for (int i = SIZE - 1; i >= 0; i--)
            {
                blankOnIandJ(i, SIZE - 1 - i);
            }
            
        }

        /// <summary>
        ///     Blank the diagonal from the left upper to the right.
        /// </summary>
        private void blankLeft2RightDiagonal()
        {
            for (int i = 0; i < SIZE; i++)
            {
                blankOnIandJ(i, i);
            }
        }

        /// <summary>
        ///     Blank on the game board[i,j].
        /// </summary>
        /// <param name="i"></param>
        /// <param name="j"></param>
        private void blankOnIandJ(int i,int j)
        {
            gameImages[i, j].SetImageResource(Resource.Drawable.blank);
            gameBoard[i, j].Shape = "blank";
            gameBoard[i, j].Color = "white";
            gameBoard[i, j].Amount = 0;
            gameImages[i, j].Enabled = true;
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
            { //Successfull column.
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
            { //Successfull column.
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
            { //Successfull column.
                sum += 50;
                columnSuccess += 1;
            }

            // If bonus needed.
            if (columnSuccess == 3){
                sum *= 2;
            }
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
            if (rowSuccess == 3){
                sum *= 2;
            }
            return sum;
        }

        /// <summary>
        ///     Check from the left upper corner to the lower right, the diagonal.
        /// </summary>
        /// <returns></returns>
        private int checkLeft2RightDiagonal()
        {
            int sum=0,tempInt,cnt=0,diagonalSuccess=0;
            string tempStr;
            for (int j = 0; j < SIZE; j++){ //checked with the rest.
                if (gameBoard[j, j].Shape == "blank"){
                    // at least 1 blank exists.
                    return 0;
                }
            }
            // Check amount.
            tempInt = gameBoard[0, 0].Amount; //took the first.
            for (int j = 1; j < SIZE; j++){ //checked with the rest.
                if (gameBoard[j, j].Amount == tempInt){
                    cnt++;
                }
            }
            if (cnt == 3){ 
                sum += 50;
            }
            // Check color.
            cnt = 0;
            tempStr = gameBoard[0, 0].Color; //took the first.
            for (int j = 1; j < SIZE; j++){ //checked with the rest.
                if (gameBoard[j, j].Color == tempStr){
                    cnt++;
                }
            }
            if (cnt == 3){
                sum += 50;
            }
            // Check shape.
            cnt = 0;
            tempStr = gameBoard[0, 0].Shape; //took the first.
            for (int j = 1; j < SIZE; j++){ //checked with the rest.
                if (gameBoard[j, j].Shape == tempStr){
                    cnt++;
                }
            }
            if (cnt == 3){ //Successfull diagonal.
                sum += 50;
                diagonalSuccess += 1;
            }

            // If bonus needed.
            if (diagonalSuccess == 3){
                sum *= 2;
            }
            return sum;
        }

        /// <summary>
        ///     Check from the right upper corner to the lower left, the diagonal.
        /// </summary>
        /// <returns></returns>
        private int checkRight2LeftDiagonal()
        {
            int sum = 0, tempInt, cnt = 0, diagonalSuccess = 0;
            string tempStr;
            for (int j = 0; j < SIZE ; j++)
            { //checked with the rest.
                if (gameBoard[j, SIZE - 1 - j].Shape == "blank")
                {
                    // at least 1 blank exists.
                    return 0;
                }
            }
            // Check amount.
            tempInt = gameBoard[0, SIZE-1].Amount; //took the first.
            for (int j = 1; j < SIZE; j++)
            { //checked with the rest.
                if (gameBoard[j, SIZE - 1 - j].Amount == tempInt)
                {
                    cnt++;
                }
            }
            if (cnt == 3){
                sum += 50;
            }
            // Check color.
            cnt = 0;
            tempStr = gameBoard[0, SIZE - 1].Color; //took the first.
            for (int j = 1; j < SIZE; j++){
                if (gameBoard[j, SIZE - 1 - j].Color == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3)
            {
                sum += 50;
            }
            // Check shape.
            cnt = 0;
            tempStr = gameBoard[0, SIZE - 1].Shape; //took the first.
            for (int j = 1; j < SIZE; j++){
                if (gameBoard[j, SIZE - 1 - j].Shape == tempStr)
                {
                    cnt++;
                }
            }
            if (cnt == 3){ //Successfull diagonal.
                sum += 50;
                diagonalSuccess += 1;
            }

            // If bonus needed.
            if (diagonalSuccess == 3){
                sum *= 2;
            }
            return sum;
        }

    }
}