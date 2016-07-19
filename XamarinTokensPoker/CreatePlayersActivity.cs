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
    [Activity(Label = "CreatePlayersActivity", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape)]
    public class CreatePlayersActivity : Activity
    {
        EditText startcashEditText;
        EditText startsmallblind;
        ListView createplayerslistview;
        List<string> names = new List<string>();
        ImageButton newplayerbuttn;
        Button go_to_table;
        NumberPicker numberpicker;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature(WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.CreatePlayers);
            // Create your application here
            numberpicker = FindViewById<NumberPicker>(Resource.Id.numberPicker1);
            startcashEditText = FindViewById<EditText>(Resource.Id.cashforeveryoneEditText);
            createplayerslistview = FindViewById<ListView>(Resource.Id.createplayersListView);
            newplayerbuttn = FindViewById<ImageButton>(Resource.Id.newPlayerButton);
            go_to_table = FindViewById<Button>(Resource.Id.gotoTableButton);
            startsmallblind = FindViewById<EditText>(Resource.Id.smallblindforstart);

            numberpicker.MaxValue = 50;
            numberpicker.MinValue = 5;


            var adapter = new CreatePlayerAdapter(names, this);
            names.Add("");names.Add("");
            createplayerslistview.Adapter = adapter;


            newplayerbuttn.Click += (e, o) =>
            {
                
                if (names.Count < 10)
                {
                    names.Add("");
                    RunOnUiThread(() => { adapter.NotifyDataSetChanged(); });

                }
            };

            go_to_table.Click += (e, o)=>{
                long startcash;
                long smallblind;

                try
                {
                   startcash = long.Parse(startcashEditText.Text);
                    if (startcash > 99999999)
                        startcash = 99999999;
                    smallblind = long.Parse(startsmallblind.Text);
                bool canstart = true;
                for (int i = 0; i < createplayerslistview.Count; i++)
                {
                    EditText playername = createplayerslistview.GetChildAt(i).FindViewById<EditText>(Resource.Id.EnterName);
                        if (playername.Text.Length<1 || playername.TextColors.DefaultColor != Android.Graphics.Color.Black)
                        {
                            playername.Text = "Enter name";
                            playername.SetTextColor(Android.Graphics.Color.Gray);
                            canstart = false;
                        }
                        names[i] = playername.Text;
                }
                    if (canstart && startcash >= 10 && (smallblind > 10 && smallblind < startcash))
                {
                    Intent intent = new Intent(this, typeof(GameScreen));
                    
                    string[] namesarray = new string[names.Count];
                    for (int i = 0; i < names.Count; i++)
                    {
                        namesarray[i] = names.ElementAt(i);
                    }

                    


                    AlertDialog.Builder alertdialogbuilder = new AlertDialog.Builder(this);

                    alertdialogbuilder.SetTitle("Start game");
                        startcash = ((startcash / 10)* 10);
                        smallblind = ((smallblind / 10) * 10);
                    alertdialogbuilder.SetMessage("Do you wanna start a game  ? \r\nStart cash :" + startcash + ", small blind :" + smallblind);
                    alertdialogbuilder.SetCancelable(true);
                    
                    alertdialogbuilder.SetPositiveButton("Yes", (object sender,DialogClickEventArgs ea) => {
                        intent.PutExtra("raise", numberpicker.Value);
                        intent.PutExtra("listagraczy", namesarray);
                        intent.PutExtra("cashstart", startcash);
                        intent.PutExtra("smallblind", smallblind);
                        StartActivity(intent);
                    });
                    alertdialogbuilder.SetNegativeButton("No", (object sender, DialogClickEventArgs ea) => { });
                    

                    AlertDialog alert = alertdialogbuilder.Create();
                    alert.Show();

                }
                }
                catch (FormatException ex)
                {
                    AlertDialog.Builder builder = new AlertDialog.Builder(this);
                    builder.SetTitle("Cash problem");
                    builder.SetMessage("Enter the amount");
                    builder.SetNeutralButton("OK", delegate { });
                   AlertDialog alertd = builder.Create();
                   alertd.Show();
                  
                }

            };
           


        }

        
        


    }
    }
