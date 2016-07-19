using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;

namespace XamarinTokensPoker
{
    [Activity(MainLauncher = true ,ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,Icon ="@drawable/icon")]

    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


           
            RequestWindowFeature(WindowFeatures.NoTitle);

            SetContentView(Resource.Layout.Main);

            


            
            Button button = FindViewById<Button>(Resource.Id.MyButton);

            //button.Click += delegate { StartActivity(typeof(CreatePlayersActivity)); };
            button.Click += delegate { StartActivity(typeof(CreatePlayersActivity)); };
        }
    }
}

