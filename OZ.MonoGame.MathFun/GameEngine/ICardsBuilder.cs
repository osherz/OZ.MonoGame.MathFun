using OZ.MonoGame.MathFun.GameEngine.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace OZ.MonoGame.MathFun.GameEngine
{
    public interface ICardsBuilder<TCard>
        where TCard: class, ICard
    {
        ICards<TCard> CreateCardsCollection(int rows, int columns);

    }


}
