using OZ.MonoGame.MathFun.GameEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace OZ.MonoGame.MathFun.GameObjects
{
    public class CardsBuilder : ICardsBuilder<CardDraw>
    {
        public Engine<CardDraw> Engine { get; set; }
        public CardsCollection CardsCollection { get; set; }

        public CardsBuilder()
        {
        }

        public ICards<CardDraw> CreateCardsCollection(int rows, int columns)
        {
            if(Engine is null || CardsCollection is null)
            {
                throw new ArgumentNullException("You must assign Engine and CardsCollection first");
            }
            CardsCollection.Reset(rows, columns, Engine);
            return CardsCollection;
        }
    }
}
