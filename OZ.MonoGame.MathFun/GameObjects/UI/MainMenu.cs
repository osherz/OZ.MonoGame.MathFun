using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using OZ.MonoGame.GameObjects.UI;

namespace OZ.MonoGame.MathFun.GameObjects.UI
{
    public class MainMenu : GameMenu
    {
        #region EVENTS
        public event EventHandler StartGameBtnClicked
        {
            add
            {
                _startGameBtn.Clicked += value;
            }

            remove
            {
                _startGameBtn.Clicked -= value;
            }
        }
        public event EventHandler ExitBtnClicked
        {
            add
            {
                _exitBtn.Clicked += value;
            }

            remove
            {
                _exitBtn.Clicked -= value;
            }
        }
        #endregion EVENTS

        #region BUTTONS
        private MenuButton _startGameBtn;
        private MenuButton _exitBtn;

        #endregion BUTTONS

        public MainMenu(GamePrototype gameParent) : base(gameParent)
        {
            Text = "Main Menu";
        }


        public override void Initialize()
        {
            InitButtons();
            base.Initialize();
        }
        private void InitButtons()
        {
            _startGameBtn = new MenuButton(GameParent)
            {
                Text = "Memory Game",
                IsEnabled = true,
                ControlApearance = ButtonsApearance
            };

            _exitBtn = new MenuButton(GameParent)
            {
                Text = "Exit",
                IsEnabled = true,
                ControlApearance = ButtonsApearance
            };

            AddRange(_startGameBtn, _exitBtn);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            InnerToMiddleX();

        }





    }
}
