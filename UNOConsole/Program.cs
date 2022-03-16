// OOP 4200 Final Project - UNO Card Game
// Matthew Ware - 100472787
// March 16 2022
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
            Deck theDeck = new Deck();
            DiscardPile discardPile = new DiscardPile();
            List<Player> playerList = new List<Player>();
            theDeck.initializeDeck();
            playerList = theGame.initializePlayers();

            // Show upon loading 
            Console.WriteLine("Welcome to UNO");
            Console.ReadKey();

            // Assign initial dealer
            theGame.CurrentDealer = theGame.setDealer(playerList, theGame.ClockwiseTurns);

            // Run the main gaim loop through recurrsion
            theGame.takeTurn(theGame, theDeck, discardPile, playerList);
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
        private bool turnOver = false;
        private bool clockwiseTurns = true;

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
            initializePlayers();
        }

        // Return a List of Players after adding a human player and three computer players
        public List<Player> initializePlayers()
        {
            // Generate list and add the human player
            List<Player> playerList = new List<Player>();
            HumanPlayer user = new HumanPlayer();
            playerList.Add(user);

            // Add three computer players to the player list
            AIPlayer computer1 = new AIPlayer();
            AIPlayer computer2 = new AIPlayer();
            AIPlayer computer3 = new AIPlayer();
            playerList.Add(computer1);
            playerList.Add(computer2);
            playerList.Add(computer3);

            // Return the list of all 4 players
            return playerList;
        }

        // Returns the player who is now the dealer from the list provided and direction of turns
        public Player setDealer(List<Player> playerList, bool clockwiseTurns)
        {
            // Check if the currentPlayer is unassigned and assign the dealer to a random player
            if (currentPlayer == null)
            {
                Random randomPlayer = new Random();
                this.CurrentDealer = playerList[randomPlayer.Next(4)];
            }
            else
            {
                this.CurrentDealer = this.CurrentPlayer; // change this to set next dealer
            }
            // Return the player who is the current dealer
            return this.CurrentDealer; 
        }

        // Main Game Loop
        public void takeTurn(Game theGame, Deck theDeck, DiscardPile discardPile, List<Player> playerList)
        {
            if (this.turnOver)
            {
                // At the end of a turn deal a new hand to each player using cards from the discard pile if needed
                CurrentPlayer.dealHand(playerList, discardPile);
                this.TurnOver = false;
            }
            else
            {
                // Perform Turn Options here
                //this.TurnOver = true;
            }
            // Pick the next player and assign them to the current player
            theGame.CurrentPlayer = pickNextPlayer(theGame.CurrentPlayer, playerList, theGame.ClockwiseTurns);

            // Self-Call to continue game through recoursion 
            takeTurn(theGame, theDeck, discardPile, playerList);
        }

        // Returns the player who is next in line to play due to last card played or in an order
        public Player pickNextPlayer(Player thisPlayer, List<Player> playerList, bool clockwiseTurns)
        {
            // Determine who is next to be the current player first in a clockwise direction
            if(clockwiseTurns)
            {
                if(playerList[3] == thisPlayer)
                {
                    thisPlayer = playerList[0];
                }
                else if (playerList[2] == thisPlayer)
                {
                    thisPlayer = playerList[3];
                }
                else if (playerList[1] == thisPlayer)
                {
                    thisPlayer = playerList[2];
                }
                else
                {
                    thisPlayer = playerList[1];
                }
            }
            // Counter-Clockwise direction
            else
            {
                if (playerList[3] == thisPlayer)
                {
                    thisPlayer = playerList[2];
                }
                else if (playerList[0] == thisPlayer)
                {
                    thisPlayer = playerList[3];
                }
                else if (playerList[1] == thisPlayer)
                {
                    thisPlayer = playerList[0];
                }
                else
                {
                    thisPlayer = playerList[1];
                }
            }
            // Return the next player to take a turn
            return thisPlayer;
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
        public static List<Card> theDeck = new List<Card>();
        
        // Default constructor for the Deck
        public Deck()
        {
            initializeDeck();
        }

        // Create 108 black cards and assign values to each card
        public void initializeDeck()
        {
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
        } 
    
        // Static shuffle deck function to randomize the cards
        public static void shuffleDeck()
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
                tempCard = Deck.theDeck[index];
                Deck.theDeck[index] = Deck.theDeck[newIndex];
                Deck.theDeck[newIndex] = tempCard;
            }
        }

        // Remove a card from the deck
        public Card removeCard(Card aCard)
        {
            Deck.theDeck.Remove(aCard);
            return aCard;
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
    }

    /// <summary>
    /// The main Player Hand  inheriting from Card Hand Class
    /// </summary>
    class PlayerHand : CardHand
    {
        // Default Constructor
        public PlayerHand()
        {
        }
    }

    /// <summary>
    /// The Computer Hand inheriting from Card Hand Class
    /// </summary>
    class ComputerHand : CardHand
    {
        // Default Constructor
        public ComputerHand()
        {
        }
    }

    /// <summary>
    /// Discard Pile for the game object with a static list of cards
    /// </summary>
    class DiscardPile
    {
        private static List<Card> theDiscardPile;
        public static List<Card> TheDiscardPile 
        {
            get
            {
                return theDiscardPile;
            } 
            set
            {
                theDiscardPile = value;
            }
        }
        
        // Default Constructor
        public DiscardPile()
        {
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
        private static List<Card> playerHand = new List<Card>();
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
        public static List<Card> ThePlayerHand
        {
            get
            {
                return playerHand;
            }
            set
            {
                playerHand = value;
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
        public Card selectCard()
        {
            return playCard();
        }

        // Method to add a card to the players hand
        public void addToHand(Card aCard)
        {
            ThePlayerHand.Add(aCard);
            Deck.theDeck.Remove(aCard);
        }

        // Method to draw a card from the deck
        public void drawCard()
        {
        }

        // Deal a full round of cards to all four players
        public void dealHand(List<Player> playerList, DiscardPile discards)
        {
            for (int j = 0; j < 7; j++)
            {
                for (int turn = 0; turn < 4; turn++)
                {
//                    if (Deck.theDeck.Count != 0)
//                    {
//                        playerList[turn].addToHand(Deck.theDeck[0]);
//                        Deck.theDeck.RemoveAt(0);
//                        for (int i = 0; i < playerList[turn].ThePlayerHand.Count; i++)
//                        {
//                            Console.WriteLine(playerList[turn].ThePlayerHand[i].ToString());
//                        }
//                    }
//                    else
//                    {
//                        // This is not working
//                        for (int i = 0; i < discards.TheDiscardPile.Count; i++)
//                        {
//                            Deck.theDeck.Add(discards.TheDiscardPile[i]);
//                            discards.TheDiscardPile.RemoveAt(i);
//                        }
//                        Deck.shuffleDeck();
//                    }
                }
            }
        }

        // Play a card from the list of possible cards to play
        public Card playCard()
        {
            return CardToPlay;
        }

        // Begin a new game
        public void beginGame()
        {
        }
    }

    /// <summary>
    /// Human Player as an instance of the player class used uniquely for the player
    /// </summary>
    class HumanPlayer : Player
    {
        private static PlayerHand userHand;
        public static PlayerHand UserHand 
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

        }
    }

    /// <summary>
    /// AI Player inheriting from the generic player class
    /// </summary>
    class AIPlayer : Player
    {
        // Field to hold the enemy hand
        private ComputerHand opponentHand;
        // Properties to get and set an enemies hand
        public ComputerHand OpponentHand 
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
