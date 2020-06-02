using System;
using Raylib;
using LiveTween;
using static Raylib.Raylib;

namespace Project2D
{
    class Game
    {
        Image logo;
        Texture2D texture;
        string logText = string.Empty;

        Tween tween;

        public void Init()
        {
            logo = LoadImage("../Images/aie-logo-dark.jpg");
            texture = LoadTextureFromImage(logo);

            ConnectToEditor();

            tween = new Tween(EasingType.Linear, 1);
        }

        public void Shutdown()
        {
        }

        public void Update()
        {
            if (IsKeyPressed(KeyboardKey.KEY_ENTER))
            {
                if (Tween.Socket.Connected)
                    tween.Link();
                else
                    ConnectToEditor();
            }

            if (IsKeyPressed(KeyboardKey.KEY_SPACE))
                tween.Play();
        }

        public void Draw()
        {
            BeginDrawing();

            ClearBackground(Color.WHITE);

            DrawText("ENTER - Link tween to editor\nSPACE - Play tween\nESCAPE - Quit game", 32, 32, 16, Color.DARKGRAY);
            DrawText(logText, GetScreenWidth() - 200, 32, 16, Color.DARKGRAY);

            EndDrawing();
        }

        private void ConnectToEditor()
        {
            if (Tween.Connect())
                logText = "Connection successful.";
            else
                logText = "Connection failed.";
        }
    }
}
