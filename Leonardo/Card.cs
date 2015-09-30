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
    /// <summary>
    ///     Card class which is a card container.
    ///     Contains an imageButton,Color,Shape and Amount.
    /// </summary>
    public class Card
    {

        // Define fields.
        string shape;
        string color;
        int amount;

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Card()
        {
            shape = "";
            color = "";
            amount = 0;
        }

        /// <summary>
        ///     Copy C'tor.
        ///     For an object to be copied deeply.
        /// </summary>
        /// <param name="card"></param>
        public Card(Card card)
        {
            shape = card.shape;
            color = card.color;
            amount = card.amount;
        }

        /// <summary>
        ///     Full constructor.
        /// </summary>
        /// <param name="ib">ImageButton </param>
        /// <param name="shp">Shape</param>
        /// <param name="clr">Color</param>
        /// <param name="amnt">Amount</param>
        public Card(string shp, string clr, int amnt)
        {
            shape = shp;
            color = clr;
            amount = amnt;
        }
        // Properties
        public string Shape
        {
            get { return shape; }
            set { shape = value; }
        }
        public string Color
        {
            get { return color; }
            set { color = value; }
        }
        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        ///     Set param1's shape,color and amount to the current button.
        /// </summary>
        /// <param name="card"></param>
        public void setWithoutImgButton(Card card){
            this.shape  = card.shape;
            this.color  = card.color;
            this.amount = card.amount;
        }

        /// <summary>
        ///     Returns a string in the following structure :
        ///         <color>_<shape>_<amount>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return color + "_" + shape + "_" + amount;

        }
    }
}