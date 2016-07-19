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
using Java.Lang;

namespace XamarinTokensPoker
{
    class PlayerAdapter : BaseAdapter<Player>
    {

        List<Player> playerslist;
        Activity context;
        
        public PlayerAdapter(List<Player> playerslist, Activity context)
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

            if (v == null)
                v = context.LayoutInflater.Inflate(Resource.Layout.CustomViewaxml, null);

            v.FindViewById<TextView>(Resource.Id.Text1).Text = item.ToString();
            
            if (!item.IsPlay)
                v.Visibility = ViewStates.Gone;
            else
                v.Visibility = ViewStates.Visible;

            return v;
        }

     
    }
}