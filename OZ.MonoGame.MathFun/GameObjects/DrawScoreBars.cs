using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using OZ.MonoGame.MathFun.GameEngine;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.GameObjects.UI;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class DrawScoreBars : Control, IGameObject
    {
        private ControlApearance _scoreBarApearance;

        public ScoreBar Player1ScoreBar { get; private set; }
        public ScoreBar Player2ScoreBar { get; private set; }

        public ScoreBar TurnOf => Player1ScoreBar.IsMyTurn ? Player1ScoreBar : Player2ScoreBar;

        public float Margin { get; set; } = 10;

        public DrawScoreBars(GamePrototype gameParent) : base(gameParent)
        {
            BkgTransparent = true;
            _scoreBarApearance = new ControlApearance();

            Player1ScoreBar = new ScoreBar(gameParent)
            {
                IsLeft = true,
                ControlApearance = _scoreBarApearance
            }; 
            Player2ScoreBar = new ScoreBar(gameParent)
            {
                IsLeft = false,
                SpriteEffect = SpriteEffects.FlipHorizontally,
                ControlApearance = _scoreBarApearance
            };


            Controls.AddRange(Player1ScoreBar, Player2ScoreBar);
        }

        public void InitPlayers(Player player1, Player player2)
        {
            Player1ScoreBar.Player = player1;
            Player2ScoreBar.Player = player2;
        }

        private void CalculateLocationAndSize()
        {
            Texture2D scoreBarTexture = ScoreBar.Reg;

            //size
            float scale = 1;
            Vector2 playerSize = new Vector2(scoreBarTexture.Width, scoreBarTexture.Height);
            if (Size.Y < playerSize.Y)
            {
                scale = Size.Y / playerSize.Y;
                playerSize.Y *= scale;
                playerSize.X *= scale;
            }
            Player1ScoreBar.Size = Player2ScoreBar.Size = playerSize;

            //location
            Player1ScoreBar.Location = new Vector2(Margin, Margin);
            Player2ScoreBar.Location = new Vector2(Size.X - Margin  - playerSize.X, Margin);

        }


        public override void LoadContent(ContentManager content)
        {
            ScoreBar.Reg = content.Load<Texture2D>("sprites/leftPlayer");
            ScoreBar.Glow = content.Load<Texture2D>("sprites/leftPlayerGlow");
            _scoreBarApearance.Font = content.Load<SpriteFont>("fonts/playerFont");

            base.LoadContent(content);
            CalculateLocationAndSize();
        }
    }
}
