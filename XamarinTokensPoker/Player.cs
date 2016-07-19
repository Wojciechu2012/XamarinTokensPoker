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
    class Player
    {

        private double _cash;
        public double Cash { get { return _cash; } set { _cash = value; } }
        private string _name;
        public string Name { get { return _name; } }
        private bool _isPlay = true;

        public bool IsPlay { get { return _isPlay; } set { _isPlay = value; } }

        public double roundbid { get; set; }
        public double maxWin { get; set; }
        public double winnercash  { get; set; }
        public double fullpot { get { return roundbid + Cash; } }

        public Player(string name, double cash)
        {
            _cash = cash;
            _name = name;
            
        }


        public void Raise(double bid)
        {
            if (roundbid<bid) {
                if (bid > fullpot)
                {
                    roundbid = Cash+roundbid;
                    Cash = 0;
                    return;
                }
                double cash = bid - roundbid;
                Cash -= cash;
                roundbid = bid;
            }
            
        }
        public void Call(double bid)
        {
            double cash = bid - roundbid;
            Cash -= cash;
            roundbid = bid;
        }

        public override string ToString()
        {
            return Name + "  " + Cash + " tokens" + "\r\nround bid = " + roundbid + "\r\n max win " + maxWin.ToString() ;
        }

    }
}