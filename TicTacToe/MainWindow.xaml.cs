using System.Data.Common;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Holds the current results of cells in the active game
        /// </summary>
        internal MarkType[] mResults;

        /// <summary>
        /// True if it's player 1's turn (X) or player 2's turn (O)
        /// </summary>
        internal bool mPlayerTurn;

        /// <summary>
        /// true if the game has ended
        /// </summary>
        internal bool mGameEnded;

        /// <summary>
        ///  Attributes from the class
        /// </summary>
        private Button button;

        private object sender;

        private int column;

        private int row;

        private int index;


        /// <summary>
        /// Default Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //new CreatesNewGame().NewGame();
            NewGame();
        }
        internal void NewGame()
        {
            //Array of cells
            mResults = new MarkType[9];

            for (var i = 0; i < mResults.Length; i++)
                mResults[i] = MarkType.Free;

            // make sure player 1 starts the game
            mPlayerTurn = true;

            //interate every button on the grid
            Container.Children.Cast<Button>().ToList().ForEach(button =>
            {
                //set default colors
                button.Content = string.Empty;
                button.Background = Brushes.White;
                button.Foreground = Brushes.Blue;
            });

            //make sure the game hasn't ended
            mGameEnded = false;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfGameEnded())
            {
                NewGame();
            }
            else
            {
                this.sender = sender;

                FindButtonPosition();

                GetIndex();

                if (!CheckIfIndexAlreadyHasValue())
                {
                    SetCellValue();

                    ChangeForegroundBasedOnPlayersTurn();

                    TogglePlayersTurn();

                    CheckIfHasWinner();
                }
            }
        }
        private bool CheckIfGameEnded()
        {
            return mGameEnded;
        }
        private void FindButtonPosition()
        {
            //cast the sender to a button
            button = (Button)sender;

            //find the buttons position in the array
            column = Grid.GetColumn(button);
            row = Grid.GetRow(button);
        }
        private void GetIndex()
        {
            index = column + (row * 3);
        }
        private bool CheckIfIndexAlreadyHasValue()
        {
            // don't do anything if the cell already has a value in it
            if (mResults[index] != MarkType.Free)
                return true;
            return false;
        }
        private void SetCellValue()
        {
            //set the cell value based on wich player turn it's
            mResults[index] = mPlayerTurn ? MarkType.Cross : MarkType.Nought;

            //set button text to the result
            button.Content = mPlayerTurn ? "X" : "O";
        }
        private void ChangeForegroundBasedOnPlayersTurn()
        {
            //change noughts to red
            if (!mPlayerTurn)
                button.Foreground = Brushes.Red;
        }
        private void TogglePlayersTurn()
        {
            //toggle the players turns
            mPlayerTurn ^= true;
        }
        private void CheckIfHasWinner()
        {
            //check for a winner
            CheckForAWinner();
        }
        internal void CheckForAWinner()
        {
            if (!CheckHorizontalWinner())
                if (!CheckVerticalWinner())
                    if (!CheckDiagonalWinner())
                        if (!NoWinnerCheck()) ;
        }
        private bool CheckHorizontalWinner()
        {
            //check for horizontal wins
            //row 0

            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[1] & mResults[2]) == mResults[0])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button0_0.Background = Button1_0.Background = Button2_0.Background = Brushes.Green;
            }

            //row 1
            if (mResults[3] != MarkType.Free && (mResults[3] & mResults[4] & mResults[5]) == mResults[3])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button0_1.Background = Button1_1.Background = Button2_1.Background = Brushes.Green;
            }

            //row 2
            if (mResults[6] != MarkType.Free && (mResults[6] & mResults[7] & mResults[8]) == mResults[6])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button0_2.Background = Button1_2.Background = Button2_2.Background = Brushes.Green;
            }
            return mGameEnded;
        }

        private bool CheckVerticalWinner()
        {
            //check for vertical wins
            //column 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[3] & mResults[6]) == mResults[0])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button0_0.Background = Button0_1.Background = Button0_2.Background = Brushes.Green;

            }

            //column 1
            if (mResults[1] != MarkType.Free && (mResults[1] & mResults[4] & mResults[7]) == mResults[1])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button1_0.Background = Button1_1.Background = Button1_2.Background = Brushes.Green;

            }

            //column 2
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[5] & mResults[8]) == mResults[2])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button2_0.Background = Button2_1.Background = Button2_2.Background = Brushes.Green;

            }
            return mGameEnded;
        }
        private bool CheckDiagonalWinner()
        {
            //check for diagonal wins
            //diagonal 0
            if (mResults[0] != MarkType.Free && (mResults[0] & mResults[4] & mResults[8]) == mResults[0])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button0_0.Background = Button1_1.Background = Button2_2.Background = Brushes.Green;

            }

            //diagonal 2
            if (mResults[2] != MarkType.Free && (mResults[2] & mResults[4] & mResults[6]) == mResults[2])
            {
                mGameEnded = true;

                //highlight winning cells in green
                Button2_0.Background = Button1_1.Background = Button0_2.Background = Brushes.Green;

            }
            return mGameEnded;
        }
        private bool NoWinnerCheck()
        {
            //check for no winner and full board
            if (!mResults.Any(result => result == MarkType.Free))
            {
                mGameEnded = true;

                //turn all cells orange
                Container.Children.Cast<Button>().ToList().ForEach(button =>
                {
                    //set default colors
                    button.Background = Brushes.Orange;
                });
            }
            return mGameEnded;
        }
    }
}
