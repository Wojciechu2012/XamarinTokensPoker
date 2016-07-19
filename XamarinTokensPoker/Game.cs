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
    class Game
    {
        
        private int _countpassplayers { get; set; }

        public long _startcash { get; private set; }
        public int actualPlayernumber { get;private set; }
        public double smallblind { get; private set; }

        private double bigblind { get; set; }

        public double actualbid { get;private set; }

        private int lastbetplayer { get; set; }
        private int startplayer { get; set; }
        private int Round { get; set; }       
        private int roundraiseblind { get; set; }
        public Flops flop = Flops.PreFlop;


        public Player actualPlayer { get; private set; }


        public double roundpot { get; set; }
        public double fullpot { get; set; }

        public static double getglobalcashtowin;
       
        // public List<int> bidlist = new List<int>();
        //  public List<int> potlist = new List<int>();
        public List<Player> playerlist = new List<Player>();
        Activity activity;

        public Game(long startcash,long smallblind,Activity activity,int roundraiseblind)
        {
            this.roundraiseblind = roundraiseblind;
            Round = 0;
            this.activity = activity;
            startplayer = 0;
            _startcash = startcash;
            this.smallblind = smallblind;
            actualbid = bigblind;
        }

        public bool checkplayersCount()
        {
            CheckLoosers();
            if (playerlist.Count == 1)
            {
                Intent intent = new Intent(activity, typeof(ShowWinner));
                intent.PutExtra("thewinner", playerlist[0].Name);
                activity.StartActivity(intent);
                return false;
            }
            return true;
        }
        public void startNextRound()
        {
            
             checkplayersCount();
              roundpot = 0;
              fullpot = 0;
            
            RestartPlayers();
               _countpassplayers = 0;
                flop = 0;

            Round++;
            if (Round == roundraiseblind)
            {
                smallblind *= 2;
                Round = 0;
            }
            bigblind = smallblind * 2;

                actualPlayernumber = startplayer;
                actualPlayer = playerlist.ElementAt(startplayer);
                 
                Raise(smallblind);
                NextPlayer();
                Raise(bigblind);
                NextPlayer();
                if (actualPlayernumber < playerlist.Count - 1)
                lastbetplayer = actualPlayernumber + 1;
                else
                lastbetplayer = 0;

            if (startplayer < playerlist.Count-1 )
                {
                    startplayer += 1;
                }
                else { startplayer = 0; }
        }

        public void CreatePlayer(string name)
        {
            Player player = new Player(name, _startcash);
            playerlist.Add(player);
            if (flop != Flops.PreFlop)
                playerlist[playerlist.Count-1].IsPlay = false;
            else playerlist[playerlist.Count-1].IsPlay = true;
        }
               
        public List<string> PlayerToString()
        {
            List<string> stringlist = new List<string>();
            foreach(Player p in playerlist)
            {
              stringlist.Add(p.ToString());
            }
            actualPlayer = playerlist.ElementAt(actualPlayernumber);
            return stringlist;
            }

        private void RestartPlayers()
        {
            foreach (Player p in playerlist)
            {
                p.IsPlay = true;
                p.roundbid = 0;
                p.maxWin = 0;
            }
        }

        public int NextPlayer()
        {               
            do
            {
                if (!(playerlist.Exists(x =>x.IsPlay&&x.Cash>0))){
                    NextFlop();
                    return 4;
                }
                    
                if (actualPlayernumber < playerlist.Count - 1)
                    actualPlayernumber++;
                else actualPlayernumber = 0;            
            actualPlayer = playerlist.ElementAt(actualPlayernumber);
                if (playerlist.ElementAt(lastbetplayer) == actualPlayer && actualPlayer.roundbid == actualbid)
                {
                    NextFlop();
                }
            } while (!actualPlayer.IsPlay||actualPlayer.Cash==0);

            if(_countpassplayers == playerlist.Count-1)
            {
                actualPlayer.Cash += roundpot + (int)actualPlayer.maxWin;
                startNextRound();
            }
            return (int)flop;
        }
        
        public void Raise(double bid)
        {           
            actualPlayer.Raise(bid);
            actualbid = bid;            
            lastbetplayer = actualPlayernumber;
                    
        }
        
        public void Call(double bid)
        {
            if (actualPlayer.fullpot >= bid)
                actualPlayer.Raise(bid);
            else actualPlayer.Raise(actualPlayer.Cash+actualPlayer.roundbid);          
        }
   
      public void PlayerPass()
        {
            actualPlayer.IsPlay = false;
            _countpassplayers++;
        }


        private void NextFlop()
        {
            flop = flop + 1;
            AllRoundMoney();
            fullpot += roundpot;
            foreach (Player p in playerlist)
            {                               
                if (p.IsPlay)
                {
                    foreach(Player p2 in playerlist)
                    if (p.roundbid >= p2.roundbid)
                    {
                            p.maxWin += p2.roundbid;
                    }
                    else p.maxWin += p.roundbid;
                }
                               
            }
            ResetBid();

            actualbid = 0;
            roundpot = 0;

        }
        private void CheckLoosers()
        {
            
            for (int i = 0; i < playerlist.Count; i++)
            {
                if (playerlist[i].Cash <= 0) {
                    if (i <= startplayer & startplayer > 0)
                        startplayer--;
                    playerlist.RemoveAt(i);
                    i--;
            }

            }
        }
        private void AllRoundMoney()
        {
            foreach (Player p in playerlist)
                roundpot += p.roundbid;
        }


        private void ResetBid()
        {
            foreach (Player p in playerlist)
                p.roundbid = 0;
    }
        public void Resetwinnercash()
        {
            foreach(Player p in playerlist)
                p.winnercash = 0;
        }


    }

    

}