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
    class ActionBarAdapter : BaseAdapter<string>
    {
        Activity activity;
        List<string> actions;

        public ActionBarAdapter(List<string> actions,Activity activity)
        {
            this.activity = activity;
            this.actions = actions;
        }



        public override string this[int position]
        {
            get
            {
                return actions[position];
            }
        }

        public override int Count
        {
            get
            {
                return actions.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var item = actions[position];
            View v = convertView;
            if (v == null)
                v = activity.LayoutInflater.Inflate(Resource.Layout.WinnerView,null);

            v.FindViewById<TextView>(Resource.Id.playercashtowintext).Text = item;


            return v;

        }
    }
}