using Microsoft.Xna.Framework;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine
{
#if OLD
    public class Board<TCard>  : IEnumerable<TCard>
        where TCard : class, ICard
    {
        public class BoardEnumerator : IEnumerator<TCard>
        {
            IEnumerator _enumerator;

            public BoardEnumerator(TCard[,] cards)
            {
                _enumerator = cards.GetEnumerator();
            }

            public TCard Current => _enumerator.Current as TCard;

            object IEnumerator.Current => _enumerator.Current;

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                return _enumerator.MoveNext();
            }

            public void Reset()
            {
                _enumerator.Reset();
            }
        }


        private readonly TCard[,] board;
        private readonly ICardBuilder<TCard> cardBuilder;
        
        public TCard this[int row, int column] => board[row,column];
        public TCard this[Point position] => this[position.X, position.Y];

        public int Rows { get; private set; }
        public int Columns { get; private set; }
        public int NumOfCards => Rows * Columns;
        public int RemainCards { get; private set; }

        public Board(int rows, int columns, ICardBuilder<TCard> cardBuilder)
        {
            Rows = rows;
            Columns = columns;
            board = new TCard[Rows, Columns];
            this.cardBuilder = cardBuilder;
        }

        public void InitBoard()
        {
            for (int cardNum = 0; cardNum < NumOfCards; cardNum++)
            {
                TCard[] cards = cardBuilder.GeneratePairCard();
                board[GetRow(cardNum), GetColumn(cardNum)] = cards[0];

                cardNum++;
                board[GetRow(cardNum), GetColumn(cardNum)] = cards[1];
            }
            RemainCards = NumOfCards;
            MixCards(NumOfCards);
        }

        public bool IsNotEmptyOrVisible(int row, int column)
        {
            return board[row, column] != null && !board[row, column].IsValueVisible;
        }

        public bool IsNotEmptyOrVisible(Point position)
        {
            return IsNotEmptyOrVisible(position.X, position.Y);
        }

        private int GetRow(int cardNumber) => cardNumber / Columns ;
        private int GetColumn(int cardNumber) => cardNumber % Columns;

        public void RemoveCard(int row,int column)
        {
            if (board[row, column] != null)
            {
                board[row, column] = null;
                RemainCards--;
            }
        }

        public void RemoveCard(params Point[] positions)
        {
            foreach (Point position in positions)
            {
                RemoveCard(position.X, position.Y);
            }         
        }

        private void MixCards(int times)
        {
            Random rand = new Random();
            do
            {
                int card1Index = rand.Next(0, NumOfCards);
                int card2Index = rand.Next(0, NumOfCards);

                ReplaceCardsPositions(card1Index, card2Index);

            } while (times-- > 0);
        }

        private void ReplaceCardsPositions(int card1Index, int card2Index)
        {
            int row1 = GetRow(card1Index), column1 = GetColumn(card1Index);
            int row2 = GetRow(card2Index), column2 = GetColumn(card2Index);

            TCard cardTemp = board[row1, column1];
            board[row1, column1] = board[row2, column2];
            board[row2, column2] = cardTemp;
        }

        public IEnumerator<TCard> GetEnumerator()
        {
            return new BoardEnumerator(board);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
#endif
}
