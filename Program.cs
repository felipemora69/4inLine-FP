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


}

public class Player
{
    
}

public class Game
{

}

public class Program
{

}
