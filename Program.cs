namespace PRB99.ASN.FourInOneRow
{
    internal class Program
    {
        private int rows = 6;
        private int columns = 7;
        private char[,] board;
        private string[] playerNames = new string[2];
        private char[] playerSymbols = { 'R', 'G' };
        private char currentPlayer = 'R'; // Initializing the current player with red because red always starts.
        private int moves = 0;
        static void Main(string[] args)
        {
            Program game1 = new Program();

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
            Console.WriteLine("    1    2    3    4    5    6    7"); // Indicate which column has which number.

            for (int i = 0; i < rows; i++) // Iterate through the rows to place symbols in each column.
            {
                Console.WriteLine("  -----------------------------------");
                for (int j = 0; j < columns; j++) // Iterate through the columns to place symbols. Use a double loop to fill rows first and then columns.
                {
                    Console.Write($"  | {board[i, j]} ");
                }
                Console.WriteLine(" |"); // Close with a | to format the board correctly.
            }
        }

        // Place coins in the specified column.
        public void PlaceCoins(int column)
        {
            for (int i = rows - 1; i >= 0; i--) // Search from bottom to top for an empty spot, hence use -1 to move upwards in rows.
            {
                if (board[i, column - 1] == '\0') // Check if the spot is empty; if yes, place the coin of the current player. '\0' represents an empty spot in a char array.
                {
                    board[i, column - 1] = currentPlayer; // Use -1 because the array starts at 0 while the user starts at 1.
                    moves++; // Increment the move count to check later if the board is full.
                    break;
                }
                else
                {
                    Console.WriteLine("You cannot make a move here, try again");
                }
            }
        }

        // Process player input.
        public void PlayerInput(string input)
        {
            int column;

            if (IsValidInput(input, out column)) // First, check if the player provides valid input using the IsValidInput function.
            {
                if (!IsColumnFull(column)) // Then, check if the columns are full using the IsColumnFull function.
                {
                    PlaceCoins(column); // If both conditions are true, place the coin of the current player.
                }
                else
                {
                    Console.WriteLine("Invalid move. Column is full. Try again."); // If the column is full, display an error message.
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a valid column (1-7). Try again."); // If the input is not valid, display an error message.
            }
        }

        // Check if the input for columns is valid. Note: We check input for columns, not rows, because in Connect Four, you can only place a coin at the top, not on the sides.
        public bool IsValidInput(string input, out int column)
        {
            column = 0;
            return int.TryParse(input, out column) && column >= 1 && column <= columns; // This returns true if the input is an integer and is between 1 and 7.
        }   // We use an out int because we want to store the column in the function and use it in the PlaceCoins function.

        // Check for a draw in the game.
        public bool CheckForDraw()
        {
            bool isDraw = false;
            if (moves > 42) // The maximum number of moves a Connect Four game can have with this board is 42 (calculated by 7*6).
            {
                isDraw = true;
            }
            if (isDraw == true)
            {
                Console.WriteLine("The board is full, it's a draw");
            }
            return isDraw;
        }

        // Check if the specified row in the column is full.
        public bool CheckIfRowFull(int column)
        {
            for (int i = rows - 1; i >= 0; i--)
            {
                if (board[i, column] == '\0')
                {
                    return false;
                }
            }
            return true;
        }

        // Check if the specified column is full.
        public bool IsColumnFull(int column)
        {
            for (int i = 0; i < rows; i++)
            {
                if (board[i, column] == '\0')
                {
                    return false;
                }
            }
            return true;
        }

        // Get the name of the current player.
        public string GetCurrentPlayer()
        {
            string currentPlayerName = "";   // Create an empty string to store the name of the current player.
            if (currentPlayer == playerSymbols[0]) // Check which player is currently playing based on the symbol, and store the player's name in the string.
            {
                currentPlayerName = playerNames[0];
            }
            else
            {
                currentPlayerName = playerNames[1];
            }
            return currentPlayerName;
        }

        // Run the Connect Four game.
        public void RunGame()
        {
            GetNames();
            do
            {
                DrawBoard();
                Console.WriteLine($"It's {GetCurrentPlayer()}'s turn. Enter a column number (1-7) to place your coin.");
                PlayerInput();
                CheckIfRowFull();
                IsColumnFull();
            } while (!CheckForDraw());
        }
    }
}

// Sources: ChatGPT => Used to review my code, check if I was on the right track, understand how to use functions, and clean up my code.
