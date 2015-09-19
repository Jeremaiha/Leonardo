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
        ImageButton imageButton;
        string shape;
        string color;
        int amount;

        /// <summary>
        ///     Empty constructor.
        /// </summary>
        public Card()
        {
            imageButton = null;
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
            imageButton = card.imageButton;
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
        public Card(ImageButton ib, string shp, string clr, int amnt)
        {
            imageButton = ib;
            shape = shp;
            color = clr;
            amount = amnt;
        }

        // Define properties.
        public ImageButton ImageButton
        {
            get { return imageButton; }
            set { imageButton = value; }
        }
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


    }
}