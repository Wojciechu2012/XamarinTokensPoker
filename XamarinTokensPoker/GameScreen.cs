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
using Android.Support.V4.App;
using Android.Support.V4.Widget;

namespace XamarinTokensPoker
{
    [Activity(Label = "Table", ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape, Icon = "@android:color/transparent")]
    public class GameScreen : Activity
    {
        Game game;
        ListView lv;
        TextView activPlayer;
        TextView FlopRound;
        TextView Bidtext;
        TextView actualblindtv;
        TextView roundpottv;


        Button Check;
        Button Call;
        Button pass;
        Button Raisebutton;

        SeekBar Bidbar;


        List<string> mLeftItems = new List<string>();

        ListView mleftDrawer;
        DrawerLayout mDrawerLyaout;
        ActionBarDrawerToggle mDrawerToggle;





        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);
          //  
            SetContentView(Resource.Layout.GameScreen);
            var smallblind = Intent.GetLongExtra("smallblind", 10);
            var startcash = Intent.GetLongExtra("cashstart",5000);
            var roundtoriseblind = Intent.GetIntExtra("raise", 1);
            game = new Game(startcash, smallblind, this,roundtoriseblind);
            string[] playerlistnames = Intent.GetStringArrayExtra("listagraczy");

            foreach(string name in playerlistnames)
            {
                game.CreatePlayer(name);
            }



            mLeftItems.Add("Add player");
            mLeftItems.Add("Delete player");
            mLeftItems.Add("Restart");

            mLeftItems.Add("Exit");


            game.startNextRound();
            setLayoutItems();
            StartGame();
           

      

           

        }
        public void setLayoutItems()
        {
            FlopRound = FindViewById<TextView>(Resource.Id.Round);
            lv = FindViewById<ListView>(Resource.Id.listView1);
            activPlayer = FindViewById<TextView>(Resource.Id.activPlayer);
            Check = FindViewById<Button>(Resource.Id.CheckButton);
            pass = FindViewById<Button>(Resource.Id.Pass);
            Raisebutton = FindViewById<Button>(Resource.Id.Rise);
            Call = FindViewById<Button>(Resource.Id.Call);
            Bidtext = FindViewById<TextView>(Resource.Id.Bid);
            Bidbar = FindViewById<SeekBar>(Resource.Id.seekBar1);
            actualblindtv = FindViewById<TextView>(Resource.Id.actualblindtxt);
            roundpottv = FindViewById<TextView>(Resource.Id.roundpot);
            mDrawerLyaout = FindViewById<DrawerLayout>(Resource.Id.myDrawer);
            mleftDrawer = FindViewById<ListView>(Resource.Id.leftListView);



            Check.Click += Check_Click;
            Call.Click += Call_Click;
            pass.Click += Pass_Click;
            Raisebutton.Click += Raisebutton_Click;


            UpgradePlayerInfo();

            Bidtext.Text = "Bid: " + game.actualbid * 2;
            Bidbar.Max = (int)game.actualPlayer.Cash;
            Bidbar.Progress = (int)game.actualbid * 2;
            Bidbar.ProgressChanged += Bidbar_ProgressChanged;


            mDrawerToggle = new ActionBarDrawerToggle(this, mDrawerLyaout, Android.Resource.Drawable.IcInputAdd, Resource.String.open_drawer, Resource.String.close_drawer);


            mleftDrawer.Adapter = new ActionBarAdapter(mLeftItems, this);



            mDrawerLyaout.SetDrawerListener(mDrawerToggle);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);


            mleftDrawer.ItemClick += (sender, e) =>
            {

                switch (e.Position)
                {
                    case 0:
                        if (game.playerlist.Count < 10)
                        {
                            SetContentView(Resource.Layout.AddPlayer);
                            Addplayer();
                        }
                        else Toast.MakeText(this, "Too much players", ToastLength.Long).Show();
                        break;
                    case 1:
                        SetContentView(Resource.Layout.Deleteplayer);
                        DeletePlayer();

                        break;
                    case 2:
                        StartActivity(typeof(CreatePlayersActivity));
                        break;
                    default:
                        System.Environment.Exit(0);
                        break;
                }
                Toast.MakeText(this, e.Position.ToString(), ToastLength.Long).Show();
            };
        }

        private void DeletePlayer()
        {
            ListView playertodelete = FindViewById<ListView>(Resource.Id.playerstodeletelistview);
            playertodelete.Adapter = new PlayerAdapter(game.playerlist, this);
            playertodelete.ItemClick += (sender, e) =>
            {
                AlertDialog.Builder alertdialogbuilder = new AlertDialog.Builder(this);

                alertdialogbuilder.SetTitle("Delete player");
                string message = string.Format("Do you wanna delete player {0} ?", game.playerlist[e.Position].Name);
                alertdialogbuilder.SetMessage(message); 
                alertdialogbuilder.SetCancelable(true);

                alertdialogbuilder.SetPositiveButton("Yes", (object s, DialogClickEventArgs ea) => {
                    game.playerlist.RemoveAt(e.Position);
                    SetContentView(Resource.Layout.GameScreen);
                    setLayoutItems();
                    game.checkplayersCount();
                    
                });
                alertdialogbuilder.SetNegativeButton("No", (object s, DialogClickEventArgs ea) => { });


                AlertDialog alert = alertdialogbuilder.Create();
                alert.Show();

            };
        }

        private void Addplayer()
        {
            EditText playername = FindViewById<EditText>(Resource.Id.addplayerEdittext);
            Button addplayer = FindViewById<Button>(Resource.Id.addplayerbutton);
            addplayer.Click += (sender, o) => {
                if(playername.Text.Length<1)
                {
                    Toast.MakeText(this, "Enter name", ToastLength.Long).Show();
                }
                else
                {
                    game.CreatePlayer(playername.Text);
                    SetContentView(Resource.Layout.GameScreen);
                    UpgradePlayerInfo();
                    setLayoutItems();
                  //  StartGame();
                }
            };
            }

        private void Raisebutton_Click(object sender, EventArgs e)
        {
            // podbijanie i aktualizacja danych
            game.Raise(Bidbar.Progress);
            CheckRound();
        }



        private void Bidbar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            Bidbar.Progress = ((Bidbar.Progress / 10) * 10);
            Bidtext.Text = "Bid: " + Bidbar.Progress;
            ChangeBidBar();
        }

        private void Pass_Click(object sender, EventArgs e)
        {
            game.PlayerPass();
            CheckRound();

        }

        private void Check_Click(object sender, EventArgs e)
        {
            CheckRound();
        }
        private void Call_Click(object sender, EventArgs e)
        {
            game.Call(game.actualbid);
            CheckRound();
        }

        public void UpgradePlayerInfo()
        {
            lv.Adapter = new PlayerAdapter(game.playerlist, this);
            activPlayer.Text = game.actualPlayer.Name + "\r\n" + "Tokens " + game.actualPlayer.Cash + "\r\n";
            FlopRound.Text = game.flop.ToString();
            actualblindtv.Text = "Blinds (" + game.smallblind + "," + game.smallblind * 2 + ")";
            roundpottv.Text = "Round Pot: " + game.actualbid;
            ChangeBidBar();
            CheckButtons();

        }

        private void CheckButtons()
        {
            double full = game.actualPlayer.fullpot;
            if (game.actualPlayer.roundbid == game.actualbid)
            {
                Raisebutton.Enabled = true;
                Call.Enabled = false;
                Check.Enabled = true;
            }
            else if (full < game.actualbid)
            {
               
                Raisebutton.Enabled = false;
                Call.Enabled = true;
                Check.Enabled = false;
            }
            else
            {
                Raisebutton.Enabled = true;
                Check.Enabled = false;
                Call.Enabled = true;
                
            }


            pass.Enabled = true;
            //pass.Clickable = true;
        }


        public void setWinnerLayout()
        {

            SetContentView(Resource.Layout.FlopLayout);
            Stack<int> kolejnoscmiejsc = new Stack<int>();
            TextView cashtowintv = FindViewById<TextView>(Resource.Id.winnertext);
            ListView winerlist = FindViewById<ListView>(Resource.Id.winnerlistview);
            Button nextroundbutton = FindViewById<Button>(Resource.Id.nextroundbutton);
            CheckBox choosenadapter = FindViewById<CheckBox>(Resource.Id.chooseadapter);
            WinnerListAdapterseekbar adapterseekbar = new WinnerListAdapterseekbar(game.playerlist, this,game);
            WinnerListAdapter adapter = new WinnerListAdapter(game.playerlist, this);
            winerlist.Adapter = adapter;
            Game.getglobalcashtowin = game.fullpot;

            nextroundbutton.Click += delegate {

                Givemoney(choosenadapter);
            };

            choosenadapter.Click += (o, e) => {
                if(choosenadapter.Checked)
                {
                    if (kolejnoscmiejsc.Count == 0)
                    {
                        winerlist.Adapter = adapterseekbar;
                  
                    }
                    else choosenadapter.Checked = false;
                }
                else if(!choosenadapter.Checked)
                {
                    if (Game.getglobalcashtowin == game.fullpot)
                    {
                        winerlist.Adapter = adapter;          
                    }
                    else choosenadapter.Checked = true;
                }
                
                
            };




                winerlist.ItemClick += (o, e) =>
                {
                    if (e.View.Enabled)
                    {
                        if (Game.getglobalcashtowin > 0)
                        {
                            Player p = game.playerlist[e.Position];
                            double winingcash = p.maxWin - (-p.winnercash);
                          
                            if (winingcash > 0)
                            {
                                kolejnoscmiejsc.Push(e.Position);
                                p.winnercash = winingcash;
                                p.Cash += winingcash;
                                e.View.SetBackgroundColor(Android.Graphics.Color.DarkGreen);
                                //e.View.Enabled = false;
                                adapter.GetView(e.Position, e.View, e.Parent).Enabled = false;
                                Game.getglobalcashtowin -= p.winnercash;

                                for (int i = 0; i < winerlist.Count; i++)
                                {
                                    Player player = game.playerlist[i];
                                    if (winerlist.GetChildAt(i).Enabled)
                                    {
                                        player.winnercash -= p.winnercash;
                                    }


                                }

                            }


                        }
                    }
                    else
                    {
                        if (e.Position == kolejnoscmiejsc.Peek())
                        {
                            Player p = game.playerlist[e.Position];
                            kolejnoscmiejsc.Pop();

                            Game.getglobalcashtowin += game.playerlist.ElementAt(e.Position).winnercash;


                            e.View.SetBackgroundColor(Android.Graphics.Color.Transparent);

                            for (int i = 0; i < winerlist.Count; i++)
                            {
                                Player player = game.playerlist[i];
                                {
                                    if (winerlist.GetChildAt(i).Enabled)
                                        player.winnercash += p.winnercash;

                                }

                            }
                            adapter.GetView(e.Position, e.View, e.Parent).Enabled = true;
                            p.Cash -= p.winnercash;
                            p.winnercash = p.winnercash - p.maxWin;

                        }
                    }
                    playerswiners(e.View, winerlist, adapter);


                    cashtowintv.Text = "Cash to win " + Game.getglobalcashtowin;

                    if (Game.getglobalcashtowin == 0)
                    {
                        nextroundbutton.Enabled = true;
                    }
                    else nextroundbutton.Enabled = false;
                };

          

            


        }

     
        private void playerswiners (View v, ListView view,WinnerListAdapter win){
            
            for (int i = 0; i < game.playerlist.Count; i++)
            {
                
                
                Player player = game.playerlist[i];
                double finalcash = -player.winnercash - player.maxWin;

                View ViewChild= view.GetChildAt(i);
                if (!ViewChild.Enabled)
                    win.GetView(i, ViewChild, view).FindViewById<TextView>(Resource.Id.playercashtowintext).Text = player.Name + " win " + player.winnercash + " tokens" ;
                else
                    if (player.winnercash < -player.maxWin)
                    win.GetView(i, ViewChild, view).FindViewById<TextView>(Resource.Id.playercashtowintext).Text = player.Name + " can win " + 0;
                else
                    win.GetView(i, ViewChild, view).FindViewById<TextView>(Resource.Id.playercashtowintext).Text = player.Name + " can win " + (finalcash * -1);
               }
        }

        private void Givemoney(CheckBox check)
        {
            if (check.Checked)
            {
                foreach (Player p in game.playerlist)
                    p.Cash += p.winnercash;
            }
            if (game.checkplayersCount())
            {
                game.startNextRound();
                game.Resetwinnercash();
                SetContentView(Resource.Layout.GameScreen);
                setLayoutItems();
                StartGame();
            }

        }

        private void StartGame()
        {
            
            UpgradePlayerInfo();

            for (int i = 0; i < lv.Count; i++)
            {
                lv.SetItemChecked(i, true);
            }
            

            Bidtext.Text = "Bid: " + game.actualbid * 2;
            Bidbar.Max =(int) game.actualPlayer.Cash ;
            Bidbar.Progress = (int)game.actualbid * 2;
            Bidbar.ProgressChanged += Bidbar_ProgressChanged;
         }

       

        private void CheckRound()
        {
            //nastepny gracz
            if (game.NextPlayer() != 4)
            {
                UpgradePlayerInfo();               
                // jezeli jest koniec zmiana layoutu               
            }
            else { setWinnerLayout(); }
        }

        private void ChangeBidBar()
        {
            int fullpot =(int)game.actualPlayer.fullpot;
            if (fullpot< game.actualbid + 10)
                Bidbar.Progress = fullpot;
            else if (Bidbar.Progress < game.actualbid + 10)
                Bidbar.Progress = (int)game.actualbid + 10;

            Bidbar.Max = fullpot;

        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            mDrawerToggle.SyncState();

        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (mDrawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

    }
}