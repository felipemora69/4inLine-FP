using System;

public class Board
{
    private char?[,] grid;

    // Constructor to initialize the game board with empty spaces
    public Board()
    {
        // Create a 6 rows x 7 columns grid
        grid = new char?[6, 7];
    }

    public void Display()
    {
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                Console.Write("| " + (grid[row, col]?.ToString() ?? " ") + " ");
            }
            // End the row with a "|" to define the border of the board
            Console.WriteLine("|");
        }
        // Display column numbers below the board for player reference
        Console.WriteLine("| 1 | 2 | 3 | 4 | 5 | 6 | 7 |");
    }

    public bool DropPiece(int col, char symbol)
    {
        // Start from the bottom row and move upwards to find the first available space in the column rule
        for (int row = 5; row >= 0; row--)
        {
            if (grid[row, col] == null)
            {
                grid[row, col] = symbol;//Place the symbol of the players
                return true; // Piece dropped successfully
            }
        }
        return false;
    }
    public bool IsFull()
    {
        // Check if the top row of each column is completely filled (no null values)
        for (int col = 0; col < 7; col++)
        {
            if (grid[0, col] == null)
                return false;
        }
        return true; // Board is full
    }

    // Method to check if the specified player has won the game
    public bool CheckWinner(char symbol)
    {
        // Check horizontal win
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (grid[row, col] == symbol &&
                    grid[row, col + 1] == symbol &&
                    grid[row, col + 2] == symbol &&
                    grid[row, col + 3] == symbol)
                {
                    return true; // Horizontal win
                }
            }
        }

        // Check vertical win
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                if (grid[row, col] == symbol &&
                    grid[row + 1, col] == symbol &&
                    grid[row + 2, col] == symbol &&
                    grid[row + 3, col] == symbol)
                {
                    return true; // Vertical win
                }
            }
        }

        // Check diagonal (positive slope) win
        for (int row = 0; row < 3; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                if (grid[row, col] == symbol &&
                    grid[row + 1, col + 1] == symbol &&
                    grid[row + 2, col + 2] == symbol &&
                    grid[row + 3, col + 3] == symbol)
                {
                    return true; // Diagonal (positive slope) win
                }
            }
        }

        // Check diagonal (negative slope) win
        for (int row = 0; row < 3; row++)
        {
            for (int col = 3; col < 7; col++)
            {
                if (grid[row, col] == symbol &&
                    grid[row + 1, col - 1] == symbol &&
                    grid[row + 2, col - 2] == symbol &&
                    grid[row + 3, col - 3] == symbol)
                {
                    return true; // Diagonal (negative slope) win
                }
            }
        }

        return false;
    }
}

public class Player
{
    public string Name { get; }
    public char Symbol { get; }//represents the symbol of the player

    //get the name and symbol of the players
    public Player(string name, char symbol)
    {
        Name = name;
        Symbol = symbol;
    }

    public int MakeMove()
    {
        while (true)
        {
            Console.Write($"{Name}'s turn ({Symbol}): Enter column (1-7): ");//Prompt for column input
            if (int.TryParse(Console.ReadLine(), out int col) && col >= 1 && col <= 7)
            {
                return col - 1;//Convert input (1-7) to zero-based index (0-6)
            }
            else
            {
                Console.WriteLine("Invalid column. Please try again.");//Display error message
            }
        }
    }
}

public class Game
{
    private Board board;
    private Player[] players;
    private int currentPlayerIndex;

    public Game(Player player1, Player player2)
    {
        board = new Board();
        players = new Player[] { player1, player2 };
        currentPlayerIndex = 0;//Start with the first player
    }

    public void Start()
    {
        Console.WriteLine("Connect 4 Game Development Project:");

        bool gameEnded = false;

        while (!gameEnded)
        {
            // Display the current state of the game board
            Console.WriteLine();
            board.Display();

            //Get the current player
            Player currentPlayer = players[currentPlayerIndex];

            //Prompt the player for a column to drop their piece
            int column;
            do
            {
                Console.Write($"It is {currentPlayer.Name}'s turn ({currentPlayer.Symbol}): Enter column (1-7): ");
                if (!int.TryParse(Console.ReadLine(), out column) || column < 1 || column > 7)
                {
                    Console.WriteLine("Invalid input. Please enter a valid column number (1-7).");
                    continue;
                }

                column--;
            } while (!board.DropPiece(column, currentPlayer.Symbol));

            //Check for a win
            if (board.CheckWinner(currentPlayer.Symbol))
            {
                Console.WriteLine();
                board.Display();
                Console.WriteLine($"It is a Connect 4. {currentPlayer.Name} wins!");

                gameEnded = !PromptRestart();
            }
            else if (board.IsFull())
            {
                Console.WriteLine();
                board.Display();
                Console.WriteLine("It's a draw! The board is full.");

                //Prompt for restart
                gameEnded = !PromptRestart();
            }
            else
            {
                //Switch to the next player
                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
        }
    }

    private bool PromptRestart()
    {
        Console.Write("Restart? Yes(1) No(0): ");
        int choice;
        if (int.TryParse(Console.ReadLine(), out choice))
        {
            if (choice == 1)
            {
                //Reset game state for a new game
                ResetGame();
                return true;//Continue with a new game
            }
        }
        return false;
    }

    private void ResetGame()
    {
        //Clear the game board
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                board.DropPiece(col, null);// Drop null (clear) piece at each column
            }
        }

        // Reset current player index to start with the first player again
        currentPlayerIndex = 0;
    }
}

public class Program
{
    static void Main(string[] args)
    {
        // Prompt for player names and create Player instances with assigned symbols
        Console.Write("Enter Player 1's name: ");
        string player1Name = Console.ReadLine();
        Player player1 = new Player(player1Name, 'X'); // First player always has symbol 'X'

        Console.Write("Enter Player 2's name: ");
        string player2Name = Console.ReadLine();
        Player player2 = new Player(player2Name, 'O'); // Second player always has symbol 'O'

        // Create a new Game instance with the two players
        Game game = new Game(player1, player2);

        // Start the game
        game.Start();
    }
}
