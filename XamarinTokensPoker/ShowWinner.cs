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

namespace XamarinTokensPoker
{
    [Activity(Label = "ShowWinner", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class ShowWinner : Activity
    {
     
       // TextView winner;
        Button newGamebutton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.ShowWinner);
           
            string winner = Intent.GetStringExtra("thewinner").ToUpper();
            winner = FindViewById<TextView>(Resource.Id.ShowWinnerText).Text ="The winner is \r\n\r\n" +  winner ;

            newGamebutton = FindViewById<Button>(Resource.Id.NewGameButton);
              newGamebutton.Click += delegate { StartActivity(typeof(MainActivity)); };
        }
    }
}