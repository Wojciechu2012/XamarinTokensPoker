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
    class WinnerListAdapterseekbar : BaseAdapter<Player>
    {
        List<Player> playerslist;
        Activity context;
        Game game;
        public WinnerListAdapterseekbar(List<Player> playerslist, Activity context,Game game)
        {
            this.playerslist = playerslist;
            this.context = context;
            this.game = game;
        }
        public override int Count
        {
            get
            {
                return playerslist.Count;
            }
        }

        public override Player this[int position]
        {
            get
            {
                return playerslist[position];
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = playerslist[position];
            View v = convertView;

            string playercashwin = item.Name + " wygra " + item.maxWin;
            string winner = ", " + item.Name + " wygra " + item.maxWin;


            if (v == null)
            {
                v = context.LayoutInflater.Inflate(Resource.Layout.Choosewinnerseekbar, null);

                v.FindViewById<TextView>(Resource.Id.playercashtowintextseekbar).Text = item.Name + " wygra " + item.winnercash + "                " + item.maxWin + "              " + item.Cash;
                SeekBar cashtowin = v.FindViewById<SeekBar>(Resource.Id.playercashtowinseekbar);


                cashtowin.ProgressChanged += (sender, e) =>
                 {
                     cashtowin.Progress = (cashtowin.Progress / 10) * 10;
                  
                         Game.getglobalcashtowin = game.fullpot;

                         item.winnercash = cashtowin.Progress;

                         

                         foreach (Player p in playerslist)
                         {
                             Game.getglobalcashtowin -= p.winnercash;
                         }

                         if (item.maxWin > Game.getglobalcashtowin+item.winnercash)
                             cashtowin.Max = (int)Game.getglobalcashtowin+(int)item.winnercash;
                         else cashtowin.Max = (int)item.maxWin;

                         v.FindViewById<TextView>(Resource.Id.playercashtowintextseekbar).Text = item.Name + " wygra " + item.winnercash + "                " + item.maxWin + "              " + item.Cash;

                         context.FindViewById<TextView>(Resource.Id.winnertext).Text = " " + Game.getglobalcashtowin;
                         Button nextroundbutton = context.FindViewById<Button>(Resource.Id.nextroundbutton);
                         if (Game.getglobalcashtowin == 0)
                         {
                             nextroundbutton.Enabled = true;
                         }
                         else nextroundbutton.Enabled = false;
                     
                 };
            }

            if (item.IsPlay)
                v.Visibility = ViewStates.Visible;
            else v.Visibility = ViewStates.Invisible;


            return v;

        }

     
    }
    }