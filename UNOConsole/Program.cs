// OOP 4200 Final Project - UNO Card Game
// Matthew Ware - 100472787
// March 27 2022
using System;
using System.Collections.Generic;

namespace UNOConsole
{
    /// <summary>
    /// Main Program Class containing Main Function
    /// </summary>
    class Program
    {
        // Main function
        static void Main(string[] args)
        {
            // Initialize the game, deck and discard pile, and player list
            Game theGame = new Game();
            Game.thisDeck.theDeck = Deck.initializeDeck();
            Deck drawPile = new Deck(Game.thisDeck);
            Game.ThePile = drawPile;

            Game.thisDeck.printDeck();
            drawPile.printDeck();

            Console.WriteLine("Deck size is " + drawPile.DeckSize);
            //Console.WriteLine(drawPile.ToString());
            Console.ReadKey();

            Deck.shuffleDeck();
            // theGame.drawPile = mainDeck;
            Game.thisDeck.printDeck();
            drawPile.printDeck();

            Deck.shufflePile();
            Game.thisDeck.printDeck();
            drawPile.printDeck();


            // Show upon loading 
            Console.WriteLine("Welcome to UNO");
            Console.ReadKey();

            // Assign initial dealer
            theGame.DealerIndex = theGame.setDealer(theGame.ClockwiseTurns);
            Console.WriteLine(theGame.CurrentDealer.Name);
            Console.ReadKey();

            // Run the main gaim loop through recurrsion
            theGame.takeTurn(drawPile);
        }
    }

    /// <summary>
    /// Game Class to hold the static deck and discard pile and track dealer and current player
    /// </summary>
    class Game
    {
        // Hold values for the current player and the current dealer, the turn over and turn direction
        private Player currentPlayer;
        private Player currentDealer;
        private int dealerIndex;
        private int playerIndex = 0;
        private bool turnOver = false;
        private bool clockwiseTurns = true;
        private bool firstDealer = true;
        public static Card topCard;
        public static Deck thisDeck = new Deck();
        public static Deck discardPile = new Deck();
        public static Deck thePile = new Deck();
        public static List<Player> playerList = new List<Player>();

        // Properties for above fields
        public Player CurrentPlayer
        {
            get
            {
                return currentPlayer;
            }
            set
            {
                currentPlayer = value;
            }
        }
        public Player CurrentDealer
        {
            get
            {
                return currentDealer;
            }
            set
            {
                currentDealer = value;
            }
        }
        public bool TurnOver
        {
            get
            {
                return turnOver;
            }
            set
            {
                turnOver = value;
            }
        }
        public bool ClockwiseTurns
        {
            get
            {
                return clockwiseTurns;
            }
            set
            {
                clockwiseTurns = value;
            }
        }
        public int DealerIndex
        {
            get
            {
                return dealerIndex;
            }
            set
            {
                dealerIndex = value;
            }
        }
        public int PlayerIndex
        {
            get
            {
                return playerIndex;
            }
            set
            {
                playerIndex = value;
            }
        }
        public static Card TopCard
        {
            get
            {
                return topCard;
            }
            set
            {
                topCard = value;
            }
        }

        public static Deck ThePile
        {
            get
            {
                return thePile;
            }
            set
            {
                thePile = value;
            }
        }
        public static Deck ThisDeck
        {
            get
            {
                return thisDeck;
            }
            set
            {
                thisDeck = value;
            }
        }
        // Default Constructor
        public Game()
        {
            // Set up the Game Object
            initializeGame();
        }

        // Run all functions needed to set up a game
        public void initializeGame()
        {
            // Set up players for the game
            playerList = initializePlayers();
            CurrentDealer = playerList[setDealer(true)];
            CurrentPlayer = playerList[PlayerIndex];
        }

        // Return a List of Players after adding a human player and three computer players
        public List<Player> initializePlayers()
        {
            // Generate list and add the human player
            List<Player> playerList = new List<Player>();
            HumanPlayer user = new HumanPlayer();
            user.Name = "Player";
            playerList.Add(user);

            // Add three computer players to the player list
            AIPlayer computer1 = new AIPlayer();
            computer1.Name = "Computer 1";
            AIPlayer computer2 = new AIPlayer();
            computer2.Name = "Computer 2";
            AIPlayer computer3 = new AIPlayer();
            computer3.Name = "Computer 3";
            playerList.Add(computer1);
            playerList.Add(computer2);
            playerList.Add(computer3);

            // Print players
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine(playerList[i].ToString());
            }

            // Return the list of all 4 players
            return playerList;
        }

        // Returns the player who is now the dealer from the list provided and direction of turns
        public int setDealer(bool clockwiseTurns)
        {
            // Check if the currentPlayer is unassigned and assign the dealer to a random player
            if (firstDealer)
            {
                Random randomPlayer = new Random();
                DealerIndex = randomPlayer.Next(4);
                firstDealer = false;
                if (DealerIndex == 3)
                {
                    PlayerIndex = 0;
                }
                else PlayerIndex = DealerIndex + 1;
            }
            else
            {
                if (clockwiseTurns)
                {
                    DealerIndex++;
                    if (DealerIndex > 3) DealerIndex = 0;
                }
                else
                {
                    DealerIndex--;
                    if (DealerIndex < 0) DealerIndex = 3;
                }
            }
            // Return the player who is the current dealer
            return DealerIndex; 
        }

        // Main Game Loop
        public void takeTurn(Deck sourceCards)
        {
            //Main Game Loop that continues through recurrsion
            // if round over
            if (TurnOver)
            {
                // determine round winner
                int winnerScore = 0;
                int winnerIndex = 0;
                // calculate score
                for(int i = 0; i < 4; i++)
                {
                    int roundScore = 0;
                    for ( int j = 0; j < playerList[i].ThePlayerHand.Count; j++)
                    {
                        roundScore += playerList[i].ThePlayerHand[j].CardValue;
                    }
                    if (playerList[i].ThePlayerHand.Count == 0) winnerIndex = i;
                    winnerScore += roundScore;
                }

                playerList[winnerIndex].Score += winnerScore;
                // if score > 500
                if (playerList[winnerIndex].Score >= 500)
                {
                    Console.WriteLine("GAME WINNER IS " + playerList[winnerIndex].Name);
                    Console.ReadKey();
                    EndGame(thisDeck);
                }
                else
                {
                    // determine winner
                    Console.WriteLine("The Winner is " + playerList[winnerIndex].Name);
                    Console.ReadKey();
                    // end game
                }// else Deal Hand
                DealerIndex = setDealer(ClockwiseTurns);
                PlayerIndex = pickNextPlayer(ClockwiseTurns);
                //CurrentDealer.dealHand();
                TurnOver = false;
                takeTurn(sourceCards);
            // assign next player
            }// else Next Player takes turn
            else
            {
                DealerIndex = setDealer(ClockwiseTurns);
                PlayerIndex = pickNextPlayer(ClockwiseTurns);
                TurnOver = false;
                playerList[DealerIndex].dealHand(sourceCards);

                for (int index = 0; index < 4; index++)
                {
                    playerList[index].displayPlayerHand();
                    Console.WriteLine("");
                }

                // Continue to take turns until a player wins the round and TurnOver becomes true
                playRound(sourceCards);
            }
            // turns continue until round ends ie end with call to takeTurn();
            if (Deck.deckIndex >= 0) takeTurn(sourceCards);
        }

        // Main interaction of Players
        public void playRound(Deck drawPile)
        {
            int lastIndex;
            // continue until current player has 0 cards in hand
            do
            {
                playerList[PlayerIndex].selectCard();
                playerList[PlayerIndex].displayPlayerHand();
                lastIndex = PlayerIndex;
                PlayerIndex = pickNextPlayer(clockwiseTurns);

            } while (playerList[lastIndex].ThePlayerHand.Count > 0 && Deck.pileIndex >= 0);

            if(Deck.pileIndex < 0)
            {
                //Deck.resetDeck();
                Deck.pileIndex = drawPile.DeckSize - 1;
                playRound(drawPile);
            }

            if(playerList[lastIndex].ThePlayerHand.Count <= 0)
            {
                TurnOver = true;
                takeTurn(drawPile);
            }
            drawPile.printDeck();
        }

        // Returns the player who is next in line to play due to last card played or in an order
        public int pickNextPlayer(bool clockwiseTurns)
        {
            // Determine who is next to be the current player first in a clockwise direction
            if(clockwiseTurns)
            {
                playerIndex++;
                if (playerIndex > 3) playerIndex = 0;
            }
            // Counter-Clockwise direction
            else
            {
                playerIndex--;
                if (playerIndex < 0) playerIndex = 3;
            }
            // Return the next player to take a turn
            return playerIndex;
        }

        public void EndGame(Deck mainDeck)
        {
            string playAgain;
            do
            {
                Console.WriteLine("Do you want to play again? ");
                playAgain = Console.ReadLine();
            } while (playAgain != "y" && playAgain != "Y" && playAgain != "n" && playAgain != "N");
            
            if (playAgain == "y" || playAgain == "Y")
            {
                //ResetGame();
                for(int i = 0; i < 4; i++)
                {
                    playerList[i].Score = 0;
                    playerList[i].ThePlayerHand.Clear();
                }
                discardPile.theDeck.Clear();
                thePile.theDeck.Clear();
                firstDealer = true;
                thisDeck.theDeck.Clear();

                initializeGame();

                Deck.deckIndex = thePile.DeckSize;
                DealerIndex = setDealer(ClockwiseTurns);
                Console.WriteLine(CurrentDealer.Name);
                //takeTurn();
                Console.ReadKey();

            }
            else
            {
                Console.Clear();
                Console.WriteLine("THANKS FOR PLAYING -- GOODBYE");
                Console.ReadKey();
                // go to end screen;
            }
        }
    }
 
    /// <summary>
    /// Card class to hold the colour and card value as well as if it is a special card
    /// </summary>
    class Card
    {
        // Private fields to hold special status, colour and value of a card
        private bool special;
        private string colour;
        private int cardValue;

        // Properties to get and set the above fields
        public bool Special
        {
            get
            {
                return special;
            }
            set
            {
                special = value;
            }
        }
        public string Colour
        {
            get
            {
                return colour;
            }
            set
            {
                colour = value;
            }
        }
        public int CardValue 
        { 
            get 
            {
                return cardValue;
            } 
            set
            {
                cardValue = value;
            }
        }

        // Default Card Constructor
        public Card()
        {
        }

        // Function that assigns special status, colour and card value to an individual card
        public void assignCard(bool special, string colour, int cardValue)
        {
            Special = special;
            Colour = colour;
            CardValue = cardValue;
        }

        // Display a card in the console
        public override string ToString()
        {
            return "Colour is " + Colour +", Value is " + CardValue;
        }
    }

    /// <summary>
    /// Deck class that is a collection of card objects with methods
    /// </summary>
    class Deck
    {
        // Single static deck list of cards for play
        public List<Card> theDeck = new List<Card>();
        public static int deckIndex = 107;
        public static int pileIndex = 107;
        private int deckSize;

        public int DeckSize
        {
            get
            {
                return theDeck.Count;
            }
            set
            {
                deckSize = value;
            }
        }
        // Default constructor for the Deck
        public Deck()
        {
            //initializeDeck();
            //deckIndex = theDeck.Count;
            //Console.WriteLine("The Deck Size is " + deckIndex);
            //Console.ReadKey();
        }

        public Deck(Deck deck)
        {
            for(int i = 0; i < deck.DeckSize; i++)
            {
                theDeck.Add(deck.theDeck[i]);
            }
        }

        // Create 108 black cards and assign values to each card
        public static List<Card> initializeDeck()
        {
            List<Card> theDeck = new List<Card>();
            for (int i = 0; i < 108; i++)
            {
                Card aCard = new Card();
                theDeck.Add(aCard);
            }
            // Assign the red cards
            // Assign the first card with a zero value
            theDeck[0].assignCard(false, "red", 0);
            // Assign the next 18 cards with values from 1 to 9 with two of each
            for (int i = 1; i <= 9; i++)
            {
                theDeck[(2 * i) - 1].assignCard(false, "red", i);
                theDeck[2 * i].assignCard(false, "red", i);
            }
            theDeck[19].assignCard(true, "red", 10);
            theDeck[20].assignCard(true, "red", 10);
            theDeck[21].assignCard(true, "red", 11);
            theDeck[22].assignCard(true, "red", 11);
            theDeck[23].assignCard(true, "red", 12);
            theDeck[24].assignCard(true, "red", 12);

            // Assign the green cards
            // Assing the first green card a value of zero
            theDeck[25].assignCard(false, "green", 0);
            // Assign the next 18 cards with values from 1 to 9 with two of each
            for (int i = 1; i <= 9; i++)
            {
                theDeck[(2 * i) + 24].assignCard(false, "green", i);
                theDeck[(2 * i) + 25].assignCard(false, "green", i);
            }
            theDeck[44].assignCard(true, "green", 10);
            theDeck[45].assignCard(true, "green", 10);
            theDeck[46].assignCard(true, "green", 11);
            theDeck[47].assignCard(true, "green", 11);
            theDeck[48].assignCard(true, "green", 12);
            theDeck[49].assignCard(true, "green", 12);

            // Assign the blue cards
            // Assing the first blue card a value of zero
            theDeck[50].assignCard(false, "blue", 0);
            // Assign the next 18 cards with values from 1 to 9 with two of each
            for (int i = 1; i <= 9; i++)
            {
                theDeck[(2 * i) + 49].assignCard(false, "blue", i);
                theDeck[(2 * i) + 50].assignCard(false, "blue", i);
            }
            theDeck[69].assignCard(true, "blue", 10);
            theDeck[70].assignCard(true, "blue", 10);
            theDeck[71].assignCard(true, "blue", 11);
            theDeck[72].assignCard(true, "blue", 11);
            theDeck[73].assignCard(true, "blue", 12);
            theDeck[74].assignCard(true, "blue", 12);

            // Assign the yellow cards
            // Assing the first yellow card a value of zero
            theDeck[75].assignCard(false, "yellow", 0);
            // Assign the next 18 cards with values from 1 to 9 with two of each
            for (int i = 1; i <= 9; i++)
            {
                theDeck[(2 * i) + 74].assignCard(false, "yellow", i);
                theDeck[(2 * i) + 75].assignCard(false, "yellow", i);
            }
            theDeck[94].assignCard(true, "yellow", 10);
            theDeck[95].assignCard(true, "yellow", 10);
            theDeck[96].assignCard(true, "yellow", 11);
            theDeck[97].assignCard(true, "yellow", 11);
            theDeck[98].assignCard(true, "yellow", 12);
            theDeck[99].assignCard(true, "yellow", 12);

            // Assign the 8 wild cards
            for (int i = 0; i < 4; i++)
            {
                theDeck[100 + i].assignCard(true, "wild", 13);
            }
            for (int i = 0; i < 4; i++)
            {
                theDeck[104 + i].assignCard(true, "wild", 14);
            }

            return theDeck;
        } 
    
        // Static shuffle deck function to randomize the cards
        public static Deck shuffleDeck()
        {
            // Randomize the deck and perform 100000 random swaps
            Random randomCard = new Random();
            // Shuffle the deck 100,000 times
            for (int i = 0; i < 100000; i++)
            {
                // create a temporary card to hold one value
                Card tempCard = new Card();
                // create two indexes to swap
                int index = randomCard.Next(108);
                int newIndex;
                // make sure the indicies are unique
                do
                {
                    newIndex = randomCard.Next(108);
                } while (newIndex == index);
                tempCard = Game.thisDeck.theDeck[index];
                Game.thisDeck.theDeck[index] = Game.thisDeck.theDeck[newIndex];
                Game.thisDeck.theDeck[newIndex] = tempCard;
            }
            return Game.thisDeck;
        }
        public static Deck shufflePile()
        {
            // Randomize the deck and perform 100000 random swaps
            Random randomCard = new Random();
            // Shuffle the deck 100,000 times
            for (int i = 0; i < 100000; i++)
            {
                // create a temporary card to hold one value
                Card tempCard = new Card();
                // create two indexes to swap
                int index = randomCard.Next(Game.thePile.DeckSize);
                int newIndex;
                // make sure the indicies are unique
                do
                {
                    newIndex = randomCard.Next(Game.thePile.DeckSize);
                } while (newIndex == index);
                tempCard = Game.thePile.theDeck[index];
                Game.thePile.theDeck[index] = Game.thePile.theDeck[newIndex];
                Game.thePile.theDeck[newIndex] = tempCard;
            }
            return Game.thePile;
        }

        // Remove a card from the deck
        public Card removeCard(Card aCard)
        {
            return aCard;
        }

        public static void resetDeck()
        {
            int anIndex = Game.discardPile.DeckSize - 1;
            Console.WriteLine("Discard pile is " + anIndex);
            Console.ReadKey();
            Game.TopCard = Game.discardPile.theDeck[anIndex];
            Game.discardPile.theDeck.RemoveAt(anIndex);
            anIndex--;
            Game.thePile.theDeck.Clear();

            for(int i = pileIndex; i > 0;  i--)
            {
                Game.thePile.theDeck.Add(Game.discardPile.theDeck[i]);
            }

            Game.discardPile.theDeck.Clear();
            //shuffleDeck();
            shufflePile();
            Game.ThePile.printPile();
        }

        public void printDeck()
        {
            Console.WriteLine("This Deck has " + DeckSize + " Cards.");
            for (int i = 0; i < DeckSize; i++)
            {
                Console.WriteLine(theDeck[i].ToString());
            }
        }

        public void printPile()
        {
            for (int i = 0; i < Game.thePile.DeckSize; i++)
            {
                Console.WriteLine(Game.thePile.theDeck[i].ToString());
            }
        }
    }

    /// <summary>
    /// Card hand class from which the players hand and computer hands are derived
    /// </summary>
    class CardHand
    {
        // Fields to hold lists of cards for the hand and the cards in that hand that can be played
        private List<Card> theHand;
        private List<Card> playableCards;
        
        // Properties to set and get the above lists
        public List<Card> TheHand 
        {
            get
            {
                return theHand;
            } 
            set
            {
                theHand = value;
            }
        
        }
        public List<Card> PlayableCards 
        {
            get
            {
                return playableCards;
            } 
            set
            {
                playableCards = value;
            }
        }

        // Default Card Hand Constructor
        public CardHand()
        {
        }

        public override string ToString()
        {
            string printCardHand = "";
            for(int i = 0; i < TheHand.Count; i++)
            {
                printCardHand += TheHand[i].ToString();
            }
            return printCardHand;
        }
    }

    /// <summary>
    /// The main Player Hand  inheriting from Card Hand Class
    /// </summary>
    class PlayerHand : CardHand
    {
        List<Card> hand = new List<Card>();
                
        // Default Constructor
        public PlayerHand()
        {
            hand = TheHand;
        }
    }

    /// <summary>
    /// The Computer Hand inheriting from Card Hand Class
    /// </summary>
    class ComputerHand : CardHand
    {
        List<Card> hand = new List<Card>();

        // Default Constructor
        public ComputerHand()
        {
            hand = TheHand;
        }
    }


    /// <summary>
    /// Player class to hold all of the current player information
    /// </summary>
    class Player
    {
        // Fields used to hold name, score, current hand, and the card chosen to play
        private string name;
        private int score;
        private List<Card> thePlayerHand = new List<Card>();
        private List<Card> playableCards = new List<Card>();
        private Card cardToPlay;
        
        // Properties to set and get the above fields
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
            }
        }
        public List<Card> ThePlayerHand
        {
            get
            {
                return thePlayerHand;
            }
            set
            {
                thePlayerHand = value;
            }
        }
        public List<Card> PlayableCards
        {
            get
            {
                return playableCards;
            }
            set
            {
                playableCards = value;
            }
        }
        public Card CardToPlay
        {
            get
            {
                return cardToPlay;
            }
            set
            {
                cardToPlay = value;
            }
        }

        // Default Constructor
        public Player()
        {
        }

        // Method to determine which card to play
        public void selectCard()
        {
            PlayableCards.Clear();
            for (int i = 0; i < ThePlayerHand.Count; i++)
            {
                if (ThePlayerHand[i].Colour == Game.TopCard.Colour || ThePlayerHand[i].CardValue == Game.TopCard.CardValue)
                {
                    PlayableCards.Add(ThePlayerHand[i]);
                }
            }

            Card aCard;
            if (PlayableCards.Count > 0)
            {
                aCard = PlayableCards[0];
                CardToPlay = aCard;
                playCard();
            }
            else
            {
                aCard = drawCard();
                addToHand(aCard);
                if (aCard.Colour == Game.TopCard.Colour || aCard.CardValue == Game.TopCard.CardValue)
                {
                    CardToPlay = aCard;
                    PlayableCards.Add(aCard);
                    playCard();
                }
            }
        }

        // Method to add a card to the players hand
        public void addToHand(Card aCard)
        {
            ThePlayerHand.Add(aCard);
        }

        // Method to draw a card from the deck
        public Card drawCard()
        {
            Card aCard;
            if (Deck.pileIndex > 0)
            {
                aCard = Game.thePile.theDeck[Deck.pileIndex];
                Deck.pileIndex--;
            }
            else
            {
                aCard = Game.thePile.theDeck[0];
                Deck.resetDeck();
            }
            return aCard; 
        }

        // Deal a full round of cards to all four players
        public void dealHand(Deck Pile)
        {
            Card justACard = new Card();
            Game.TopCard = Pile.theDeck[Deck.pileIndex];
            Console.WriteLine("TOP CARD IS " + Game.TopCard.ToString());
            for (int j = 0; j < 7; j++)
            {
                Console.WriteLine("This is Card " + j + " ");
                for (int turn = 0; turn < 4; turn++)
                {
                    Console.WriteLine("This is Player " + turn + " ");
                    if (Deck.pileIndex > 0)
                    {
                        justACard = Game.thePile.theDeck[Deck.pileIndex];
                        Game.playerList[turn].addToHand(justACard);
                        Console.WriteLine(justACard.ToString());
                        Deck.pileIndex--;
                    }
                    else
                    {
                        Deck.resetDeck();
                        Deck.shufflePile();
                        Deck.pileIndex = Game.thePile.DeckSize - 1;
                    }
                }
            }
        }

        // Play a card from the list of possible cards to play
        public void playCard()
        {
            if (PlayableCards.Count > 0)
            {
                Game.TopCard = CardToPlay;
                Console.WriteLine("TOP CARD IS " + Game.TopCard.ToString());
                Game.discardPile.theDeck.Add(CardToPlay);
                ThePlayerHand.Remove(CardToPlay);
                Console.WriteLine("");
            }
            
        }

        // Begin a new game
        public void beginGame()
        {
        }

        public override string ToString()
        {
            return "This player is " + Name;
        }

        public void displayPlayerHand()
        {
            for(int i = 0; i < ThePlayerHand.Count; i++)
            {
                Console.WriteLine(ThePlayerHand[i].ToString());
            }
        }
    }

    /// <summary>
    /// Human Player as an instance of the player class used uniquely for the player
    /// </summary>
    class HumanPlayer : Player
    {
        private List<Card> userHand;
        public List<Card> UserHand 
        {
            get
            {
                return userHand;
            } 
            set
            {
                userHand = value;
            }
        }
        public HumanPlayer()
        {
            userHand = ThePlayerHand;
        }
    }

    /// <summary>
    /// AI Player inheriting from the generic player class
    /// </summary>
    class AIPlayer : Player
    {
        // Field to hold the enemy hand
        private List<Card> opponentHand;
        // Properties to get and set an enemies hand
        public List<Card> OpponentHand 
        {
            get
            {
                return opponentHand;
            }
            set
            {
                opponentHand = value;
            }
        }

        // Default Constructor
        public AIPlayer()
        {
            OpponentHand = ThePlayerHand;
        }
    }

    /// <summary>
    /// Main Menu class for the initial game screen
    /// </summary>
    class MainMenu
    {
        // Default Constructor
        public MainMenu()
        {
        }

        // Method used to start a new game
        public void startGame()
        {
        }
        // Method to display the game rules
        public void instructions()
        {
        }
        // Method to set game options such as difficulty level
        public void gameOptions()
        {
        }
    }

    /// <summary>
    /// Game View Screen where the actual game play takes place
    /// </summary>
    class GameView
    {
        // Default Constructor
        public GameView()
        {
        }

        // Returns the player who deals first
        public Player assignDealer(List<Player> players)
        {
            Random firstPlayer = new Random();
            return players[firstPlayer.Next(4)];
        }

        // Method to assign the next turn
        public void assignTurn()
        {
        }

        // Method to display the cards
        public void showPlayerHand()
        {
        }
    }

    /// <summary>
    /// Results and Statistics Menu 
    /// </summary>
    class ResultsView
    {
        // Default Constructor
        public ResultsView()
        {
        }

        // Fields holding values to display
        private string winner;
        private int turns;
        private float gameTime;

        // Properties to get and set above fields
        public string Winner
        {
            get
            {
                return winner;
            }
            set
            {
                winner = value;
            }
        }
        public int Turns
        {
            get
            {
                return turns;
            }
            set
            {
                turns = value;
            }
        }
        public float GameTime
        {
            get
            {
                return gameTime;
            }
            set
            {
                gameTime = value;
            }
        }

        // Returns the winners name
        public string assignWinner()
        {
            return Winner;
        }

        // Returns the number of turns for the player
        public int countTurns()
        {
            return Turns;
        }

        // Returns the total game time
        public float calculateGameTime()
        {
            return GameTime;
        }

        // Display the above results
        public void displayResults()
        {
        }
    }

    /// <summary>
    /// Options menu 
    /// </summary>
    class Options
    {
        // Default Constructor
        public Options()
        {

        }
    }
}
