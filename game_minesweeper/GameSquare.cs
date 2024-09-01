using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace game_minesweeper
{
    internal class GameSquare : Button
    {
        public bool IsBomb { get; set; }
        public bool IsFlagged { get; set; }
        public int Adjacent { get; set; }
        public bool IsRevealed { get; set; }

        /// <summary>
        /// Constructs a square object with specified dimensions
        /// </summary>
        /// <param name="width">square widt</param>
        /// <param name="height">square height</param>
        public GameSquare(int width, int height)
        {
            this.IsBomb = false;
            this.IsFlagged = false;
            this.Adjacent = 0;
            this.IsRevealed = false;
            this.Width = width;
            this.Height = height;
        }

        /// <summary>
        /// Will display content on a square object 
        /// </summary>
        /// <param name="content">content to be displayed on square</param>
        /// <param name="color">new background color of square</param>
        public void RevealSquare(string content, System.Windows.Media.Color color)
        {
            this.Content = content;
            this.Background = new SolidColorBrush(color);
            this.IsRevealed = true;
        }

        /// <summary>
        /// Will flag/unflag a square 
        /// </summary>
        /// <param name="content">content to be displayed when flagging</param>
        public void FlagSquare(string content)
        {
            if (this.IsFlagged)
            {
                this.Content = "";
                this.IsFlagged = false;
            }
            else
            {
                this.Content = content;
                this.IsFlagged = true;
            }
        }
    }
}
