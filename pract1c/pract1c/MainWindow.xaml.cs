using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace pract1c
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private bool playerTurn;
        private bool endGame;
        private string[] buttonContents;

        public string[] ButtonContents
        {
            get { return buttonContents; }
            set
            {
                buttonContents = value;
                NotifyPropertyChanged();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            InitializeGame();
            RandomlyAssignSymbol();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InitializeGame()
        {
            playerTurn = true;
            endGame = false;
            ButtonContents = new string[9];
        }

        private void RandomlyAssignSymbol()
        {
            playerTurn = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (endGame) return;
            Button button = (Button)sender;

            if (button.Content == null || button.Content.ToString() == "")
            {
                string currentPlayerSymbol = playerTurn ? "X" : "O";
                button.Content = currentPlayerSymbol;

                int row = Grid.GetRow(button);
                int column = Grid.GetColumn(button);
                int index = row * 3 + column;

                ButtonContents[index] = currentPlayerSymbol;
                button.IsEnabled = false;
                playerTurn = !playerTurn;
                ProverkaStatusaIgri();

                if (!playerTurn && !endGame)
                {
                    BotTurn();
                }
            }
        }

        private void ProverkaStatusaIgri()
        {
            for (int i = 0; i < 3; i++)
            {
                int row = i * 3;
                if (!string.IsNullOrEmpty(ButtonContents[row]) && ButtonContents[row] == ButtonContents[row + 1] && ButtonContents[row] == ButtonContents[row + 2])
                {
                    GameEnd(ButtonContents[row]);
                    return;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                if (!string.IsNullOrEmpty(ButtonContents[i]) && ButtonContents[i] == ButtonContents[i + 3] && ButtonContents[i] == ButtonContents[i + 6])
                {
                    GameEnd(ButtonContents[i]);
                    return;
                }
            }

            for (int i = 0; i < 3; i++)
            {
                int column = i;
                if (!string.IsNullOrEmpty(ButtonContents[column]) && ButtonContents[column] == ButtonContents[column + 3] && ButtonContents[column] == ButtonContents[column + 6])
                {
                    GameEnd(ButtonContents[column]);
                    return;
                }
            }

            if (!string.IsNullOrEmpty(ButtonContents[0]) && ButtonContents[0] == ButtonContents[4] && ButtonContents[0] == ButtonContents[8])
            {
                GameEnd(ButtonContents[0]);
                return;
            }

            if (!string.IsNullOrEmpty(ButtonContents[2]) && ButtonContents[2] == ButtonContents[4] && ButtonContents[2] == ButtonContents[6])
            {
                GameEnd(ButtonContents[2]);
                return;
            }

            if (ButtonContents.All(content => !string.IsNullOrEmpty(content)))
            {
                GameEnd("Ничья");
            }
        }

        private void GameEnd(string winner)
        {
            endGame = true;
            MessageBox.Show("Игра окончена. Победитель: " + winner);
            foreach (var button in game.Children.OfType<Button>().Where(b => b.Name != "restartButton"))
            {
                button.IsEnabled = false;
            }
            restartButton.IsEnabled = true;
        }

        private void BotTurn()
        {
            for (int i = 0; i < 9; i++)
            {
                if (string.IsNullOrEmpty(ButtonContents[i]))
                {
                    ButtonContents[i] = "O";
                    Button button = (Button)game.FindName("bt" + i);
                    if (button != null)
                    {
                        button.Content = "O";
                        button.IsEnabled = false;
                    }
                    playerTurn = true;
                    ProverkaStatusaIgri();
                    break;
                }
            }
        }

        private void Restart_Click(object sender, RoutedEventArgs e)
        {
            InitializeGame();
            RandomlyAssignSymbol();
            ClearGameBoard();
        }

        private void ClearGameBoard()
        {
            foreach (var button in game.Children.OfType<Button>().Where(b => b.Name != "restartButton"))
            {
                button.Content = null;
                button.IsEnabled = true;
            }
        }
    }
}