using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OZ.MonoGame.MathFun.GameEngine.Cards;

namespace OZ.MonoGame.MathFun.GameEngine
{
    public class Engine<TCard>
        where TCard: class, ICard
    {

        #region PLAYERS
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public Player TurnOf { get; private set; }
        #endregion PLAYERS


        private bool _isGameStarted = false;
        public bool IsGameStarted
        {
            get => _isGameStarted;

            set
            {
                bool old = _isGameStarted;
                if(old!=value)
                {
                    _isGameStarted = value;
                }

                if(_isGameStarted)
                {
                    OnGameStarted();
                }
            }
        }

        public bool IsGameFinished => IsGameStarted && Board.RemainCards == 0;
        public Player Winner
        {
            get
            {
                if(Player1.Score > Player2.Score)
                {
                    return Player1;
                }
                if(Player2.Score > Player1.Score)
                {
                    return Player2;
                }
                return null;
            }
        }

        public ICardBuilder<TCard> CardBuilder { get; private set; }
        public ICardsBuilder<TCard> BoardBuilder { get; private set; }

        private Point?[] _visibleCards = new Point?[2];
        public IReadOnlyList<Point> VisibleCards
        {
            get
            {
                return _visibleCards.Select(i => i.Value).ToArray();
            }
        }

        public bool ThereIs2VisibleCards
        {
            get
            {
                foreach (Point? position in _visibleCards)
                {
                    if (!position.HasValue) return false;
                }

                return true;
            }
        }


        #region EVENTS
        public event EventHandler GameStarted;
        #endregion EVENTS



        public ICards<TCard> Board { get; private set; }

        public Engine(ICardBuilder<TCard> cardBuilder, ICardsBuilder<TCard> boardBuilder)
        {
            CardBuilder = cardBuilder;
            BoardBuilder = boardBuilder;
        }


        public void StartNewGame()
        {
            ResetPlayersScore();

            if (new Random(DateTime.Now.Millisecond).Next(1, 2) == 1)
            {
                TurnOf = Player1;
            }
            else TurnOf = Player2;

            IsGameStarted = true;
        }

        public void InitBoard(int rows, int columns)
        {
            if ((rows * columns) % 2 == 1)
            {
                if (rows > columns)
                {
                    rows--;
                }
                else columns--;
            }

            if(!(Board is null)) Board.CardClicked -= Board_CardClicked;
            Board = BoardBuilder.CreateCardsCollection(rows, columns);
            Board.CardClicked += Board_CardClicked;
            Board.FillBoardAndMix(CardBuilder);
        }

        private void Board_CardClicked(object sender, Point position)
        {
            ChooseCard(position);
        }

        bool isSecondCard = false;
        public void ChooseCard(int row,int column)
        {
            if(IsGameStarted && Board.IsNotEmptyOrVisible(row,column) && !ThereIs2VisibleCards)
            {
                Point position = new Point(row, column);
                if (isSecondCard)
                {
                    _visibleCards[1] = position;
                }
                else
                {
                    _visibleCards[0] = position;
                }
                Board[position].VisibleValue();

                isSecondCard = !isSecondCard;
            }
        }

        public void ChooseCard(Point position) => ChooseCard(position.X, position.Y);

        private void HideVisibleCards()
        {
            for (int i = 0; i < 2; i++)
            {
                Point? position = _visibleCards[i];
                if (position.HasValue)
                {
                    TCard card = Board[position.Value];
                    if (card != null)
                    {
                        card.IsValueVisible = false;
                    }
                    _visibleCards[i] = null;
                }
            }
        }

        public bool CheckMatch(bool checkOnly = false)
        {
            bool result = false;
            if(ThereIs2VisibleCards)
            {
                TCard card1 = Board[_visibleCards[0].Value];
                TCard card2 = Board[_visibleCards[1].Value];

                if(card1.CheckMatch(card2))
                {
                    if (checkOnly) return true;
                    result = true;
                    TurnOf.Score++;
                    Board.RemoveCard(_visibleCards.Select(p => p.Value).ToArray());
                }
                else
                {
                    if (checkOnly) return false;
                    card1.IsValueVisible = card2.IsValueVisible = false;
                    TurnOf = TurnOf != Player1 ? Player1 : Player2;
                }

                _visibleCards[0] = _visibleCards[1] = null;
                HideVisibleCards();
            }

            return result;
        }


        private void ResetPlayersScore()
        {
            Player1.Score = Player2.Score = 0;
        }
        
        protected virtual void OnGameStarted()
        {

            GameStarted?.Invoke(this, EventArgs.Empty);
        }
    }
}
