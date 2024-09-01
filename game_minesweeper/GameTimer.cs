using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace game_minesweeper
{
    internal class GameTimer
    {
        private DispatcherTimer Timer;
        private TimeSpan Time;
        private TextBox TimerTextBox;
        
        /// <summary>
        /// Constructs a gametimer object
        /// </summary>
        /// <param name="timerTextBox">control to where the timer will be displayed</param>
        public GameTimer(TextBox timerTextBox)
        {
            this.Timer = new DispatcherTimer();
            this.Time = new TimeSpan();
            this.TimerTextBox = timerTextBox;
            this.Timer.Interval = TimeSpan.FromSeconds(1);
            this.Timer.Tick += UpdateTimeTextBox;
        }

        /// <summary>
        /// Updates control displaying time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimeTextBox(object? sender, EventArgs e)
        {
            Time = Time.Add(TimeSpan.FromSeconds(1));
            TimerTextBox.Text = Time.ToString(@"mm\:ss");
        }

        /// <summary>
        /// Starts timer
        /// </summary>
        public void StartTime()
        {
            Time = TimeSpan.Zero;
            Timer.Start();
        }

        /// <summary>
        /// Stops timer
        /// </summary>
        public void StopTime()
        {
            Timer.Stop();
        }

        /// <summary>
        /// Gets gurrent time from timer
        /// </summary>
        /// <returns>current time on timer in format mm:ss</returns>
        public string GetTime()
        {
            return Time.ToString(@"mm\:ss");
        }
    }
}
