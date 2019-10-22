using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using OZ.MonoGame.GameObjects.UI;
using OZ.MonoGame.MathFun.GameEngine;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using System.Linq;
using Microsoft.Xna.Framework.Content;
using OZ.MonoGame.MathFun.GameObjects.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public sealed class CardsCollection : Panel, ICards<CardDraw>
    {

        #region ICard Implementation
        public CardDraw this[Point position]
        {
            get => this[position.X, position.Y];
            private set => this[position.X, position.Y] = value;
        }

        public CardDraw this[int row, int column]
        {
            get => Controls[GetIndex(row, column)] as CardDraw;
            private set => Controls[GetIndex(row, column)] = value;
        }

        public int Rows { get; private set; }

        public int Columns { get; private set; }

        public int NumOfCards => Rows * Columns;

        public int RemainCards { get; private set; }

        public Engine<CardDraw> Engine { get; private set; }
        public SpriteFont CardText { get; set; }

        public void InsertCard(CardDraw card, int row, int column)
        {
            Controls.Insert(GetIndex(row, column), card);
            card.Position = new Point(row, column);
            RegisterEventsToCard(card);
            RemainCards++;
        }

        public void InsertCard(IReadOnlyList<CardDraw> cards, IReadOnlyList<Point> positions)
        {
            for (int i = 0; i < cards.Count; i++)
            {
                Point position = positions[i];
                InsertCard(cards[i], position.X, position.Y);
            }
        }

        public void Replace(Point card1Pos, Point card2Pos)
        {
            CardDraw temp = this[card1Pos];
            this[card1Pos] = this[card2Pos];
            this[card2Pos] = temp;

            this[card1Pos].Position = card1Pos;
            this[card2Pos].Position = card2Pos;
        }

        public void RemoveCard(int row, int column)
        {
            int index = GetIndex(row, column);
            UnregisterEventsFromCard(Controls[index] as CardDraw);
            Controls[index] = null;
            RemainCards--;
        }

        public void RemoveCard(params Point[] positions)
        {
            foreach (var position in positions)
            {
                RemoveCard(position.X, position.Y);
            }
        }

        IEnumerator<ICard> IEnumerable<ICard>.GetEnumerator()
        {
            return Controls.Cast<CardDraw>().GetEnumerator();
        }
        #endregion ICard Implementation

        #region Events
        public event EventHandler<Point> CardClicked;
        #endregion Events

        public CardsCollection(GamePrototype gameParent) : base(gameParent)
        {
            IsEnabled = false;
            IsVisible = false;
            BlackTextureWhenNotEnabled = false;
            RectangleOfContentAfterResizeing = false;

        }



        public void Reset(int rows, int columns, Engine<CardDraw> engine)
        {
            if (!(Engine is null))
            {
                Engine.GameStarted -= Engine_GameStarted;
            }

            Rows = rows;
            Columns = columns;
            RemainCards = 0;
            Engine = engine;

            Engine.GameStarted += Engine_GameStarted;

            IsEnabled = false;
            IsVisible = false;
            BlackTextureWhenNotEnabled = false;
            RectangleOfContentAfterResizeing = false;

        }

        private void Engine_GameStarted(object sender, EventArgs e)
        {
            CalculateCardSize();
            UpdateCardsLocationAndSize();
            IsEnabled = true;
            IsVisible = true;
        }

        private Vector2 _totalSizeForCards;
        private Vector2 _sizeOfCard;
        private float _minMargin = 10;
        private float _leftRightMargin;
        private float _topDownMargin;


        private void CalculateCardSize()
        {
            _totalSizeForCards = new Vector2()
            {
                X = RectangleOfContentDrawingMultiplyScale.Width - _minMargin * (Columns),
                Y = RectangleOfContentDrawingMultiplyScale.Height - _minMargin * (Rows),
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
            float remainRowSpace = RectangleOfContentDrawingMultiplyScale.Width - rowSpace;
            _leftRightMargin = remainRowSpace / (Columns);

            float columnSpace = _sizeOfCard.Y * Rows;
            float remainColumnSpace = RectangleOfContentDrawingMultiplyScale.Height - columnSpace;
            _topDownMargin = remainColumnSpace / (Rows);
        }
        private void UpdateCardsLocationAndSize()
        {
            Vector2 location = new Vector2();
            for (int row = 0; row < Rows; row++)
            {
                location.X = RectangleOfContentDrawingMultiplyScale.X + _leftRightMargin / 2;
                location.Y = RectangleOfContentDrawingMultiplyScale.Y + _topDownMargin / 2 + (_sizeOfCard.Y + _topDownMargin) * row;
                for (int column = 0; column < Columns; column++)
                {
                    CardDraw card = this[row, column];
                    card.Location = location;
                    card.Size = _sizeOfCard;

                    location.X += _leftRightMargin + _sizeOfCard.X;
                }
            }
        }

        private int GetIndex(int row, int columns)
        {
            return Columns * row + columns;
        }

        public override void LoadContent(ContentManager content)
        {

            UIApearance.CardReg = content.Load<Texture2D>("sprites/card");
            UIApearance.HoverCard = content.Load<Texture2D>("sprites/cardHover");
            UIApearance.CardPressed = content.Load<Texture2D>("sprites/cardPressed");
            UIApearance.CardTextFont = content.Load<SpriteFont>(@"fonts/cardTextFont");
            UIApearance.CardHoverEffect = content.Load<SoundEffect>("sound/bubbles");
            UIApearance.CardPressedEffect = content.Load<SoundEffect>("sound/pressed");

            RegTexture = content.Load<Texture2D>("background/boardBkgWithShadow");

            base.LoadContent(content);

        }


        private void RegisterEventsToCard(params CardDraw[] cards)
        {
            foreach (var card in cards)
            {
                card.Clicked += Card_Clicked;
            }
        }

        private void UnregisterEventsFromCard(params CardDraw[] cards)
        {
            foreach (var card in cards)
            {
                card.Clicked -= Card_Clicked;
            }
        }


        private void Card_Clicked(object sender, EventArgs e)
        {
            OnCardClicked((sender as CardDraw).Position);
        }


        #region Occur Events Method
        private void OnCardClicked(Point position)
        {
            CardClicked?.Invoke(this, position);
        }
        #endregion Occur Events Method

    }
}
