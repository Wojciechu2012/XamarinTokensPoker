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
    class CreatePlayerAdapter : BaseAdapter<string>
    {
        List<string> names;
        Activity context;
        public CreatePlayerAdapter(List<string> names,Activity context)
        {
            this.names = names;
            this.context = context;
        }
        public override string this[int position]
        {
            get
            {
              return names[position];
            }
        }

        public override int Count
        {
            get
            {
                return names.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View v = convertView;
            if (convertView == null)
            {
                v = context.LayoutInflater.Inflate(Resource.Layout.CreatePlayerLayAdapter,null);
            }
            TextView tv = v.FindViewById<TextView>(Resource.Id.textView1);
            EditText name = v.FindViewById<EditText>(Resource.Id.EnterName);

            name.TextChanged += (e,o)=> {
            name.SetTextColor(Android.Graphics.Color.Black);
            };

            tv.Text = "Player " + (position+1);

            return v;

        }

       
    }
}