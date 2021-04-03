using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Animation;

namespace u20774312_INF154_Project
{
    public partial class formWordRichesGame : Form
    {
        public formWordRichesGame()
        {
            InitializeComponent();

            //sets the cursor for main buttons to hand cursor type
            btnStart.Cursor = Cursors.Hand;
            btnSettings.Cursor = Cursors.Hand;
            btnHowToPlay.Cursor = Cursors.Hand;
            btnHighScores.Cursor = Cursors.Hand;
            btnBackFromSettings.Cursor = Cursors.Hand;
            btnBackFromHowToPlay.Cursor = Cursors.Hand;
            btnBackFromHighScore.Cursor = Cursors.Hand;
            btnMenu0.Cursor = Cursors.Hand;
        }

        //sets default values for timeLeft and coins
        int timeLeft = 10, coins = 0;

        //list of words to use in the game
        String[] words = {"sharp", "dive", "sun", "cold", "flag", "lazy", "deal", "wet", "stop", "idea", 
                            "stir", "contain", "love", "new", "trap", "snag", "bed", "swim", "price",
                            "claim", "media", "social", "grapple", "climb", "president"};
        
        Random rnd = new Random();

        private void btnStart_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageGame;

            //resets all values to the default at the beginning of a new game
            timeLeft = 10;
            pBarTimer.ForeColor = Color.Gold;
            txtAnswer.Text = "";
            lblCoins.Text = "0";
            txtCountDown.Text = "10";
            pBarTimer.Value = 10;
            coins = 0;

            //chooses a random word out of the words array
            int i = rnd.Next(0, 25), wordLength;
            String chosenWord = words[i];
            wordLength = lblWord.Text.Length;

            //selects and removes a random letter from the chosen word
            int rndLetter = rnd.Next(0, wordLength - 1);
            char chosenLetter = chosenWord[rndLetter];
            lblMissingLetter.Text = chosenLetter.ToString();
            lblWord.Text = chosenWord.Replace(chosenLetter, '_');
        }

        private void btnHowToPlay_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageHowToPlay;
        }

        private void btnHighScores_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageHighScores;

            //runs if the listbox in the window has no values
            if(lBoxHighScores.Items.Count == 0)
            {
                string[] storeScores = new string[10];
                string temp;

                //adds each line from the text file into separate elements in the array storeScores
                storeScores = File.ReadAllLines("high_scores.txt");

                //sorts the array and reverses the order to display the scores from largest to smallest
                Array.Sort(storeScores);
                Array.Reverse(storeScores);

                //loop to add all elements of the storeScores array that are storing a score to the high scores listbox
                for (int j = 0; j < storeScores.Length; j++)
                {
                    //checks array element for a value to add to the listbox
                    if (storeScores[j] != "" && storeScores[j] != null)
                    {
                        lBoxHighScores.Items.Add(storeScores[j]);
                    }
                }
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageSettings;
        }

        private void btnBackFromSettings_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageMenu;
        }

        private void btnBackFromHowToPlay_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageMenu;
        }

        private void btnBackFromHighScore_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageMenu;

            //resets the name entered by the player once they have seen the high scores list
            txtName.Text = "";
            lBoxHighScores.Items.Clear();
        }

        private void txtAnswer_TextChanged(object sender, EventArgs e)
        {
            //starts the 10 second count down for the game and changes the colour of the progress bar
            timerCount.Start();
            pBarTimer.ForeColor = Color.Red;
        }

        private void timerCount_Tick(object sender, EventArgs e)
        {
            //control to make the count down tick down as long as the timer value is greater than 0
            if (timeLeft > 0)
            {
                timeLeft--;
                txtCountDown.Text = timeLeft.ToString();
                pBarTimer.PerformStep();
            }
            else
            {
                timerCount.Stop();
            }
        }

        private void txtCountDown_TextChanged(object sender, EventArgs e)
        {
            //performs an action when the timer equals 0
            if(txtCountDown.Text == "0")
            {
                /*checks to see which Game Over screen must be opened (Game over with a score vs Game over with 0 score)
                  depending on if the player got more than 0 coins, or 0 coins*/
                if (lblCoins.Text == "0")
                {
                    tControlPages.SelectedTab = tPageGameOver0;
                    lblCoinsTotal.Text = coins.ToString();
                }
                else
                {
                    tControlPages.SelectedTab = tPageGameOverScore;
                    lblCoinsFinalScore.Text = coins.ToString();
                }
                
            }
        }

        private void btnMenu0_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageMenu;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            tControlPages.SelectedTab = tPageHighScores;

            //checks to make sure the player has entered a name
            if(txtName.Text == null || txtName.Text == "")
            {
                MessageBox.Show("Please enter a name");
            }

            //control to check whether the player has beaten the game or not (coins >= 10)
            if(Convert.ToInt32(lblCoinsFinalScore.Text) < 10)
            {
                string[] storeScores = new string[10];
                string temp;

                //checks if the listbox has any stored values
                if(lBoxHighScores.Items.Count == 0)
                {
                    //adds each line from the text file into separate elements in the array storeScores
                    storeScores = File.ReadAllLines("high_scores.txt");

                    //sorts the array and reverses the order to display the scores from largest to smallest
                    Array.Sort(storeScores);
                    Array.Reverse(storeScores);

                    //loop to add all elements of the storeScores array that are storing a score to the high scores listbox
                    for (int j = 0; j < storeScores.Length; j++)
                    {
                        //checks array element for a value to add to the listbox
                        if (storeScores[j] != "" && storeScores[j] != null)
                        {
                            lBoxHighScores.Items.Add(storeScores[j]);
                        }
                    }
                }

                //checks whether the cap of 10 scores to fill the high scores list has been reached
                if(lBoxHighScores.Items.Count == 10)
                {
                    /*checks to see if the players score is higher than the lowest score in the high score list,
                      if so, the players score replaces the lowest score which will be sorted once the score has been saved*/
                    if(Convert.ToInt32(lblCoinsFinalScore.Text) > Convert.ToInt32(lBoxHighScores.Items[9].ToString().Substring(0, 1)))
                    {
                        lBoxHighScores.Items[9] = lblCoinsFinalScore.Text + "\t" + txtName.Text;
                    }
                    else
                    {
                        MessageBox.Show("Your score is too low to make it on to the list");
                    }
                }
                else
                {
                    lBoxHighScores.Items.Add(lblCoinsFinalScore.Text + "\t" + txtName.Text);
                }

                string[] saveScores = new string[10];

                //copies the scores from the high scores list into the array saveScores
                for (int x = 0; x < lBoxHighScores.Items.Count; x++)
                {
                    if (x != saveScores.Length)
                    {
                        saveScores[x] = lBoxHighScores.Items[x].ToString();
                    }
                    else
                    {
                        break;
                    }
                }

                //saves the updated high scores list to the text file
                File.WriteAllLines("high_scores.txt", saveScores);
            }

            /*if the number of coins is greater than or equal to 10, 
              it tells the player the beat the game and shows them the high scores list*/
            else
            {
                MessageBox.Show("Congratulations, you beat the game!!");

                string[] storeScores = new string[11];
                string temp;

                //adds each line from the text file into separate elements in the array storeScores
                storeScores = File.ReadAllLines("high_scores.txt");

                //sorts the array and reverses the order to display the scores from largest to smallest
                Array.Sort(storeScores);
                Array.Reverse(storeScores);

                //loop to add all elements of the storeScores array that are storing a score to the high scores listbox
                for (int j = 0; j < storeScores.Length; j++)
                {
                    //checks array element for a value to add to the listbox
                    if (storeScores[j] != "" && storeScores[j] != null)
                    {
                        lBoxHighScores.Items.Add(storeScores[j]);
                    }
                }
            }
        }

        private void btnChangeBackBlue_Click(object sender, EventArgs e)
        {
            //changes the background colour of every tab page to blue
            tPageGame.BackColor = Color.Blue;
            tPageGameOver0.BackColor = Color.Blue;
            tPageGameOverScore.BackColor = Color.Blue;
            tPageHighScores.BackColor = Color.Blue;
            tPageHowToPlay.BackColor = Color.Blue;
            tPageMenu.BackColor = Color.Blue;
            tPageSettings.BackColor = Color.Blue;
        }

        private void btnChangeBackRed_Click(object sender, EventArgs e)
        {
            //changes the background colour of every tab page to red
            tPageGame.BackColor = Color.Red;
            tPageGameOver0.BackColor = Color.Red;
            tPageGameOverScore.BackColor = Color.Red;
            tPageHighScores.BackColor = Color.Red;
            tPageHowToPlay.BackColor = Color.Red;
            tPageMenu.BackColor = Color.Red;
            tPageSettings.BackColor = Color.Red;
        }

        private void btnChangeBackGreen_Click(object sender, EventArgs e)
        {
            //changes the background colour of every tab page to green (default)
            tPageGame.BackColor = Color.Green;
            tPageGameOver0.BackColor = Color.Green;
            tPageGameOverScore.BackColor = Color.Green;
            tPageHighScores.BackColor = Color.Green;
            tPageHowToPlay.BackColor = Color.Green;
            tPageMenu.BackColor = Color.Green;
            tPageSettings.BackColor = Color.Green;
        }

        private void btnChangeBackGrey_Click(object sender, EventArgs e)
        {
            //changes the background colour of every tab page to grey
            tPageGame.BackColor = Color.Gray;
            tPageGameOver0.BackColor = Color.Gray;
            tPageGameOverScore.BackColor = Color.Gray;
            tPageHighScores.BackColor = Color.Gray;
            tPageHowToPlay.BackColor = Color.Gray;
            tPageMenu.BackColor = Color.Gray;
            tPageSettings.BackColor = Color.Gray;
        }

        private void btnChooseBackImage_Click(object sender, EventArgs e)
        {
            //opens a file dialog to select your background image
            OpenFileDialog newImageBack = new OpenFileDialog();
            newImageBack.ShowDialog();

            //filters the format of the files allowed to be selected for the background
            newImageBack.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";

            //tries to set the background of every tab page to the selected image
            try
            {
                tPageGame.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageGameOver0.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageGameOverScore.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageHighScores.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageHowToPlay.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageMenu.BackgroundImage = Image.FromFile(newImageBack.FileName);
                tPageSettings.BackgroundImage = Image.FromFile(newImageBack.FileName);
            }
            catch (Exception p)
            {
                
            }
            
        }

        private void txtAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            //control to check if the Enter Key is pressed once a letter has been typed in and the user would like to guess
            if (e.KeyCode == Keys.Enter)
            {
                //cancels the event created when the Enter Key has been pressed 
                e.Handled = true;

                //when Enter Key is pressed it will not effect the txtAnswer textbox
                e.SuppressKeyPress = true;

                //checks the letter entered by the player against the answer
                //if the letter entered is the correct answer, a new word is shown
                if (txtAnswer.Text == lblMissingLetter.Text)
                {
                    txtAnswer.Text = "";

                    //selects a random word from the word array
                    int i = rnd.Next(0, 25), wordLength;
                    String chosenWord = words[i];
                    wordLength = chosenWord.Length;
                    
                    //selects and removes a random letter from the word
                    int rndLetter = rnd.Next(0, wordLength - 1);
                    char chosenLetter = chosenWord[rndLetter];
                    lblMissingLetter.Text = chosenLetter.ToString();
                    lblWord.Text = chosenWord.Replace(chosenLetter, '_');

                    //adds the coins for the correct answer to your total
                    coins++;
                    lblCoins.Text = coins.ToString();
                }

                //if answer entered is wrong tells the player to try again
                else if(txtAnswer.Text != lblMissingLetter.Text)
                {
                    MessageBox.Show("Try Again");
                    txtAnswer.Text = "";
                }
            }
        }
    }
}
