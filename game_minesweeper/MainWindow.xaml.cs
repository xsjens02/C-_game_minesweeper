using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace game_minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameTimer Timer;
        private GameBoard Board;
        public MainWindow()
        {
            InitializeComponent();
            Timer = new GameTimer(txtTime);
            Board = new GameBoard(9, 9, 10, 50, GameGrid, 450, 450);
        }

        /// <summary>
        /// Eventhandler when start game button is clicked
        /// Will start game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            Board.InitializeBoard();
            Board.GameOver -= StopGame;
            Board.GameOver += StopGame;
            Timer.StartTime();
        }

        /// <summary>
        /// Handles when game is over
        /// Will display a messagebox to player
        /// </summary>
        /// <param name="won">boolean to indicate if game is won or lost</param>
        /// <param name="msg">message to appear in messagebox</param>
        private void StopGame(bool won, string msg)
        {
            Timer.StopTime();
            txtTime.Text = "00:00";
            MessageBox.Show( won ? $"{msg}\nYour time is: {Timer.GetTime()}" : msg);
        }
    }
}