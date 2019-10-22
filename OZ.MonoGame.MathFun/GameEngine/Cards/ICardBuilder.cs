using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameEngine.Cards
{
    public interface ICardBuilder<TCard>
        where TCard : class, ICard
    {
        TCard[] GeneratePairCard();
    }
}
