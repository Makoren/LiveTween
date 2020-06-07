using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Raylib;
using static Raylib.Raylib;
using LiveTween;

namespace Project2D
{
    class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }

        private Tween tween;

        public Player()
        {
            X = 100;
            Y = 300;

            tween = new Tween(EasingType.Linear, 1, true);
        }

        public Player(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                if (Tween.Socket.Connected)
                    tween.Link();
            }

            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
            {
                tween.Play(X, Y, GetMouseX(), GetMouseY());
            }

            tween.Update();
        }

        public void Draw()
        {
            DrawCircle(X, Y, 25, Color.RED);
        }
    }
}
