using System.Numerics;
using System.IO;
using System.Security.AccessControl;

namespace PRB99.ASN.FourInOneRow
{
    internal class Program
    {
        int rows = 6; // We use 6 rows and 7 columns because that's the standard for Connect Four.
        int columns = 7;
        char[,] board; //not initialized yet but will be initialized in the main function
        string[] playerNames = new string[2]; // We use a string array to store the names of the players.
        char[] playerSymbols = { 'O', '0' }; // We use a char array to store the symbols of the players.
        char currentPlayerSymbol = 'O'; // Initializing the first player with red because red always starts.
        int moves = 0;

        static void Main(string[] args)
        {
            Program game1 = new Program();

            game1.board = new char[game1.rows, game1.columns + 1]; //We initialize the board first, we also add 1 to the columns because we want to add a column number to the board.
            game1.GetNames(); // Get the names of the players.
            game1.RunGame();
        }

        // Function to get player names and store them in the string array.
        public void GetNames()
        {
            Console.WriteLine("Enter the name of Player 1:");
            playerNames[0] = Console.ReadLine();
            Console.WriteLine("Enter the name of Player 2:");
            playerNames[1] = Console.ReadLine();
        }

        // Draw the game board.
        public void DrawBoard()
        {
            Console.WriteLine("   1   2   3   4   5   6   7"); // Indicate which column has which number.

            for (int i = 0; i < rows; i++) // Iterate through the rows to place symbols in each column.
            {
                Console.WriteLine("  -----------------------------");
                for (int j = 0; j < columns; j++) // Iterate through the columns to place symbols. Use a double loop to fill rows first and then columns.
                {
                    if (board[i, j] == 'O')
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // Set text color to red for Player 1's symbol.
                    }
                    else if (board[i, j] == '0')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Set text color to yellow for Player 2's symbol.
                    }

                    Console.Write($" | {board[i, j]} ");

                    Console.ResetColor(); // Reset text color to default.
                }
                Console.WriteLine("|"); // Close with a | to format the board correctly.
            }
        }

        // Process player input.
        public void PlayerInput(string input)
        {
            int column = IsInputValid(input); // Check if the input is valid and store the column in the variable column.

            if (column >= 1 && column <= 7) // Check if the input is valid by checking if the column is not -1.
            {
                if (!IsColumnFull(column)) // Check if the column is full.
                {
                    PlaceCoins(column); // Place the coin in the column.

                    // Check for win after the player makes a move.
                    if (CheckIfWin())
                    {
                        Console.Clear(); // Clear the console to display the final state of the board.
                        DrawBoard(); // Draw the final state of the board.

                        Console.WriteLine($"Congratulations {GetCurrentPlayer()}! You won!"); // If the current player has won, congratulate them.
                        Console.WriteLine();
                        Console.WriteLine("Would you like to play again? (y/n)");

                        string playAgainInput = Console.ReadLine().ToLower();

                        if (playAgainInput == "y")
                        {
                            Console.WriteLine("Do you want to continue using the same names? (y/n)");
                            string input2 = Console.ReadLine().ToLower();
                            if (input2 == "n")
                            {
                                GetNames(); // Get new names for the players.
                            }

                            board = new char[rows, columns + 1]; // Reset the board for a new game.
                            moves = 0; // Reset the move count.
                        }
                        else if (playAgainInput == "n")
                        {
                            Console.WriteLine("Thanks for playing!");
                            return;
                        }
                    }
                    SwitchPlayer(); // Switch to the other player.

                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid column (1-7). Try again.");
                }
            }
        }

        // Check if the input for columns is valid. Note: We check input for columns, not rows, because in Connect Four, you can only place a coin at the top, not on the sides.
        public int IsInputValid(string userInput)
        {
            try // We use a try-catch block to check if the input is an integer.
            {
                int parsedColumn = int.Parse(userInput); // Parse the input to an integer.

                if (parsedColumn >= 1 && parsedColumn <= columns) // Check if the input is between 1 and 7.
                {
                    return parsedColumn; // Return the column if the input is valid.
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid column (1-7). Try again."); // If the input is not valid, tell the user to try again.
                    return -1; // Return -1 if the input is not valid.
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid column (1-7). Try again."); // If the input is not an integer, tell the user to try again.
                return -1; // Return -1 if the input is not valid.
            }
        } // This returns true if the input is an integer and is between 1 and 7.

        // Place coins in the specified column.
        public void PlaceCoins(int column)
        {
            for (int i = rows - 1; i >= 0; i--) // Search from bottom to top for an empty spot, hence use -1 to move upwards in rows.
            {
                if (board[i, column - 1] == '\0') // Check if the spot is empty; if yes, place the coin of the current player. '\0' represents an empty spot in a char array.
                {
                    board[i, column - 1] = currentPlayerSymbol; // Use -1 because the array starts at 0 while the user starts at 1.
                    moves++; // Increment the move count to check later if the board is full.
                    break;
                }
            }
        }

        // Check if the specified column is full.
        public bool IsColumnFull(int column)
        {
            for (int i = 0; i < rows; i++) // Iterate through the rows to check if the column is full.
            {
                if (board[i, column - 1] == '\0') // Check if the spot is empty; if yes, return false.
                {
                    return false;
                }
            }
            Console.WriteLine("Column is full. Please choose another column.");

            return true;
        }

        // Get the name of the current player.
        public string GetCurrentPlayer()
        {
            string currentPlayerName = "";   // Create an empty string to store the name of the current player.
            if (currentPlayerSymbol == playerSymbols[0]) // Check which player is currently playing based on the symbol, and store the player's name in the string.
            {
                currentPlayerName = playerNames[0];
            }
            else
            {
                currentPlayerName = playerNames[1];
            }
            return currentPlayerName;
        }
        public void SwitchPlayer()
        {
            int currentPlayerIndex = Array.IndexOf(playerSymbols, currentPlayerSymbol); // Get the index of the current player in the playerSymbols array.

            // Toggle to the other player
            currentPlayerSymbol = playerSymbols[(currentPlayerIndex + 1) % playerSymbols.Length]; //we use the modulo to make sure the playerindex doesn't exceed the length of the first array and goes back to the first player if it does.
            if (moves == 0)
            {
                // If it's the first move in a new game, set the currentPlayerSymbol to the symbol of the first player.
                currentPlayerSymbol = playerSymbols[0];
            }
        }
        public bool CheckIfWin()
        {
            bool hasWon = false;

            //Check for horizontal win
            for (int row = 0; row < rows; row++) // Iterate through the rows first to look from bottom to top.
            {
                for (int col = 0; col <= columns - 4; col++) // Iterate through the columns to look from left to right.
                {
                    if (board[row, col] == currentPlayerSymbol &&     // Check if the current player has four coins in a row horizontally.
                        board[row, col + 1] == currentPlayerSymbol && // We check if the next three columns have the same symbol as the current player.
                        board[row, col + 2] == currentPlayerSymbol &&
                        board[row, col + 3] == currentPlayerSymbol)
                    {
                        hasWon = true;
                    }
                }
            }

            //Check for vertical win

            for (int row = 0; row <= rows - 4; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (board[row, col] == currentPlayerSymbol && // Check if the current player has four coins in a row veritically.
                        board[row + 1, col] == currentPlayerSymbol && // We check if the next three rows have the same symbol as the current player.
                        board[row + 2, col] == currentPlayerSymbol &&
                        board[row + 3, col] == currentPlayerSymbol)
                    {
                        return true;
                    }
                }
            }

            //Check for diagonal win
            for (int row = 0; row <= rows - 4; row++)
            {
                // Check for diagonal win from left to right.
                for (int col = 0; col <= columns - 4; col++)
                {
                    if (board[row, col] == currentPlayerSymbol && // Check if the current player has four coins in a row diagonally.
                        board[row + 1, col + 1] == currentPlayerSymbol && // We check if the next three rows and columns have the same symbol as the current player.
                        board[row + 2, col + 2] == currentPlayerSymbol &&
                        board[row + 3, col + 3] == currentPlayerSymbol)

                    {
                        hasWon = true;
                    }
                    // Check for diagonal win from right to left.
                    if (board[row + 3, col] == currentPlayerSymbol && //Bottom-left to top-right, 3 steps.
                          board[row + 2, col + 1] == currentPlayerSymbol && // Up-right, 2 up, 1 right.
                          board[row + 1, col + 2] == currentPlayerSymbol && //Up-right, 1 up, 2 right.
                          board[row, col + 3] == currentPlayerSymbol) //Top - right, 3 steps right.
                    {
                        hasWon = true;
                    }
                }
            }
            return hasWon;
        }
        //Check for a draw.
        public bool CheckForDraw()
        {
            bool isDraw = false;
            if (moves >= rows * columns) // Check if the number of moves equals the total number of cells on the board.
            {
                isDraw = true;
                Console.WriteLine("The board is full, it's a draw");
            }
            return isDraw;
        }
        // Run the Connect Four game.
        public void RunGame()
        {
            do
            {
                Console.Clear(); // Clear the console to make the game look better.
                DrawBoard(); // Draw the board first to show the player the current state of the board.

                Console.WriteLine($"It's {GetCurrentPlayer()}'s turn. Enter a column number (1-7) to place your coin.");
                PlayerInput(Console.ReadLine()); // Get the player's input and process it.
               
                string winnerText = $"time: {DateTime.Now}, {playerNames[0]} vs {playerNames[1]}/ winner is {GetCurrentPlayer()}! Congrats!"; // Create a string to store the winner's name and the time they won.
                File.AppendAllText("log.txt", winnerText + Environment.NewLine); // Append the winner's name and the time they won to the log file.
            } while (true);
        }

    }
}
