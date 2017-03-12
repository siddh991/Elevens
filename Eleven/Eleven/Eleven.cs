//Project Name       : A1_Review
//File Name          : Eleven.cs
//Author             : Siddharth Surana
//Due Date           : Feb 15 2017
//Modified Date      : Feb 15 2017
//Program Description: A single player console game which simulates the card game named Elevens
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eleven
{
    class Eleven
    {
        // Create global variables to store the game loop options and if the game is being played or ended.
        static string gameState = "newGame";
        static bool continuePlay = true;

        // Global list and 2d array to store all game cards. 
        // The cards that are with the dealer are in "deck" and the displayed cards are in "onDisplay".
        static List<int> deck = new List<int>();
        static int[,] onDisplay = new int[12, 2];

        // Create variable for generating random number.
        static Random rnd = new Random();

        // Create constants for storing unchanging values within the game, including the first face card number, 
        // the sum of eleven, and the ASCII value for a.
        const int FACE_CARD_START = 11;
        const int CORRECT_SUM_ELEVEN = 11;
        const int ASCII_A_LOWER = 97;

        static void Main(string[] args)
        {
            // Define variable which takes user input for the two game options(swap and sum of eleven) and another to store if the input is valid.
            string userInput;
            bool validInput;

            // Define variables to store the values of the two cards which add up to eleven in the form of integer and character.
            int cardOneLoc;
            int cardTwoLoc;
            char cardOneLocChar;
            char cardTwoLocChar;

            // Define variables to take, store and manipulate the face cards which the user wants to swap.
            int faceCardChoiceLoc;
            char faceCardChar;

            // Enter loop if the game is started and being played.
            while (continuePlay == true)
            {
                // Enter if statement if the game is setting up for a new game.
                if (gameState == "newGame")
                {
                    // Create and shuffle the deck by calling the respective subprograms. 
                    CreateDeck(deck);
                    Shuffle(deck);

                    // Assign the top 12 cards from the deck to a unique pile and remove these cards from the deck.
                    // Set the pile count for each card to 1 becasue this is the first card placed on each pile.
                    for (int a = 0; a < onDisplay.GetLength(0); a++)
                    {
                        onDisplay[a, 0] = deck[a];
                        onDisplay[a, 1] = 1;
                        deck.RemoveAt(a);
                    }

                    // Call procedure to update the screen and display the cards.
                    UpdateScreen(onDisplay);

                    // Set the game state to "inGame" meaning the game is fully set up and ready to play.
                    gameState = "inGame";
                }
                // Enter the if statement if the game state is "inGame", meaning the game is ready to play. The game is played by the user within this if statement.
                else if (gameState == "inGame")
                {
                    // Display two options the user has to play the game, either move a face card or state an eleven pair.
                    Console.WriteLine("\n\nChoose an option by number:");
                    Console.WriteLine("1) Move a Face Card");
                    Console.WriteLine("2) State an Eleven Pair");

                    // Call a function that checks if the game has been won, lost, or neither. If the game is lost or won, store whether the game should restart or end.
                    // If the game is still incomplete, then continue without changing gameState value.
                    gameState = CheckWinLoss(deck, onDisplay);

                    // If the user chooses to end game, then break out of the game loop and let it end.
                    if (gameState == "endGame")
                    {
                        break;
                    }
                    // If the game is set to continue then enter the if statement.
                    else if (gameState == "inGame")
                    {
                        // Store the users choice between the two game options in a variable called userInput.
                        userInput = Console.ReadLine();

                        // If the user chose option 1 to move face card, enter the if statement.
                        if (userInput == "1")
                        {
                            // Update the screen to clear the previous menu and ask the user for the pile that they want to swap based on the letter associated with the card.
                            UpdateScreen(onDisplay);
                            Console.WriteLine("\n\nChoose a pile by its designated number");

                            // Try to get user input and correctly manipulate it. If the input is incorrect the program will redirect to the catch.
                            try
                            {
                                // Read in the user's letter choice to swap and convert it to an integer.
                                faceCardChar = Convert.ToChar(Console.ReadLine());
                                faceCardChoiceLoc = (int)(faceCardChar - ASCII_A_LOWER);

                                // If the pile selection is out of bounds(outside 0-11) or a face card is selected, then the input is invalid. In such a case, enter the if statement. 
                                if (faceCardChoiceLoc >= onDisplay.GetLength(0) || faceCardChoiceLoc < 0 || onDisplay[faceCardChoiceLoc, 0] < FACE_CARD_START || onDisplay[faceCardChoiceLoc, 1] > 1)
                                {
                                    // Store the input validity as false.
                                    validInput = false;

                                    // Display an error message telling the user that the input was invalid.
                                    ErrorMessage();
                                }
                                // If the input was valid, then enter the else option.
                                else
                                {
                                    // Call a procedure to put the face card at the end of the deck and put the top card from the deck in its place.
                                    CardSwap(faceCardChoiceLoc, deck, onDisplay);
                                }

                            }
                            // If the above try section has an error, catch it here and enter the catch statement.
                            catch (Exception)
                            {
                                // Display an error message telling the user that the input was invalid.
                                ErrorMessage();
                            }
                        }
                        // If the user selected the second option to find two cards whos values add up to 11, then enter this if statemment. 
                        else if (userInput == "2" && deck.Count > 1)
                        {
                            // Set the user input to valid by default.
                            validInput = true;

                            // Update the screen to clear the previous menu.
                            UpdateScreen(onDisplay);

                            // Try to run the code within the try section, and if an error is thrown, catch it in the catch section below.
                            try
                            {
                                // Ask the user for the fist card based on the letter associated with the card.
                                Console.WriteLine("\n\nChoose the first pile by its designated letter");

                                // Read in the users first letter choice and convert it to an integer.
                                cardOneLocChar = Convert.ToChar(Console.ReadLine());
                                cardOneLoc = (int)(cardOneLocChar - ASCII_A_LOWER);

                                // If the chosen card pile is invalid because it is out of bounds or is a face card, then enter the if statement
                                if (cardOneLoc >= onDisplay.GetLength(0) || cardOneLoc < 0 || onDisplay[cardOneLoc, 0] >= FACE_CARD_START)
                                {
                                    // Store that the input was invalid.
                                    validInput = false;

                                    // Display an error message telling the user that the input was invalid.
                                    ErrorMessage();
                                }

                                // If the first input was valid then enter this if statement where the second input is chosen. 
                                if (validInput == true)
                                {
                                    // Update the screen to clear the previous menu and ask the user for the second of the two cards that add to 11.
                                    UpdateScreen(onDisplay);
                                    Console.WriteLine("\n\nChoose the second pile by its designated letter that is not pile " + cardOneLocChar);

                                    // Read in the users second letter choice and convert it to an integer.
                                    cardTwoLocChar = Convert.ToChar(Console.ReadLine());
                                    cardTwoLoc = (int)(cardTwoLocChar - ASCII_A_LOWER);

                                    // If the chosen card pile is invalid because it is out of bounds or is a face card, then enter the if statement
                                    if (cardTwoLoc >= onDisplay.GetLength(0) || cardTwoLoc < 0 || cardTwoLoc == cardOneLoc || onDisplay[cardTwoLoc, 0] >= FACE_CARD_START)
                                    {
                                        // Store that the input was invalid.
                                        validInput = false;

                                        // Display an error message telling the user that the input was invalid.
                                        ErrorMessage();
                                    }

                                    // If both the inputs were found to be valid, call a procedure to check if the cards add up to 11 and if so, place 1 new card on top of each. 
                                    if (validInput == true)
                                    {
                                        PairOfEleven(cardOneLoc, cardTwoLoc, onDisplay, deck);
                                    }
                                }
                            }
                            // If the try above threw an error becasue the input was invalid, catch it here and enter the catch section. 
                            catch (Exception)
                            {
                                // Display an error message telling the user that the input was invalid.
                                ErrorMessage();
                            }
                        }
                        // If neither option 1 or 2 is selected, inform the user that this is an invalid input. 
                        else
                        {
                            // Display an error message telling the user that the input was invalid.
                            ErrorMessage();
                        }
                    }
                    // Update the screen to clear the previous menu and text.
                    UpdateScreen(onDisplay);
                }
            }
        }

        // Pre: The deck list called "deck" is passed to this procedure, assuming the list is of integers. 
        // Post: All cards in a deck are added to the list called "deck".
        // Description: The procedure first clears the list called "deck". Then, it stores every card of a standard deck within the emptied list using a nested loop.
        private static void CreateDeck(List<int> deck)
        {
            const int NUM_CARDS_PER_SUIT = 13;
            const int NUM_OF_SUITS = 4;
            // Remove all cards that are currently stored in the list.
            deck.Clear();

            // Loop through every count in the number of cards per suit that exist within a deck(from 1 to 13).
            for (int cardNum = 1; cardNum <= NUM_CARDS_PER_SUIT; cardNum++)
            {
                // Loop throgh the number of suits that exist within a deck or cards(4 card types).
                for (int suits = 1; suits <= NUM_OF_SUITS; suits++)
                {
                    // Add the same 4 card numbers to the end of deck.
                    deck.Add(cardNum);
                }
            }
        }

        // Pre: The deck list called "deck" is passed to this procedure. The list must be of integers and the deck must be full with 52 cards within it.
        // Post: The cards within the deck are swapped randomly, resulting in a shuffled deck. 
        // Description: The cards of the list are shuffled by taking two cards and swapping their location. The entire shuffle process(involving the shuffle of every card) happens 3 times. 
        private static void Shuffle(List<int> deck)
        {
            // Temporary holder of card and random number generator used during the swap process.
            int tempCardHolder;
            int getCard;

            // Define an integer to store the number of times the deck will be shuffled.  
            int numOfShuffle = 3;

            // Loop for 3 times where every card is shuffled in the deck
            for (int x = 0; x < numOfShuffle; x++)
            {
                // Loop through the length of the deck and swap the current card with a random card from anywhere in the deck.
                for (int y = 0; y < deck.Count; y++)
                {
                    // Generate and store a random card location in the list and store this card in a temporary variable.
                    getCard = rnd.Next(1, deck.Count);
                    tempCardHolder = deck[getCard];

                    // Swap the randomly chosen card with the card located at at the current loop iteration and store the temporary card in its place. 
                    deck[getCard] = deck[y];
                    deck[y] = tempCardHolder;
                }
            }
        }

        // Pre: The 2D cards integer array of the piles is passed into this procedure. The array must be full with 12 cards.
        // Post: All the cards from the 2D array are displayed to the user. 
        // Description: First the console is cleared. Then, the cards are displayed to the user. The face cards and aces are converted to their symbol. 
        private static void UpdateScreen(int[,] onDisplay)
        {
            // The console is cleared to remove the past data and information from the previous move. 
            Console.Clear();

            // A constant integer is defined to store the card location of half the cards, after which the following cards are displayed on the next line.  
            const int HALF_THROUGH_DISPLAY = 6;

            // A string is defined to hold the card location which is to be displayed. 
            string displayCard;

            // A for loop is used to loop through every card which is to be displayed.
            for (int b = 0; b < onDisplay.GetLength(0); b++)
            {
                // The card which is to be displayed is converted to a string so it can be replaced with a symbol if necessary.
                displayCard = Convert.ToString(onDisplay[b, 0]);

                // A switch statement is used to determine if the card which is to be displayed is an ace or a face card by comparing the card to the value of these cards.
                switch (displayCard)
                {
                    // If the card to be displayed has a value of 1, then it is replaced with the character 'A'. Then, breaks out of the case.
                    case "1":
                        displayCard = "A";
                        break;
                    // If the card to be displayed has a value of 11, then it is replaced with the character 'J'. Then, change the text colour to cyan and break out of the case.
                    case "11":
                        displayCard = "J";
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    // If the card to be displayed has a value of 12, then it is replaced with the character 'Q'. Then, change the text colour to cyan and break out of the case.
                    case "12":
                        displayCard = "Q";
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    // If the card to be displayed has a value of 13, then it is replaced with the character 'K'. Then, change the text colour to cyan and break out of the case.
                    case "13":
                        displayCard = "K";
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    // If the card to be displayed does not fall into the cases above, break out of the case.
                    default:
                        break;
                }

                // Display the card with the character location and pile count beside it in backets. Add space after this has been displayed. 
                Console.Write(displayCard + "(" + (char)(ASCII_A_LOWER + b) + "-" + onDisplay[b, 1] + ")" + "      ");

                // Change the text colour back to default which is a light grey/white. 
                Console.ResetColor();

                // If the card that is being displayed is the 6th card, then add a blank line after this card.
                if (b + 1 == HALF_THROUGH_DISPLAY)
                {
                    Console.WriteLine("\n");
                }
            }
        }

        // Pre: The face card location, the fully shuffled deck list of integers, and the full 2D integer array of cards on display are passed into this procedure.
        // Post: The chosen face card is moved to the back of the deck and the top card of the deck replaces it. If an exception occurs, a proper error message is displayed.
        // Description: This subprogram moves the face card to the back of the deck and the top card replaces it. 
        private static void CardSwap(int faceCardChoiceLoc, List<int> deck, int[,] onDisplay)
        {
            // Try to run the code in the try section to do the whole swap process. If an exception is thrown, it will be caught be the catch section below. 
            try
            {
                // Add the face card to the end of the deck. 
                deck.Add(onDisplay[faceCardChoiceLoc, 0]);

                // Move the top card form the deck to the pile where the face card was removed. Remove the top card from the deck after that. 
                onDisplay[faceCardChoiceLoc, 0] = deck[0];
                deck.RemoveAt(0);

            }
            // If the above try section thows an exception as it attempts to perform the swap, then catch it here and enter the catch section. 
            catch (Exception)
            {
                // Display an error message telling the user that the input was invalid.
                ErrorMessage();
            }
        }

        // Pre: The first and second cards chosen by the user, the 2D full integer array of cards on display, and the fully shuffled integer deck list are passed into this procedure.
        // Post: The subprogram removes the two cards from the 2D array, replaces them with two cards from the deck, and increases their pile count. 
        //       If the two cards do not add up to 11, an error message is shown to the user. 
        // Description: This procedure checks if the two cards add to 11 and if so, replaces them with two new cards from the deck and increases their pile count. 
        private static void PairOfEleven(int cardOneLoc, int cardTwoLoc, int[,] onDisplay, List<int> deck)
        {
            // An integer to store the sum of the two chosen cards is created and the sum is calculated.
            int sumOfCards = onDisplay[cardOneLoc, 0] + onDisplay[cardTwoLoc, 0]; ;

            // If the sum is 11, then enter the if statement.
            if (sumOfCards == CORRECT_SUM_ELEVEN)
            {
                // Replace the fist chosen card with the top card form the deck. Increment the pile count for that pile. 
                onDisplay[cardOneLoc, 0] = deck[0];
                onDisplay[cardOneLoc, 1]++;

                // Replace the fist chosen card with the top card form the deck. Increment the pile count for that pile.
                onDisplay[cardTwoLoc, 0] = deck[1];
                onDisplay[cardTwoLoc, 1]++;

                // Remove the top two cards of the deck becasue they have been placed on display in replacement for the two cards that add up to 11. 
                deck.RemoveAt(0);
                deck.RemoveAt(1);
            }
            // If the cards do not add to 11, enter the else section. 
            else
            {
                // Display a message to inform the user that the cards do not add up to 11. Wait until the user presses enter before continuing the program. 
                Console.WriteLine("Sorry, the two chosen piles do not add to 11");
                Console.WriteLine("Please ENTER to continue");
                Console.ReadLine();
            }
        }

        // Pre: N/A
        // Post: A message is shown to tell the user that the input was invalid and the program waits for the user to press enter to continue.
        // Description: The procedure tells the user that the pile choice was invalid. This happens if the input is out of range, formatted incorrectly, etc. 
        private static void ErrorMessage()
        {
            // Display message saying that the pile choice was invalid. Wait for user to press enter to continue through the program.
            Console.WriteLine("Sorry! Invalid option choice! Please select from the given options.");
            Console.WriteLine("Please ENTER to continue");
            Console.ReadLine();
        }

        // Pre: The complete deck list of integers and the full 2D integer array of cards on display are passed into this procedure.
        // Post: Return to the game state whether the game should restart, end, or continue. 
        // Description: This function fist checks if the game has been won, if not then it checks if two cards add to 11 and further the game. 
        //              If not, then the function checks whether there is a valid face card to be swapped and continue the game. If not, the game has been lost. 
        //              If the game is won or lost, the user is given the option to play again or end the game. 
        private static string CheckWinLoss(List<int> deck, int[,] onDisplay)
        {
            // Create string variable to store if the game has won, lost or neither. 
            string gameResult = "continue";

            // Create a string to store the users response if asked to restart the game in the case of a win or a loss. 
            string playAgain;

            // This constant stores the minimum number of cards the deck must have for a chance to win.
            const int MIN_CARD_COUNT = 2; 

            // Loop through every card in the 2D array which are on display to the user. 
            for (int m = 0; m < onDisplay.GetLength(0); m++)
            {
                // If even a single card is not a face card, then set the game result to a loss and break out of the loop. 
                if (onDisplay[m, 0] <= FACE_CARD_START)
                {
                    gameResult = "loss";
                    break;
                }
                // If the card is a face card, set the game result to a win. 
                else
                {
                    gameResult = "win";
                }
            }

            // If the number of cards in the deck is at least 2 and the game has not been won, then enter the if statement to check if the user can select two cards from
            // the cards on display to add to 11 and continue the game. 
            if (deck.Count() >= MIN_CARD_COUNT && gameResult != "win")
            {
                // Loop through every card on display except the last one. 
                for (int j = 0; j < onDisplay.GetLength(0) - 1; j++)
                {
                    // Loop through every card on display after the card selected from the previous loop.
                    for (int s = j + 1; s < onDisplay.GetLength(0); s++)
                    {
                        // If any combination of two cards added together results in a sum of eleven, this means that the player has an option to continue the game. Enter the if statement. 
                        if (onDisplay[j, 0] + onDisplay[s, 0] == CORRECT_SUM_ELEVEN)
                        {
                            // Set the game result to continue and break out of both loops. 
                            gameResult = "continue";
                            j = onDisplay.GetLength(0);
                            break;
                        }
                    }
                }
            }

            // If the game result is at a loss, enter the if statment to check  if there is a possibility that the player can continue the game. 
            if (gameResult == "loss")
            {
                // Loop through every card on display
                for (int f = 0; f < onDisplay.GetLength(0); f++)
                {
                    // If the deck has a count of at least 2 and a face card with a pile count of 1 is found, then enter the if statement. 
                    if (deck.Count() >= MIN_CARD_COUNT && onDisplay[f, 0] >= FACE_CARD_START && onDisplay[f, 1] == 1)
                    {
                        // Set the game result to continue because the player has a move they can do. Also, exit the loop.
                        gameResult = "continue";
                        f = onDisplay.GetLength(0);
                    }
                }
            }

            // If the user has won the game, enter the if statment. 
            if (gameResult == "win")
            {
                // Inform the user that they have won and ask of they want to replay. Wait for their response. 
                Console.WriteLine("\nYou Win!");
                Console.WriteLine("Do you want to play again? Press Y to replay or any other key to quit.");
                playAgain = Console.ReadLine();

                // If the user presses Y or y for yes, return a game state of "newGame" to prepare and set up for a new game. 
                if (playAgain == "Y" || playAgain == "y")
                {
                    return "newGame";
                }
                // If the user presses any other key, return a game state of "endGame" to terminate the game. 
                else
                {
                    return "endGame";
                }
            }
            // Otherwise, if the user has lost the game, enter the if statment. 
            else if (gameResult == "loss")
            {
                // Inform the user that they have lost and ask of they want to replay. Wait for their response. 
                Console.WriteLine("\nSorry you lose!");
                Console.WriteLine("Do you want to play again? Press Y to replay or any other key to quit.");
                playAgain = Console.ReadLine();

                // If the user presses Y or y for yes, return a game state of "newGame" to prepare and set up for a new game. 
                if (playAgain == "Y" || playAgain == "y")
                {
                    return "newGame";
                }
                // If the user presses any other key, return a game state of "endGame" to terminate the game. 
                else
                {
                    return "endGame";
                }
            }
            // If the user has neither won or lost, return a game state of "inGame" and continue the game as it is. 
            else
            {
                return "inGame";
            }
        }
    }
}