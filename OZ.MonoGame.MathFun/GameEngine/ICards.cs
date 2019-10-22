using Microsoft.Xna.Framework;
using OZ.MonoGame.MathFun.GameEngine.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace OZ.MonoGame.MathFun.GameEngine
{
    public interface ICards<TCard> : IEnumerable<ICard>
        where TCard: class, ICard
    {
        #region Properties
        TCard this[int row, int column] { get; }
        TCard this[Point position] { get; }
        int Rows { get; }
        int Columns { get; }
        int NumOfCards { get; }
        int RemainCards { get; }
        #endregion Properties

        #region Events
        event EventHandler<Point> CardClicked;
        #endregion Events

        #region Methods
        void InsertCard(TCard card, int row, int column);
        void InsertCard(IReadOnlyList<TCard> card, IReadOnlyList<Point> positions);

        void Replace(Point card1Pos, Point card2Pos);
        void RemoveCard(int row, int column);
        void RemoveCard(params Point[] positions);
        #endregion Methods
    }

    public static class IBoardBuilderExtension
    {
        public static int GetRow<TCard>(this ICards<TCard> board, int cardNumber)
            where TCard : class, ICard
        {
            return cardNumber / board.Columns;
        }

        public static int GetColumn<TCard>(this ICards<TCard> board, int cardNumber)
            where TCard : class, ICard
        {
            return cardNumber % board.Columns;
        }

        internal static void FillBoardAndMix<TCard>(this ICards<TCard> board, ICardBuilder<TCard> cardBuilder)
            where TCard : class, ICard
        {
            for (int cardNum = 0; cardNum < board.NumOfCards; cardNum++)
            {
                TCard[] cards = cardBuilder.GeneratePairCard();
                board.InsertCard(cards[0], board.GetRow(cardNum), board.GetColumn(cardNum));

                cardNum++;
                board.InsertCard(cards[1], board.GetRow(cardNum), board.GetColumn(cardNum));
            }
            
            board.MixCards(board.RemainCards);
        }

        internal static void MixCards<TCard>(this ICards<TCard> board, int times)
             where TCard : class, ICard
        {
            Random rand = new Random();
            do
            {
                int card1Index = rand.Next(0, board.NumOfCards);
                int card2Index = rand.Next(0, board.NumOfCards);

                board.ReplaceCardsPositions(card1Index, card2Index);

            } while (times-- > 0);
        }

        private static void ReplaceCardsPositions<TCard>(this ICards<TCard> board, int card1Index, int card2Index)
            where TCard : class, ICard
        {
            //Card1
            int row1 = board.GetRow(card1Index);
            int column1 = board.GetColumn(card1Index);

            //card2
            int row2 = board.GetRow(card2Index);
            int column2 = board.GetColumn(card2Index);

            board.Replace(new Point(row1, column1), new Point(row2, column2));
        }

        public static bool IsNotEmptyOrVisible<TCard>(this ICards<TCard> board, int row, int column)
            where TCard:class, ICard
        {
            return !(board[row, column] is null) && !board[row, column].IsValueVisible;
        }

        public static bool IsNotEmptyOrVisible<TCard>(this ICards<TCard> board, Point position)
            where TCard : class, ICard
        {
            return board.IsNotEmptyOrVisible(position.X, position.Y);
        }


    }

}
