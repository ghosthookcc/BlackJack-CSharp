 using System;
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
        private string cardtype; 
        private int bestvalue;
        private SuitType suittype;
        private int blackjackvalue;

        public string CardType
        {
            set { cardtype = value; }
            get { return cardtype; }
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

        public Card(string value, SuitType suit)
        {
            this.CardType = value;
            this.Suit = suit;
        }

        public override string ToString()
        {
            return (cardtype + suittype.ToString());
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
            (string CardType, int CardValue)[] cardInfo = new (string, int)[]
            {
                ("2", 2),
                ("3", 3),
                ("4", 4),
                ("5", 5),
                ("6", 6),
                ("7", 7),
                ("8", 8),
                ("9", 9),
                ("10", 10),
                ("Jack", 10),
                ("Queen", 10),
                ("King", 10),
                ("A", 11)
            };
            cards.Clear();

            for (int i = 1; i <= nrofdecks; i++)
            {
                for (int suit = 0; suit < 4; suit++)
                {
                    for (int value = 0; value < 13; value++)
                    {
                        currcard = new Card(cardInfo[value].CardType, (SuitType)suit);
                        currcard.BlackJackValue = cardInfo[value].CardValue;
                        cards.Add(currcard);
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
                }
                else
                {
                    for (int i = 0; i < amount; i++)
                    {
                        players[CurrPlayerIndex].Hand.Add(Deck.Draw());
                    }
                }
            }
            else
            {
                Deck.ResetAndShuffle();
            }
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

            Game game = new Game(playerCount : 2);
            game.StartGame(TotalDecks);

            timed.Stop();
            TimeSpan timexe = timed.Elapsed;
        
            Console.WriteLine("Elapsed: {0}ms", timexe.TotalMilliseconds);
        }
    }
}