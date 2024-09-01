using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace game_minesweeper
{
    internal class GameBoard
    {
        public int Cols { get; set; }
        public int Rows { get; set; }
        public int SquareSize { get; set; }
        public Grid GameGrid { get; set; }
        public GameSquare[,] Squares { get; set; }
        public int AmountSquares { get; set; }
        public int AmountBombs { get; set; }
        public int RevealedSquares { get; set; }
        private readonly Random Random = new Random();

        public event Action<bool, string>? GameOver;

        /// <summary>
        /// Constructs a minesweeper gameboard object with specified dimensions and number of bombs
        /// </summary>
        /// <param name="cols">number of columns in the game board</param>
        /// <param name="rows">number of rows in the game board</param>
        /// <param name="amountBombs">number of boms to be placed on the gameboard</param>
        /// <param name="squareDimension">dimension of squares making grid</param>
        /// <param name="gameGrid">control to where the gameboard will be displayed</param>
        /// <param name="gridWidth">gamboard width</param>
        /// <param name="gridHeight">gameboard height</param>
        public GameBoard(int cols, int rows, int amountBombs, int squareDimension, Grid gameGrid, int gridWidth, int gridHeight)
        {
            this.Cols = cols;
            this.Rows = rows;
            this.AmountBombs = amountBombs;
            this.AmountSquares = cols * rows;
            this.SquareSize = squareDimension;
            this.GameGrid = gameGrid;
            this.GameGrid.Width = gridWidth;
            this.GameGrid.Height = gridHeight;
            this.Squares = new GameSquare[cols, rows];
        }

        /// <summary>
        /// Initializes gameboard by setting up grid according with instance variables
        /// </summary>
        public void InitializeBoard()
        {
            GameGrid.Children.Clear();
            GameGrid.RowDefinitions.Clear();
            GameGrid.ColumnDefinitions.Clear();
            for (int i = 0; i < Cols; i++)
            {
                this.GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (int i = 0; i < Rows; i++)
            {
                this.GameGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    GameSquare square = new GameSquare(this.SquareSize, this.SquareSize);
                    square.Click += SquareLeftClick;
                    square.MouseRightButtonDown += SquareRightClick;
                    Squares[i, j] = square;
                    Grid.SetColumn(square, i);
                    Grid.SetRow(square, j);
                    this.GameGrid.Children.Add(square);
                }
            }
            PopulateBoard();
        }

        /// <summary>
        /// Arranges bombs and square values according with instance variables
        /// </summary>
        private void PopulateBoard()
        {
            for (int i = 0; i < AmountBombs; i++)
            {
                bool bombSet = false;
                do
                {
                    int randomCol = Random.Next(1, Cols);
                    int randomRow = Random.Next(1, Rows);
                    GameSquare square = Squares[randomCol, randomRow];
                    if (!square.IsBomb)
                    {
                        square.IsBomb = true;
                        bombSet = true;
                    }
                } while (!bombSet);
            }

            for (int i = 0; i < Cols; i++)
            {
                for (int j = 0; j < Rows; j++)
                {
                    GameSquare square = Squares[i, j];
                    if (!square.IsBomb)
                        square.Adjacent = CountAdjacentBombs(i, j);
                }
            }
        }

        /// <summary>
        /// Counts a squares adjacent bombs
        /// </summary>
        /// <param name="col">square column index</param>
        /// <param name="row">square row index</param>
        /// <returns></returns>
        private int CountAdjacentBombs(int col, int row)
        {
            int bombCount = 0;
            for (int i = col - 1; i <= col + 1; i++)
            {
                for (int j = row - 1; j <= row + 1; j++)
                {
                    if (i >= 0 && i < this.Cols && j >= 0 && j < this.Rows && !(i == col && j == row))
                    {
                        GameSquare square = this.Squares[i, j];
                        if (square.IsBomb)
                            bombCount++;
                    }
                }
            }
            return bombCount;
        }

        /// <summary>
        /// Eventhandler for left mouse click on a square 
        /// Reveals square content and handles logic
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareLeftClick(object sender, RoutedEventArgs e)
        {
            GameSquare? clickedSquare = sender as GameSquare;
            if (clickedSquare != null && !(clickedSquare.IsRevealed))
            {
                if (clickedSquare.IsBomb)
                {
                    clickedSquare.RevealSquare("\uD83D\uDCA3", Colors.DarkRed);
                    GameOver?.Invoke(false, "Bad luck! You lose!");
                }
                else
                {
                    if (clickedSquare.Adjacent > 0)
                    {
                        clickedSquare.RevealSquare(clickedSquare.Adjacent.ToString(), Colors.DarkGray);
                        CheckForWin();
                    }
                    else
                        RevealEmptyArea(Grid.GetColumn(clickedSquare), Grid.GetRow(clickedSquare));
                }
            }
        }

        /// <summary>
        /// Reveal empty area adjacent to a specified square on gameboard
        /// </summary>
        /// <param name="col"> square coloumn index</param>
        /// <param name="row"> square row index</param>
        private void RevealEmptyArea(int col, int row)
        {
            {
                if (col < 0 || col >= this.Cols || row < 0 || row >= this.Rows)
                    return;

                GameSquare square = this.Squares[col, row];
                if (square.IsRevealed)
                    return;

                if (square.Adjacent > 0)
                    square.RevealSquare(square.Adjacent.ToString(), Colors.DarkGray);
                else
                {
                    square.RevealSquare("", Colors.DarkGray);
                    RevealEmptyArea(col - 1, row);
                    RevealEmptyArea(col + 1, row);
                    RevealEmptyArea(col, row - 1);
                    RevealEmptyArea(col, row + 1);

                    RevealEmptyArea(col - 1, row - 1);
                    RevealEmptyArea(col + 1, row - 1);
                    RevealEmptyArea(col - 1, row + 1);
                    RevealEmptyArea(col + 1, row + 1);
                }
                CheckForWin();
            }
        }

        /// <summary>
        /// Eventhandler for right mouse click on a square
        /// Flags a not revealed square
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SquareRightClick(object sender, MouseButtonEventArgs e)
        {
            GameSquare? clickedSquare = sender as GameSquare;
            if (clickedSquare != null && !(clickedSquare.IsRevealed))
                clickedSquare.FlagSquare("\uD83D\uDEA9");
        }

        /// <summary>
        /// Checks if all squares not containning a bomb has been revealed 
        /// Will invoke event gameover if all squares has been revealed 
        /// </summary>
        private void CheckForWin()
        {
            int revealedSquares = 0;
            foreach (GameSquare square in this.Squares)
            {
                if (square.IsRevealed && !square.IsBomb)
                    revealedSquares++;
            }
            if (revealedSquares == (Squares.Length - AmountBombs))
                GameOver?.Invoke(true, "Gongratulations! You Win!");
        }
    }
}
