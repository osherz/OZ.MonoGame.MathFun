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
    public class ScoreBar : Control, IGameObject
    {
        public static readonly Rectangle SCORE_RECT = new Rectangle(26, 26, 66, 66);
        public static readonly Rectangle PLAYER_NAME_RECT = new Rectangle(120, 28, 175, 60);

        private Player _player;
        public Player Player
        {
            get => _player;
            set
            {
                _player = value;
                _scoreLabel.Player = value;
                _playerNameLabel.Text = _player.Name;
            }
        }

        private Label _playerNameLabel;
        private ScoreLabel _scoreLabel;

        public Color ForeColor
        {
            get => _playerNameLabel.ForeColor;
            set
            {
                _playerNameLabel.ForeColor = _scoreLabel.ForeColor = value;
            }
        }

        public override GamePrototype GameParent
        {
            get => base.GameParent;
            set => base.GameParent = value;
        }

        public static SpriteFont Font { get; set; }

        private bool _isMyTurn;
        public bool IsMyTurn
        {
            get => _isMyTurn;
            set
            {
                if (IsMyTurn != value)
                {
                    _isMyTurn = value;
                    OnTurnChanged();
                }
            }
        }


        public bool IsLeft { get; set; }

        public Vector2 MiddleOfScore
        {
            get
            {
                Vector2 midLocation = Location;
                midLocation.Y += (_scoreLabel.Location.Y + _scoreLabel.Size.Y / 2);
                midLocation.X += (_scoreLabel.Location.X + _scoreLabel.Size.X / 2);

                return midLocation;
            }
        }


        #region Events
        public event EventHandler TurnChanged;
        #endregion

        #region Raise Events Methods
        protected virtual void OnTurnChanged()
        {
            RegTexture = IsMyTurn ? UI.UIApearance.GlowScoreBar : UI.UIApearance.ScoreBar;
            TurnChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public ScoreBar(GamePrototype gameParent) : base(gameParent)
        {
            BlackTextureWhenNotEnabled = false;

            _playerNameLabel = new Label(gameParent)
            {
                Parent = this,
                TextAnchor = Anchor.Center | Anchor.Middle,
                BkgTransparent = true,
            };

            _scoreLabel = new ScoreLabel(gameParent)
            {
                Parent = this,
                TextAnchor = Anchor.Center | Anchor.Middle,
                BkgTransparent = true,
            };

            ForeColor = Color.White;

            Controls.AddRange(_playerNameLabel, _scoreLabel);

            LookingChanged += (sender, e) =>
              {
                  _playerNameLabel.Size *= Scale;
                  _scoreLabel.Size *= Scale;

                  CalculateLocationOfPlayerName();
                  CalculateLocationOfScore();

              };
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void LoadContent(ContentManager content)
        {
            RegTexture = UI.UIApearance.ScoreBar;
            _playerNameLabel.Font = Font;
            _scoreLabel.Font = Font;
            base.LoadContent(content);
        }

        private void CalculateLocationOfPlayerName()
        {
            Rectangle rect = CalculateLocationOf(PLAYER_NAME_RECT);
            _playerNameLabel.Location = rect.Location.ToVector2();
            _playerNameLabel.Size = rect.Size.ToVector2();
        }

        private void CalculateLocationOfScore()
        {
            Rectangle rect = CalculateLocationOf(SCORE_RECT);
            _scoreLabel.Location = rect.Location.ToVector2();
            _scoreLabel.Size = rect.Size.ToVector2();
        }

        private Rectangle CalculateLocationOf(Rectangle dest)
        {

            Vector2 location = dest.Location.ToVector2() * Scale;
            Vector2 size = dest.Size.ToVector2() * Scale;

            Rectangle rect = new Rectangle(location.ToPoint(), size.ToPoint());

            if (!IsLeft)
            {
                rect.X = (int)(Size.X - (location.X + size.X));
            }

            return rect;
            //Vector2 strSize = Font.MeasureString(str);
            //scale = 1;

            //if (strSize.X > dest.Width)
            //{
            //    scale = dest.Width / strSize.X;
            //    strSize.X *= scale;
            //    strSize.Y *= scale;
            //}

            //if (strSize.Y > dest.Height)
            //{
            //    scale = dest.Height / strSize.Y;
            //    strSize.X *= scale;
            //    strSize.Y *= scale;
            //}

            //return new Vector2()
            //{
            //    X = dest.X + (dest.Width - strSize.X) / 2,
            //    Y = dest.Y + (dest.Height - strSize.Y) / 2
            //};

        }


        //public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Draw(ScoreBarToDraw,
        //                        Location,
        //                        null,
        //                        Color.White,
        //                        0,
        //                        IsLeft ? Vector2.Zero : new Vector2(ScoreBarToDraw.Width, 0),
        //                        _scale,
        //                        IsLeft ? SpriteEffects.None : SpriteEffects.FlipHorizontally,
        //                        0);

        //    DrawPlayerName(spriteBatch);
        //    DrawScore(spriteBatch);

        //}

        //private void DrawPlayerName(SpriteBatch spriteBatch)
        //{
        //    DrawStr(spriteBatch, Player.Name, PlayerNameInnerLocation, _playerNameScale);
        //}

        //private void DrawScore(SpriteBatch spriteBatch)
        //{
        //    CalculateLocationOfScore();
        //    DrawStr(spriteBatch, Player.Score.ToString(), ScoreInnerLocation, _scoreScale);
        //}

        //private void DrawStr(SpriteBatch spriteBatch,string str, Vector2 innerLocation,float scale)
        //{
        //    Vector2 location = innerLocation*_scale + Location;
        //    if (!IsLeft)
        //    {
        //        location.X = Location.X - innerLocation.X*_scale;
        //    }

        //    spriteBatch.DrawString(Font,
        //                            str,
        //                            location,
        //                            Color.White,
        //                            0,
        //                            IsLeft ? Vector2.Zero : new Vector2(Font.MeasureString(str).X, 0),
        //                            scale*_scale,
        //                            SpriteEffects.None,
        //                            1);
        //}
    }
}
