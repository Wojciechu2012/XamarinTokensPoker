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
    class WinnerListAdapter : BaseAdapter<Player>
    {
        List<Player> playerslist;
        Activity context;

        public WinnerListAdapter(List<Player> playerslist, Activity context)
        {
            this.playerslist = playerslist;
            this.context = context;
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
                v = context.LayoutInflater.Inflate(Resource.Layout.WinnerView, null);

            v.FindViewById<TextView>(Resource.Id.playercashtowintext).Text = item.Name + " will win " +item.winnercash + "                " + item.maxWin + "              " + item.Cash;

            if (item.IsPlay)
                v.Visibility = ViewStates.Visible;
            else v.Visibility = ViewStates.Invisible;
          
           
            return v;

        }


      
    }

    

    
  }