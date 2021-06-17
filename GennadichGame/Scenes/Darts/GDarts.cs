using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using GennadichGame.Enums;
using GennadichGame.Input;
using GennadichGame.Sockets.Data;
using GennadichGame.Sockets.TCP;
using GennadichGame.Controls;

namespace GennadichGame.Scenes.Darts
{
    public sealed class GDarts : Scene
    {
        #region Data

        private List<MultiLabel> PlayerLabels { get; } = new List<MultiLabel>();

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

            PlayerLabels.Add(new MultiLabel(new Point(50, 50), Align.Left, Fonts.RegularConsolas12, Color.Black));
            PlayerLabels.Add(new MultiLabel(new Point(200, 50), Align.Left, Fonts.RegularConsolas12, Color.Black));
            PlayerLabels.Add(new MultiLabel(new Point(350, 50), Align.Left, Fonts.RegularConsolas12, Color.Black));
        }

        public override void Update(GameTime gameTime)
        {
            if (GMouse.IsButtonPressed(MouseButton.Left))
            {
                if (Client.ReceiverGameData.WinPlayer != null)
                {
                    Thread.Sleep(400);
                    TcpAsyncServer.SafeCloseAllSockets();
                    Game.SceneManager.ActiveState = GameState.MainMenu;
                    foreach (var label in PlayerLabels)
                    {
                        label.Clear();       
                    }
                }
                SendShootData();
            }

            foreach (var label in PlayerLabels) label.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            Game.SpriteBatch.Begin();

            Game.SpriteBatch.Draw(Darts.Texture, new Rectangle(Darts.Position.ToPoint(), Darts.Size.ToPoint()),
                Color.White);
            if (Client.ReceiverGameData != null)
            {
                var message = Client.ReceiverGameData.WinPlayer != null
                    ? $"{Client.ReceiverGameData.WinPlayer} win!"
                    : $"{Client.ReceiverGameData.TurnPlayerName} turn";
                Game.SpriteBatch.DrawString(Game.FontManager[Fonts.RegularConsolas16],
                    message, new Vector2(450, 20),
                    Color.Black);
            }

            if (Client.ReceiverGameData != null)
            {
                foreach (var point in Client.ReceiverGameData.Shoots.Select(si => si.MousePoint))
                {
                    var tex = Game.TextureManager[Textures.Dart];
                    var texWidth = tex.Width * .05;
                    var texHeight = tex.Height * .05;
                    Game.SpriteBatch.Draw(tex,
                        new Rectangle((int) (point.X - texWidth / 2), (int) (point.Y - texHeight / 2), (int) texWidth,
                            (int) texHeight), Color.White);
                }
            }

            Game.SpriteBatch.End();

            foreach (var label in PlayerLabels) label.Draw(gameTime);
        }

        #endregion

        #region PrivateMethods

        #endregion

        #region PublicMethods

        public void StartGame(bool isHosted)
        {
            Client.handleViewData += ViewGameInformation;
            if (isHosted) TcpAsyncServer.SendStartGameDataMessage();
        }

        public void ViewGameInformation(GameData gameData)
        {
            for (var i = 0; i < gameData.CurrentPlayers.Count; i++)
            {
                if (PlayerLabels[i].Count == 0)
                {
                    PlayerLabels[i].AddLabel(gameData.CurrentPlayers[i]);
                    PlayerLabels[i].AddLabel(gameData.TableScore.GetPlayerScore(gameData.CurrentPlayers[i]).Last()
                        .ToString());
                }
                else
                {
                    if (PlayerLabels[i].Count - 1 !=
                        gameData.TableScore.GetPlayerScore(gameData.CurrentPlayers[i]).Count)
                    {
                        PlayerLabels[i].AddLabel(gameData.TableScore.GetPlayerScore(gameData.CurrentPlayers[i]).Last()
                            .ToString());
                    }
                }
            }
        }

        public void SendShootData()
        {
            if (ClientName != Client.ReceiverGameData.TurnPlayerName || Client.ReceiverGameData.Shoots.Count == 3)
                return;
            var shootInfo = new ShootInfo
            {
                MousePoint = {X = GMouse.Position.X, Y = GMouse.Position.Y},
                ShootScore = Darts.GetIntersectedSegmentScore()
            };
            Client.ReceiverGameData.Shoots.Add(shootInfo);
            Client.SendGameMessage();
        }

        #endregion
    }
}