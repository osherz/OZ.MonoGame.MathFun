//#define OLD

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using OZ.MonoGame.MathFun.GameEngine;
using OZ.MonoGame.GameObjects;
using OZ.MonoGame.MathFun.GameObjects.UI;

namespace OZ.MonoGame.MathFun.GameObjects
{
#if OLD
    public class CardsDraw : IGameObject
    {

        private Board<CardDraw> _cards;
        private Vector2 _boardSize;
        private Vector2 _boardLocation;
        private Point _windowSize;
        private Vector2 _totalSizeForCards;
        private Vector2 _sizeOfCard;

        public int Rows => _cards.Rows;
        public int Columns => _cards.Columns;

        private float _leftRightMargin;
        private float _topDownMargin;
        readonly private float _minMargin;

        public SpriteFont CardText { get; set; }
        private Texture2D _boardBkg;
        private Vector2 _bkgScale;
        private Rectangle _cardsRect;

        public Game Parent { get; set; }


        public CardsDraw(Engine<CardDraw> engine, int windowWidth, int windowHeight, Vector2 location, float margin = 10)
        {
            _windowSize = new Point(windowWidth, windowHeight);
            _minMargin = margin;
            _boardLocation = location;

            engine.GameStarted += Engine_GameStarted;
        }

        private void Engine_GameStarted(object sender, EventArgs e)
        {
            _cards = (sender as Engine<CardDraw>).Board;

            foreach (var card in _cards)
            {
                card.LoadContent(Parent.Content);
            }

            CalculateBoardSize();
            CalculateCardSize();
            UpdateCardsLocationAndSize();
        }

        private void CalculateBoardSize()
        {
            int width = _windowSize.X;
            int height = _windowSize.Y;

            _boardSize = new Vector2()
            {
                X = width,
                Y = height
            };

            _bkgScale = new Vector2()
            {
                X = _boardSize.X / _boardBkg.Width,
                Y = _boardSize.Y / _boardBkg.Height
            };

            _cardsRect = new Rectangle()
            {
                Location = (new Vector2(100 * _bkgScale.X, 100 * _bkgScale.Y)).ToPoint(),
                Size = (new Vector2((943 - 100) * _bkgScale.X, (600 - 100) * _bkgScale.Y)).ToPoint()
            };
        }

        private void CalculateCardSize()
        {

            _totalSizeForCards = new Vector2()
            {
                X = _cardsRect.Width - _minMargin * (Columns),
                Y = _cardsRect.Height - _minMargin * (Rows),
            };

            _sizeOfCard = new Vector2()
            {
                X = _totalSizeForCards.X / Columns,
                Y = _totalSizeForCards.Y / Rows
            };

            if (_sizeOfCard.X < _sizeOfCard.Y)
            {
                _sizeOfCard.Y = _sizeOfCard.X;
            }
            else
            {
                _sizeOfCard.X = _sizeOfCard.Y;
            }

            float rowSpace = _sizeOfCard.X * Columns;
            float remainRowSpace = _cardsRect.Width - rowSpace;
            _leftRightMargin = remainRowSpace / (Columns);

            float columnSpace = _sizeOfCard.Y * Rows;
            float remainColumnSpace = _cardsRect.Height - columnSpace;
            _topDownMargin = remainColumnSpace / (Rows);
        }

        private void UpdateCardsLocationAndSize()
        {
            Vector2 location = new Vector2();
            for (int row = 0; row < Rows; row++)
            {
                location.X = _cardsRect.X + _leftRightMargin / 2;
                location.Y = _cardsRect.Y + _topDownMargin / 2 + (_sizeOfCard.Y + _topDownMargin) * row;
                for (int column = 0; column < Columns; column++)
                {
                    CardDraw card = _cards[row, column];
                    card.Location = location + _boardLocation;
                    card.Size = _sizeOfCard;

                    location.X += _leftRightMargin + _sizeOfCard.X;
                }
            }
        }


        public virtual void Initialize()
        {

        }

        Texture2D _rect;
        public virtual void LoadContent(ContentManager content)
        {
            UIApearance.CardReg = content.Load<Texture2D>("sprites/card");
            UIApearance.HoverCard = content.Load<Texture2D>("sprites/cardHover");
            UIApearance.CardPressed = content.Load<Texture2D>("sprites/cardPressed");
            UIApearance.CardTextFont = CardText;
            UIApearance.CardHoverEffect = content.Load<SoundEffect>("sound/bubbles");
            UIApearance.CardPressedEffect = content.Load<SoundEffect>("sound/pressed");

            _boardBkg = content.Load<Texture2D>("background/boardBkgWithShadow");

            _rect = new Texture2D(Parent.GraphicsDevice, 1, 1);
            _rect.SetData(new[] { Color.Red });

        }

        public virtual void UnloadContent(ContentManager content)
        {
        }

        public virtual void Update(GameTime gameTime)
        {

        }


        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {



            spriteBatch.Draw(_boardBkg,
                            _boardLocation,
                            null,
                            Color.White,
                            0,
                            Vector2.Zero,
                            _bkgScale,
                            SpriteEffects.None,
                            1);

            //For checking purposes
            spriteBatch.Draw(_rect,
                            _cardsRect.Location.ToVector2() + _boardLocation,
                            null,
                            Color.Red * 0, // Hidden
                            0,
                            Vector2.Zero,
                            _cardsRect.Size.ToVector2(),
                            SpriteEffects.None, 1);

            foreach (var card in _cards.Where(i => i != null))
            {
                card.Draw(gameTime, spriteBatch);
            }
        }

        public Point? CardCursorPoints(Vector2 mouseLocation)
        {

            Rectangle boardRectangle = new Rectangle(_cardsRect.Location + _boardLocation.ToPoint(), _cardsRect.Size);


            if (!boardRectangle.Contains(mouseLocation))
            {
                return null;
            }

            Vector2 point = mouseLocation - boardRectangle.Location.ToVector2();

            int column = (int)(point.X / (_sizeOfCard.X + _leftRightMargin));
            int row = (int)(point.Y / (_sizeOfCard.Y + _topDownMargin));

            Point location = new Point(row == Rows ? row - 1 : row, column == Columns ? column - 1 : column);
            var chosenCard = _cards[location];
            if (new Rectangle(0, 0, Rows, Columns).Contains(location) && chosenCard !=null && chosenCard.IsContains(mouseLocation.ToPoint()))
            {
                return location;
            }
            else return null;
        }

    }
#endif
}
