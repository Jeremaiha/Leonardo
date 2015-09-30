using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Media;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using System.Threading.Tasks;
using System.Json;
using Parse;
using System.Threading;
using Android.Graphics;

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::TOP 10")]
    public class TopScore : Activity
    {
        // Sound variables.
        SoundPool sp;
        int SoundPushButton;
        private TextView FirstColumn;
        private TextView SecondColumn;
        int top = 10;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.TopScore);
            // Sound initialization.
            Stream st = new Stream();
            sp = new SoundPool(1, st, 0);
            SoundPushButton = sp.Load(this, Resource.Raw.clickInMenu, 1);
            // Assign the button and columns
            Button WhereAreU = FindViewById<Button>(Resource.Id.buttonWhereAreU);
            FirstColumn = FindViewById<TextView>(Resource.Id.TextView3);
            SecondColumn = FindViewById<TextView>(Resource.Id.TextView4);
            //Shows data from DB
            GetTop10();
            // button click
            WhereAreUClick(WhereAreU);
        }
        /// <summary>
        ///     Get ordered data from DB
        ///     and show it in table
        /// </summary>
        private async void GetTop10()
        {
            try
            {
                var query = (from Users in ParseObject.GetQuery("Users")
                             orderby Users.Get<string>("Score") descending,
                             Users.Get<string>("Name"), Users.Get<int>("Score")
                             select Users).Limit(top);
                //save the result of query into list
                IEnumerable<ParseObject> results = await query.FindAsync();
                int cnt = 3;
                //loop on elements of list and printing them
                for (int i = 0; i < top; i++)
                {
                    var Element = results.ElementAt(i);
                    string FC = "TextView" + cnt;
                    string SC = "TextView" + (cnt + 1);
                    int rid_1 = Resources.GetIdentifier(FC, "id", this.PackageName);
                    int rid_2 = Resources.GetIdentifier(SC, "id", this.PackageName);
                    cnt += 2;
                    FirstColumn = FindViewById<TextView>(rid_1);
                    if (Element.Get<String>("Name").Equals(MainActivity.player.Name))
                    {
                        FirstColumn.SetTextColor(Android.Graphics.Color.Red);
                    }
                    FirstColumn.Text = Element.Get<String>("Name");
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = Element.Get<int>("Score").ToString();
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }//GetTop10
        /// <summary>
        ///     Get ordered data from DB
        /// </summary>
        private async void GetLow10()
        {
            try
            {
                var query = (from Users in ParseObject.GetQuery("Users")
                             orderby Users.Get<string>("Score") ascending,
                             Users.Get<string>("Name"), Users.Get<int>("Score")
                             select Users).Limit(top);
                //save results of query in list
                IEnumerable<ParseObject> results = await query.FindAsync();
                int cnt = 3;
                //loop on elements of list
                for (int i = 0; i < top; i++)
                {
                    var Element = results.ElementAt(i);
                    string FC = "TextView" + cnt;
                    string SC = "TextView" + (cnt + 1);
                    int rid_1 = Resources.GetIdentifier(FC, "id", this.PackageName);
                    int rid_2 = Resources.GetIdentifier(SC, "id", this.PackageName);
                    cnt += 2;
                    FirstColumn = FindViewById<TextView>(rid_1);
                    if (Element.Get<String>("Name").Equals(MainActivity.player.Name))
                    {
                        FirstColumn.SetTextColor(Android.Graphics.Color.Red);
                    }
                    FirstColumn.Text = Element.Get<String>("Name");
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = Element.Get<int>("Score").ToString();
                }
            }
            catch (FormatException)
            {
                throw new FormatException();
            }
        }//GetLow10
        /// <summary>
        /// Gets data from DB and shows palce of current user in table
        /// after clicking "Where am I" button on screen
        /// </summary>
        /// <param name="WhereAreU"></param>
        private void WhereAreUClick(Button WhereAreU)
        {
            WhereAreU.Click += async (sender, e) =>
            {
                try
                {
                    // play button click sound.
                    sp.Play(SoundPushButton, 1, 1, 0, 0, 1);
                    //Check if user is already registered
                    if (MainActivity.player.Email == null)
                    {
                        //If no show message
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Sign In, Please!");
                        callDialog.Show();
                        return;
                    }
                    //Check if player already have a score
                    var queryCurrentPayer = from Users in ParseObject.GetQuery("Users")
                                            where Users.Get<string>("Email") == MainActivity.player.Email
                                            select Users;
                    IEnumerable<ParseObject> resultsCurrent = await queryCurrentPayer.FindAsync();
                    var ElementCurrent = resultsCurrent.ElementAt(0);
                    int currentPlayerScore = ElementCurrent.Get<int>("Score");
                    //If have no score - show message
                    if (currentPlayerScore == 0)
                    {
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Your score is 0. Try one more time!");
                        callDialog.Show();
                        return;
                    }
                    //Query gets list of 2 users which have greater score than current User
                    var queryTop = (from Users in ParseObject.GetQuery("Users")
                                    where Users.Get<int>("Score") > currentPlayerScore
                                    orderby Users.Get<string>("Score") ascending,
                                    Users.Get<string>("Name"), Users.Get<int>("Score")
                                    select Users).Limit(2);
                    //Query gets list of 2 users which have lower score than current User
                    var queryBottom = (from Users in ParseObject.GetQuery("Users")
                                       where Users.Get<int>("Score") < currentPlayerScore
                                       where Users.Get<string>("Email") != MainActivity.player.Email
                                       orderby Users.Get<string>("Score") descending,
                                       Users.Get<string>("Name"), Users.Get<int>("Score")
                                       select Users).Limit(2);
                    IEnumerable<ParseObject> resultsTop = await queryTop.FindAsync();
                    IEnumerable<ParseObject> resultsBottom = await queryBottom.FindAsync();
                    if (resultsTop.Count() < 2)
                    {
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Congratulations! Your are in TOP!");
                        callDialog.Show();
                        return;
                    }
                    if (resultsBottom.Count() == 0)
                    {
                        GetLow10();//Show the wrost scores
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Oops! You are the last.");
                        callDialog.Show();
                        return;
                    }
                    //Clear table
                    for (int i = 3; i < 23; i++)
                    {
                        string FC = "TextView" + i;
                        int rid = Resources.GetIdentifier(FC, "id", this.PackageName);
                        FirstColumn = FindViewById<TextView>(rid);
                        FirstColumn.Text = "";
                    }
                    //Show above score
                    int cnt = 3;
                    int rid_temp;
                    int index;
                    for (index = 1; index >= 0; index--)
                    {
                        var ElementTop = resultsTop.ElementAt(index);
                        rid_temp = Resources.GetIdentifier("TextView" + (cnt), "id", this.PackageName);
                        FirstColumn = FindViewById<TextView>(rid_temp);
                        FirstColumn.Text = ElementTop.Get<String>("Name");
                        rid_temp = Resources.GetIdentifier("TextView" + (cnt + 1), "id", this.PackageName);
                        SecondColumn = FindViewById<TextView>(rid_temp);
                        SecondColumn.Text = ElementTop.Get<int>("Score").ToString();
                        cnt += 2;
                    }
                    // show yourself score
                    rid_temp = Resources.GetIdentifier("TextView" + cnt, "id", this.PackageName);
                    FirstColumn = FindViewById<TextView>(rid_temp);
                    FirstColumn.SetTextColor(Android.Graphics.Color.Red);
                    FirstColumn.Text = ElementCurrent.Get<String>("Name");
                    rid_temp = Resources.GetIdentifier("TextView" + (cnt + 1), "id", this.PackageName);
                    SecondColumn = FindViewById<TextView>(rid_temp);
                    SecondColumn.SetTextColor(Android.Graphics.Color.Red);
                    SecondColumn.Text = ElementCurrent.Get<int>("Score").ToString();
                    cnt += 2;
                    //show lower scores
                    for (index = 0; index < resultsBottom.Count(); index++)
                    {
                        var elementBot = resultsBottom.ElementAt(index);
                        rid_temp = Resources.GetIdentifier("TextView" + (cnt), "id", this.PackageName);
                        FirstColumn = FindViewById<TextView>(rid_temp);
                        FirstColumn.Text = elementBot.Get<String>("Name");
                        rid_temp = Resources.GetIdentifier("TextView" + (cnt + 1), "id", this.PackageName);
                        SecondColumn = FindViewById<TextView>(rid_temp);
                        SecondColumn.Text = elementBot.Get<int>("Score").ToString();
                        cnt += 2;
                    }
                }catch (Exception){
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Unknown error has occured");
                    callDialog.Show();
                }

            };
        }//WhereAreU
    }
}