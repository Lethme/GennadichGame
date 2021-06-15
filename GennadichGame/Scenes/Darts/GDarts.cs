using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GennadichGame.Enums;
using GennadichGame.Input;
using GennadichGame.Sockets.Data;
using GennadichGame.Sockets.TCP;

namespace GennadichGame.Scenes.Darts
{
    public sealed class GDarts : Scene
    {
        #region Data

        #endregion

        #region Properties

        private Darts Darts { get; set; }
        public string ClientName { get; set; }
        public TcpAsyncClient Client { get; set; }

        #endregion

        #region Constructors

        public GDarts(Texture2D dartsTex)
        {
            Darts = new Darts(dartsTex);
            Initialize();
        }

        #endregion

        #region BaseClassMethods

        public override void Initialize()
        {
            Darts.Scale = .5f;
            Darts.Position = new Vector2(
                Game.Window.ClientBounds.Width / 2 - Darts.Texture.Width * Darts.Scale / 2 +
                Game.Window.ClientBounds.Width * 0.2f,
                Game.Window.ClientBounds.Height / 2 - Darts.Texture.Height * Darts.Scale / 2
            );
        }

        public override void Update(GameTime gameTime)
        {
            if (GMouse.IsButtonPressed(MouseButton.Left))
            {
                SendShootData();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(Darts.Texture, new Rectangle(Darts.Position.ToPoint(), Darts.Size.ToPoint()),
                Color.White);

            var mousePosition = Darts.GetMousePositionParams();
            Game.SpriteBatch.DrawString(Game.FontManager[Fonts.RegularConsolas16],
                $"Distance: {(mousePosition.Distance > 1 ? 1 : mousePosition.Distance)}", new Vector2(50, 50),
                Color.Black);
            Game.SpriteBatch.DrawString(Game.FontManager[Fonts.RegularConsolas16],
                $"Angle: {GDartsSegment.NormalizeAngle(mousePosition.Angle)}",
                new Vector2(50, 50 + Game.FontManager[Fonts.RegularConsolas16].MeasureString("TEST").Y), Color.Black);
            Game.SpriteBatch.DrawString(Game.FontManager[Fonts.RegularConsolas16],
                $"Intersected segment: {Darts.IntersectedSegment}",
                new Vector2(50, 50 + Game.FontManager[Fonts.RegularConsolas16].MeasureString("TEST").Y * 2),
                Color.Black);

            Game.SpriteBatch.End();
        }

        #endregion

        #region PrivateMethods

        #endregion

        #region PublicMethods

        public void StartGame()
        {
            Client.handleViewData += ViewGameInformation;
            TcpAsyncServer.SendStartGameDataMessage();
        }

        public void ViewGameInformation(GameData gameData)
        {
            if (gameData.WinPlayer == null)
            {
                Console.Clear();
                Console.WriteLine("Players:");
                foreach (var playerName in gameData.CurrentPlayers)
                {
                    var listScore = gameData.TableScore.GetPlayerScore(playerName).ToList();
                    Console.WriteLine(
                        $"{gameData.CurrentPlayers.IndexOf(playerName) + 1}. {string.Join(" ", listScore)}");
                }

                Console.WriteLine(gameData.TurnPlayerName == ClientName
                    ? "Your turn:"
                    : $"{gameData.TurnPlayerName} turn:");
                foreach (var shoot in gameData.Shoots)
                {
                    Console.WriteLine(
                        $"{gameData.Shoots.IndexOf(shoot) + 1}. {shoot.MousePoint} : {shoot.ShootScore}");
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine($"{gameData.WinPlayer} win!");
            }
        }

        public void SendShootData()
        {
            if (ClientName == Client.ReceiverGameData.TurnPlayerName)
            {
                var shootInfo = new ShootInfo();
                shootInfo.MousePoint.X = GMouse.Position.X;
                shootInfo.MousePoint.Y = GMouse.Position.Y;
                // there calculate score TODO YURA
                shootInfo.ShootScore = 1;
                Client.ReceiverGameData.Shoots.Add(shootInfo);
                Client.SendGameMessage();
            }
        }

        #endregion
    }
}