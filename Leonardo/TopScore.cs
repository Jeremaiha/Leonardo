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

namespace Leonardo
{
    [Activity(ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait, Label = "Leonardo::TOP 10")]
    public class TopScore : Activity
    {
        // Sound variables.
        private static int STREAM_MUSIC = 0x00000003;
        SoundPool sp;
        int SoundPushButton;
        private TextView FirstColumn;
        private TextView SecondColumn;
        int top = 4;
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

            // Create your application here
        }


        private async void GetTop10()
        {

            try
            {
                //var query = ParseUser.Query.order", email);
                // ask for 10 in parse.
                // an option to show yourself too.
                //var query = ParseUser.Query.WhereEqualTo("email", email);
                var query = (from Users in ParseObject.GetQuery("Users")
                             orderby Users.Get<string>("Score") descending,
                             Users.Get<string>("Name"), Users.Get<int>("Score")
                             select Users).Limit(top);


                IEnumerable<ParseObject> results = await query.FindAsync();

                int cnt = 3;

                for (int i = 0; i < top; i++)
                {
                    var Element = results.ElementAt(i);
                    string FC = "TextView" + cnt;
                    string SC = "TextView" + (cnt + 1);
                    int rid_1 = Resources.GetIdentifier(FC, "id", this.PackageName);
                    int rid_2 = Resources.GetIdentifier(SC, "id", this.PackageName);
                    cnt += 2;
                    FirstColumn = FindViewById<TextView>(rid_1);
                    FirstColumn.Text = Element.Get<String>("Name");
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = Element.Get<int>("Score").ToString();

                }

            }
            catch (FormatException)
            {
                throw new FormatException();
            }

        }

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
                        var callDialog = new AlertDialog.Builder(this);
                        callDialog.SetMessage("Sign In, Please!");
                        callDialog.Show();
                        return;
                    }

                    var queryCurrentPayer = from Users in ParseObject.GetQuery("Users")
                                            where Users.Get<string>("Email") == MainActivity.player.Email
                                            select Users;

                    var queryTop = (from Users in ParseObject.GetQuery("Users")
                                    where Users.Get<int>("Score") > 50//MainActivity.player.Score
                                    orderby Users.Get<string>("Score") ascending,
                                    Users.Get<string>("Name"), Users.Get<int>("Score")
                                    select Users).Limit(2);


                    //UNCOMMENT when finish
                    //Check if player already have a score
                    /*      if (MainActivity.player.Score == 0)
                          {
                              var callDialog = new AlertDialog.Builder(this);
                              callDialog.SetMessage("You have no score!");
                              callDialog.Show();
                              return;
                          }*/


                    var queryBottom = (from Users in ParseObject.GetQuery("Users")
                                       where Users.Get<int>("Score") <= 50//MainActivity.player.Score
                                       where Users.Get<string>("Email") != MainActivity.player.Email
                                       orderby Users.Get<string>("Score") descending,
                                       Users.Get<string>("Name"), Users.Get<int>("Score")
                                       select Users).Limit(2);



                    IEnumerable<ParseObject> resultsCurrent = await queryCurrentPayer.FindAsync();
                    IEnumerable<ParseObject> resultsTop = await queryTop.FindAsync();
                    IEnumerable<ParseObject> resultsBottom = await queryBottom.FindAsync();

                    //Clear table
                    for (int i = 3; i < 21; i++)
                    {
                        string FC = "TextView" + i;
                        int rid = Resources.GetIdentifier(FC, "id", this.PackageName);
                        FirstColumn = FindViewById<TextView>(rid);
                        FirstColumn.Text = "";
                    }

                    // Show above
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

                    // show yourself
                    var ElementCurrent = resultsCurrent.ElementAt(0);
                    rid_temp = Resources.GetIdentifier("TextView" + cnt, "id", this.PackageName);
                    FirstColumn = FindViewById<TextView>(rid_temp);
                    FirstColumn.Text = ElementCurrent.Get<String>("Name");
                    rid_temp = Resources.GetIdentifier("TextView" + (cnt + 1), "id", this.PackageName);
                    SecondColumn = FindViewById<TextView>(rid_temp);
                    SecondColumn.Text = ElementCurrent.Get<int>("Score").ToString();
                    cnt += 2;
                    for (index = 0; index < 2; index++)
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
                    /*
                    var ElementTop = resultsTop.ElementAt(0);
                    int rid_1 = Resources.GetIdentifier("TextView3", "id", this.PackageName);
                    FirstColumn = FindViewById<TextView>(rid_1);
                    FirstColumn.Text = ElementTop.Get<String>("Name");
                    int rid_2 = Resources.GetIdentifier("TextView4", "id", this.PackageName);
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = ElementTop.Get<int>("Score").ToString();



                    var ElementCurrent = resultsCurrent.ElementAt(0);
                    rid_1 = Resources.GetIdentifier("TextView5", "id", this.PackageName);
                    FirstColumn = FindViewById<TextView>(rid_1);
                    FirstColumn.Text = ElementCurrent.Get<String>("Name");
                    rid_2 = Resources.GetIdentifier("TextView6", "id", this.PackageName);
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = ElementCurrent.Get<int>("Score").ToString();


                    var ElementBottom = resultsBottom.ElementAt(0);
                    rid_1 = Resources.GetIdentifier("TextView7", "id", this.PackageName);
                    FirstColumn = FindViewById<TextView>(rid_1);
                    FirstColumn.Text = ElementBottom.Get<String>("Name");
                    rid_2 = Resources.GetIdentifier("TextView8", "id", this.PackageName);
                    SecondColumn = FindViewById<TextView>(rid_2);
                    SecondColumn.Text = ElementBottom.Get<int>("Score").ToString();


             */


                }
                catch (Exception)
                {
                    var callDialog = new AlertDialog.Builder(this);
                    callDialog.SetMessage("Unknown error has occured");
                    callDialog.Show();
                }

            };
        }
    }
}