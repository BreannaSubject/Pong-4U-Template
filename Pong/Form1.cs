﻿/*
 * Description:     A basic PONG simulator
 * Author:          Breanna Subject        
 * Date:            September 22nd 2020          
 */

#region libraries

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Media;

#endregion

namespace Pong
{
    public partial class Form1 : Form
    {
        #region global values

        //graphics objects for drawing
        SolidBrush brush = new SolidBrush(Color.White);
        SolidBrush powerbrush = new SolidBrush(Color.FromArgb(162,57,88));
        Font drawFont = new Font("Courier New", 18);
        Random random = new Random();
        int brushNum;
        

        // Sounds for game
        SoundPlayer scoreSound = new SoundPlayer(Properties.Resources.score);
        SoundPlayer collisionSound = new SoundPlayer(Properties.Resources.collision);

        //determines whether a key is being pressed or not
        Boolean aKeyDown, zKeyDown, jKeyDown, mKeyDown;

        // check to see if a new game can be started
        Boolean newGameOk = true;

        //ball directions, speed, and rectangle
        Boolean ballMoveRight = true;
        Boolean ballMoveDown = true;
        int ballSpeed = 4;
        Rectangle ball;
        const int B_CHANGE = 10;
        int bSign;

        //paddle speeds and rectangles
        int paddleSpeed = 6;
        Rectangle p1, p2;
        const int P_CHANGE = 10;
        int pSign;

        //player and game scores
        int player1Score = 0;
        int player2Score = 0;
        int gameWinScore = 5;  // number of points needed to win game

        //power ball rectangle 
        Rectangle power;
        const int SPEED = 10;
        Boolean powerMoveRight = true;
        Boolean powerMoveDown = true; 

        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //check to see if a key is pressed and set is KeyDown value to true if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = true;
                    break;
                case Keys.Z:
                    zKeyDown = true;
                    break;
                case Keys.J:
                    jKeyDown = true;
                    break;
                case Keys.M:
                    mKeyDown = true;
                    break;
                case Keys.Y:
                case Keys.Space:
                    if (newGameOk)
                    {
                        SetParameters();
                    }
                    break;
                case Keys.N:
                    if (newGameOk)
                    {
                        Close();
                    }
                    break;
            }
        }
        
        // -- YOU DO NOT NEED TO MAKE CHANGES TO THIS METHOD
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            //check to see if a key has been released and set its KeyDown value to false if it has
            switch (e.KeyCode)
            {
                case Keys.A:
                    aKeyDown = false;
                    break;
                case Keys.Z:
                    zKeyDown = false;
                    break;
                case Keys.J:
                    jKeyDown = false;
                    break;
                case Keys.M:
                    mKeyDown = false;
                    break;
            }
        }

        private void colorTimer_Tick(object sender, EventArgs e)
        {
            brushNum = random.Next(1, 13);

            switch (brushNum)
            {
                case 1:
                    brush = new SolidBrush(Color.LimeGreen);
                    break;
                case 2:
                    brush = new SolidBrush(Color.Aquamarine);
                    break;
                case 3:
                    brush = new SolidBrush(Color.PaleVioletRed);
                    break;
                case 4:
                    brush = new SolidBrush(Color.Crimson);
                    break;
                case 5:
                    brush = new SolidBrush(Color.Goldenrod);
                    break;
                case 6:
                    brush = new SolidBrush(Color.Orange);
                    break;
                case 7:
                    brush = new SolidBrush(Color.DarkSlateBlue);
                    break;
                case 8:
                    brush = new SolidBrush(Color.Lavender);
                    break;
                case 9:
                    brush = new SolidBrush(Color.MintCream);
                    break;
                case 10:
                    brush = new SolidBrush(Color.MistyRose);
                    break;
                case 11:
                    brush = new SolidBrush(Color.HotPink);
                    break;
                case 12:
                    brush = new SolidBrush(Color.White);
                    break;
                default:
                    brush = new SolidBrush(Color.White);
                    break;
            }
        }
            /// <summary>
            /// sets the ball and paddle positions for game start
            /// </summary>
            private void SetParameters()
        {
            if (newGameOk)
            {
                player1Score = player2Score = 0;
                newGameOk = false;
                startLabel.Visible = false;
                gameUpdateLoop.Start();
            }

            //set starting position for paddles on new game and point scored 
            const int PADDLE_EDGE = 20;  // buffer distance between screen edge and paddle            

            p1.Width = p2.Width = 10;    //height for both paddles set the same
            p1.Height = p2.Height = 80;  //width for both paddles set the same

            //p1 starting position
            p1.X = PADDLE_EDGE;
            p1.Y = this.Height / 2 - p1.Height / 2;

            //p2 starting position
            p2.X = this.Width - PADDLE_EDGE - p2.Width;
            p2.Y = this.Height / 2 - p2.Height / 2;

            // TODO set Width and Height of ball
            ball.Width = 20;
            ball.Height = 20;
            // TODO set starting X position for ball to middle of screen, (use this.Width and ball.Width)
            ball.X = this.Width / 2 - ball.Width / 2;
            // TODO set starting Y position for ball to middle of screen, (use this.Height and ball.Height)
            ball.Y = this.Height / 2 - ball.Height / 2;

            power.Width = 10;
            power.Height = 10;

            power.X = this.Width / 2 - power.Width / 2;
            power.Y = 20;

        }

        /// <summary>
        /// This method is the game engine loop that updates the position of all elements
        /// and checks for collisions.
        /// </summary>
        private void gameUpdateLoop_Tick(object sender, EventArgs e)
        {


            #region update ball/power position

            // TODO create code to move ball either left or right based on ballMoveRight and using BALL_SPEED
            if(ballMoveRight == true)
            {
                ball.X = ball.X + ballSpeed;
            }
            else 
            {
                ball.X = ball.X - ballSpeed;
            }
            // TODO create code move ball either down or up based on ballMoveDown and using BALL_SPEED

            if (ballMoveDown == true)
            {
                ball.Y = ball.Y + ballSpeed;
            }
            else
            {
                ball.Y = ball.Y - ballSpeed;
            }

            if (powerMoveRight == true)
            {
                power.X = power.X + SPEED;
            }
            else
            {
                power.X = power.X - SPEED;
            }

            if (powerMoveDown == true)
            {
                power.Y = power.Y + SPEED;
            }
            else
            {
                power.Y = power.Y - SPEED;
            }



            #endregion

            #region update paddle positions

            if (aKeyDown == true && p1.Y > 0)
            {
                p1.Y = p1.Y - paddleSpeed;
            }
            else
            {

            }

            if (zKeyDown == true && p1.Y < 400)
            {
                p1.Y = p1.Y + paddleSpeed;
            }
            else
            {

            }

            if (jKeyDown == true && p2.Y > 0)
            {
                p2.Y = p2.Y - paddleSpeed;
            }
            else
            {

            }


            if (mKeyDown == true && p2.Y < 400)
            {
                p2.Y = p2.Y + paddleSpeed;
            }
            else
            {

            }

            #endregion

            #region ball collision with top and bottom lines

            if (ball.Y < 0) // if ball hits top line
            {
                ballMoveDown = true;
                collisionSound.Play();
                bSign = random.Next(1, 3);
                pSign = random.Next(1, 3);

                if (bSign == 1 && ball.Width > 10)
                {
                    ball.Width = ball.Width - B_CHANGE;
                    ball.Height = ball.Height - B_CHANGE;
                }
                else if (bSign == 2 && ball.Width < 70)
                {
                    ball.Width = ball.Width + B_CHANGE;
                    ball.Height = ball.Height + B_CHANGE;
                }
                else
                {

                }
                if (pSign == 1 && p2.Height > 10 && p1.Height > 10)
                {
                    p2.Height = p2.Height - P_CHANGE;
                    p1.Height = p1.Height - P_CHANGE;
                }
                else if (pSign == 2 && p2.Height < 300 && p1.Height < 300)
                {
                    p2.Height = p2.Height + P_CHANGE;
                    p1.Height = p1.Height + P_CHANGE;
                }

            }
            else if (ball.Y > 430)
            {
                ballMoveDown = false;
                collisionSound.Play();
                bSign = random.Next(1, 3);

                if (bSign == 1 && ball.Width > 10)
                {
                    ball.Width = ball.Width - B_CHANGE;
                    ball.Height = ball.Height - B_CHANGE;
                }
                else if (bSign == 2 && ball.Width < 70)
                {
                    ball.Width = ball.Width + B_CHANGE;
                    ball.Height = ball.Height + B_CHANGE;
                }
                else
                {

                }
                if (pSign == 1 && p2.Height > 20)
                {
                    p2.Height = p2.Height - P_CHANGE;
                    p1.Height = p1.Height - P_CHANGE;
                }
                else if (pSign == 2 && p2.Height < 100)
                {
                    p2.Height = p2.Height + P_CHANGE;
                    p1.Height = p1.Height + P_CHANGE;
                }


            }
            else
            {

            }

            #endregion

            #region ball collision with paddles

            if (p1.IntersectsWith(ball))
            {
                if (pSign == 1)
                {
                    ballSpeed++;
                    paddleSpeed++;
                }
                else if (pSign == 2 && ballSpeed > 2)
                {
                    ballSpeed--;
                    paddleSpeed--;
                }
                collisionSound.Play();
                ballMoveRight = true;
            }
            else if (p2.IntersectsWith(ball))
            {
                
                collisionSound.Play();
                ballMoveRight = false;

                if (pSign == 1)
                {
                    ballSpeed++;
                    paddleSpeed++;
                }
                else if (pSign == 2 && ballSpeed > 2)
                {
                    ballSpeed--;
                    paddleSpeed--;
                }
            }
            else
            {

            }
           
            #endregion

            #region ball collision with side walls (point scored)

            if (ball.X < 0)  // ball hits left wall logic
            {
                scoreSound.Play();
                player2Score++;
                paddleSpeed = 6;
                ballSpeed = 4;
                
               
                

                if (player2Score == gameWinScore)
                {
                    GameOver("Player 2");
                }
                else
                {
                    ballMoveRight = false;
                    SetParameters();
                }

                

              
            }
            else if (ball.X > 616)
            {
                scoreSound.Play();
                player1Score++;
                paddleSpeed = 6;
                ballSpeed = 4;


                if (player1Score == gameWinScore)
                {
                    GameOver("Player 1");
                }
                else
                {
                    ballMoveRight = true;
                    SetParameters(); 
                }

            }






            #endregion

            #region power collision

            int powerUp = random.Next(1, 4);

            if (power.Y < 10)
            {
                powerMoveDown = true;

            }
            else if (power.Y > 430)
            {
                powerMoveDown = false; 
            }

            if (power.X < 10)
            {
                powerMoveRight = true;
            }
            else if (power.X > 610)
            {
                powerMoveRight = false; 
            }

            if (power.IntersectsWith(p1))
            {

                if (powerUp == 1)
                {
                    p1.Height = 300;
                }
                else if (powerUp == 2)
                {
                    brush = new SolidBrush(Color.Black);
                }
                else
                {
                    p2.Height = 30;
                }
            }
            else if (power.IntersectsWith(p2))
            {

                if (powerUp == 1)
                {
                    p2.Height = 300;
                }
                else if (powerUp == 2)
                {
                    brush = new SolidBrush(Color.Black);
                }
                else
                {
                    p1.Height = 30;
                }
            }
            #endregion

            //refresh the screen, which causes the Form1_Paint method to run
            this.Refresh();
        }
        
        /// <summary>
        /// Displays a message for the winner when the game is over and allows the user to either select
        /// to play again or end the program
        /// </summary>
        /// <param name="winner">The player name to be shown as the winner</param>
        private void GameOver(string winner)
        {
            gameUpdateLoop.Stop();
            newGameOk = true;
            startLabel.Visible = true;
            startLabel.BackColor = Color.Black;
            startLabel.Text = $"{winner} is the winner";
            startLabel.Refresh();
            Thread.Sleep(1000);
            startLabel.Text = "Play Again? (Y/N)";
            brush = new SolidBrush(Color.White);
            ballSpeed = 4;
            paddleSpeed = 6; 


            // TODO create game over logic
            // --- stop the gameUpdateLoop
            // --- show a message on the startLabel to indicate a winner, (need to Refresh).
            // --- pause for two seconds 
            // --- use the startLabel to ask the user if they want to play again

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(brush, p1);
            e.Graphics.FillRectangle(brush, p2);
            e.Graphics.FillRectangle(brush, ball);
            e.Graphics.FillRectangle(powerbrush, power);
            e.Graphics.DrawString(Convert.ToString(player1Score), drawFont, brush, 586, 10);
            e.Graphics.DrawString (Convert.ToString(player2Score), drawFont, brush, 10, 10);
        }

        }
    }
