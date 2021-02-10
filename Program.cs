﻿ using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Blackjack
{
    public enum GameStatus
    {
        Won = 1,
        Lost = 2,
        Playing = 3,
        Tie = 4,
        BlackJack = 5
    }

    public enum SuitType
    {
        Club,
        Diamond,
        Heart,
        Spade
    }

    public class Card
    {
        private int cardvalue;
        private SuitType suittype;
        private int blackjackvalue;

        public int CardValue
        {
            set { cardvalue = value; }
            get { return cardvalue; }
        }

        public SuitType Suit
        {
            set { suittype = value; }
            get { return suittype; }
        }

        public int BlackJackValue
        {
            set { blackjackvalue = value; }
            get { return blackjackvalue; }
        }

        public Card(int value, SuitType suit)
        {
            this.CardValue = value;
            this.Suit = suit;
        }

        public override string ToString()
        {
            return (cardvalue + suittype.ToString());
        }
    }

    public class Deck
    {
        private Random rand = new Random();
        private int nrofdecks;
        private List<Card> cards = new List<Card> { };
        private Card currcard;

        public Deck(int decks)
        {
            this.nrofdecks = decks;
        }

        public int cardsleft
        {
            get { return cards.Count; }
        }

        public void Shuffle()
        {
            int randval;
            for (int i = 0; i < cards.Count; i++)
            {
                randval = rand.Next(0, cards.Count - 1);
        
                currcard = cards[i];
                cards[i] = cards[randval];
                cards[randval] = currcard;
            }
        }

        public void ResetAndShuffle()
        {
            cards.Clear();
    
            for (int i = 1; i <= nrofdecks; i++)
            {
                for (int suit = 0; suit < 4; suit++)
                {
                    for (int value = 1; value <= 13; value++)
                    {
                        cards.Add(new Card(value, (SuitType)suit));
                    }
                }
            }
    
            Shuffle();
        }

        public Card Draw()
        {
            currcard = cards[0];
            cards.RemoveAt(0);
    
            return currcard;
        }
    }

    public class Controller
    {
        protected Card LastDrawnCard;

        protected int LowValue;
        protected int HighValue;
        protected int BestValue;

        public void Reset(List<Card> Hand)
        {
            Hand.Clear();
        }
    }

    public class Player : Controller
    {
        public List<Card> Hand = new List<Card> { };

        private string playername;

        public Player(string name)
        {
            this.playername = name;
        }

        public string PlayerName
        {
            get { return playername; }
        }

        public int handsize
        {
            get { return Hand.Count; }
        }
    }

    public class Dealer : Controller
    {
        public List<Card> Hand = new List<Card> { };

        private string dealername;

        public Dealer(string name)
        {
            this.dealername = name;
        }

        public string DealerName
        {
            get { return dealername; }
        }

        public int handsize
        {
            get { return Hand.Count; }
        }
    }

    public class Game
    {
        public int CurrPlayerIndex = 0;

        public Player[] players;
        public Dealer Dealer = new Dealer("dealer");
        public Deck Deck;
        public GameStatus GameStatus;

        private Player player;

        public Game(int playerCount)
        {
            players = new Player[playerCount];
            for (int i = 0; i < playerCount; i++)
            { 
                players[i] = new Player("player" + (i + 1).ToString());
            }
        }

        public Player Player
        {
            get { return player; }
        }

        public void StartGame(int TotalDecks)
        {
            foreach (Player player in players)
            {
                player.Reset(player.Hand);
            }
            Dealer.Reset(Dealer.Hand);
    
            this.Deck = new Deck(TotalDecks);
            this.Deck.ResetAndShuffle();

            player = players[0];
        }

        public void Draw(bool isdealer, Player player, int amount)
        {
            if (Deck.cardsleft > 0)
            {
                if (isdealer)
                {
                    for (int i = 0; i < amount; i++)
                    {
                        Dealer.Hand.Add(Deck.Draw());
                    }
            
                    return;
                }
                else
                {

                    for (int i = 0; i < amount; i++)
                    {
                        players[CurrPlayerIndex].Hand.Add(Deck.Draw());
                    }
                    
            
                    return;
                }
            }
    
            Deck.ResetAndShuffle();
        }

        public void NextPlayer()
        {
            if (CurrPlayerIndex == players.Length - 1)
            {
                CurrPlayerIndex = 0;
            }
            else
            {
                CurrPlayerIndex += 1;
            }

            player = players[CurrPlayerIndex];
        }
    }

    class Program
    {
        static void Main()
        {
            Stopwatch timed = new Stopwatch();
            timed.Start();
        
            int TotalDecks = 6;
        
            Game game = new Game(playerCount: 2);
            game.StartGame(TotalDecks);

            game.Draw(isdealer : false, game.Player, 1);
            Console.WriteLine("{0} has {1} cards in hand", game.Player.PlayerName, game.Player.handsize);
            game.Draw(isdealer : false, game.Player, 2);
            Console.WriteLine("{0} has {1} cards in hand", game.Player.PlayerName, game.Player.handsize);

            game.NextPlayer();

            game.Draw(isdealer: false, game.Player, 2);
            Console.WriteLine("{0} has {1} cards in hand", game.Player.PlayerName, game.Player.handsize);
            game.Draw(isdealer: false, game.Player, 5);
            Console.WriteLine("{0} has {1} cards in hand", game.Player.PlayerName, game.Player.handsize);

            timed.Stop();
            TimeSpan timexe = timed.Elapsed;
        
            Console.WriteLine("Elapsed: {0}ms", timexe.TotalMilliseconds);
        }
    }
}