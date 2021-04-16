﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using WinFormsApp1;

namespace Game{
    public enum Movement {
        goLeft,
        goRight,
        Jump,
    }

    public enum ActionWithKey {
        Unpressed,
        Pressed,
        None,
    }
    
    public class Hero : DynamicObject {
        public Hero(int health, int speed, int jumpHeight, Point location, Size size)
            : base(health, speed, jumpHeight, location, size) {
            Tag = "hero";
            Image = new Bitmap(PathToImages + "hero.png",true);
        }
        public bool IsGoingLeft { get; set; }
        public bool IsGoingRight { get; set; }
        public bool IsJumping { get; set; }
        public bool IsLanded { get; set; }
        public int CurrentJumpHeight { get; set; }
        public bool IsMoving {
            get {
                return IsGoingLeft || IsGoingRight || IsJumping;
            }
        }
        public const int JumpLimit = 200;
        public int TempJumpLimit = JumpLimit;

        public new void Move() {
            if (IsGoingLeft) 
                this.Left -= this.Speed;
            if (IsGoingRight)
                this.Left += this.Speed;
            if (IsJumping && CurrentJumpHeight <= TempJumpLimit) {
                this.Top -= this.JumpHeight;
                CurrentJumpHeight += this.JumpHeight;
            }
            else if (CurrentJumpHeight > TempJumpLimit) {
                IsJumping = false;
            }
        }

        public void ProcessKeys(GameModel game, Keys key, ActionWithKey ActionWithKey) {
            switch (ActionWithKey) {
                case ActionWithKey.Pressed:
                    switch (key) {
                        case Keys.Left:
                            game.Hero.IsGoingLeft = true;
                            break;
                        case Keys.Right:
                            game.Hero.IsGoingRight = true;
                            break;
                        case Keys.Space:
                            if (game.Hero.IsLanded) {
                                game.Hero.IsJumping = true;
                                game.Hero.IsLanded = false;
                            }
                            break;
                    }
                    break;
                case ActionWithKey.Unpressed:
                    switch (key) {
                        case Keys.Left:
                            game.Hero.IsGoingLeft = false;
                            break;
                        case Keys.Right:
                            game.Hero.IsGoingRight = false;
                            break;
                        case Keys.Space:
                            if (game.Hero.IsJumping)
                                game.Hero.IsJumping = false;
                            break;
                    }
                    break;
            }
        }

        public void Action(GameModel game, Keys key, ActionWithKey actionWithKey) {
            ProcessKeys(game, key, actionWithKey);
            if (game.Hero.IsMoving) {
                game.Hero.Move();
                game.Hero.Refresh();
            }

            if (!game.Hero.IsJumping && !game.Hero.IsLanded) {
                game.Hero.Top += game.Hero.JumpHeight;
                game.Hero.Refresh();
            }
            game.Hero.IsLanded = false;
            foreach (Control obj in game.EnvironmentObjects) {
                if (obj is Platform && game.Hero.Bounds.IntersectsWith(obj.Bounds)) {
                    if (game.Hero.Bottom <= obj.Top + game.Hero.Height / 3d) {
                        game.Hero.Top = obj.Top - game.Hero.Height + 1;
                        game.Hero.Refresh();
                        game.Hero.IsLanded = true;
                        game.Hero.CurrentJumpHeight = 0;
                    }
                }

                if (obj is Platform && (game.Hero.Left <= obj.Right && game.Hero.Left >= obj.Left ||
                        game.Hero.Right >= obj.Left && game.Hero.Right <= obj.Right) &&
                    game.Hero.Top >= obj.Bottom) {
                    game.Hero.TempJumpLimit = game.Hero.Top - obj.Bottom > game.Hero.TempJumpLimit ?
                        Hero.JumpLimit :
                        game.Hero.Top - obj.Bottom + 35;
                    break;
                }
                game.Hero.TempJumpLimit = JumpLimit;
                if (obj is Platform && !game.Hero.IsLanded && game.Hero.Top <= obj.Bottom && game.Hero.Bottom >= obj.Top) {
                    if ((game.Hero.Left <= obj.Right && game.Hero.Left > obj.Left || 
                            game.Hero.Right >= obj.Left && game.Hero.Right < obj.Right)) {
                        if (game.Hero.IsGoingLeft) 
                            game.Hero.Left += 2;
                        if (game.Hero.IsGoingRight) 
                            game.Hero.Left -= 2;
                        game.Hero.IsGoingLeft = false;
                        game.Hero.IsGoingRight = false;
                        break;
                    }
                }
            }
        }
    }
}
