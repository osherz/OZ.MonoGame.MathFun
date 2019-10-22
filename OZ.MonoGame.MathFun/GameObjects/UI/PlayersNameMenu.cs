using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class PlayersNameMenu : GameMenu
    {
        GameTextBox[] _playersTextBox;
        MenuButton _start;
        MenuButton _backBtn;

        public event EventHandler StartClicked { add => _start.Clicked += value;remove => _start.Clicked -= value; }
        public event EventHandler BackClicked { add => _backBtn.Clicked += value; remove => _backBtn.Clicked -= value; }

        public string this[int player] => _playersTextBox[player].Text;

        public int Players { get => _playersTextBox.Length; }

        public PlayersNameMenu(GamePrototype gameParent, int playersNumber) : base(gameParent)
        {
            Text = "Players";

            _playersTextBox = new GameTextBox[playersNumber];

        }

        public override void Initialize()
        {
            InitControls();
            base.Initialize();

        }

        private void InitControls()
        {
            for (int i = 0; i < Players; i++)
            {
                _playersTextBox[i] = new GameTextBox(GameParent)
                {
                    Text = "player" + (i + 1),
                    IsEnabled = true,
                    Tag = "player" + (i + 1),
                    GameParent = GameParent,
                    BkgTransparent = false
                };

                Add(_playersTextBox[i]);
            }

            _start = new MenuButton(GameParent)
            {
                Text = "Start",
                IsEnabled = true,
                GameParent = GameParent,
            };

            _backBtn = new MenuButton(GameParent)
            {
                Text = "Back",
                IsEnabled = true,
                GameParent = GameParent,
            };
            AddRange(_start,_backBtn);

        }

        public override void LoadContent(ContentManager content)
        {
            InnerToMiddleX();
            base.LoadContent(content);
        }


    }
}
