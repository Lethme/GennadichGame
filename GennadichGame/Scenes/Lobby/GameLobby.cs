using System;
using System.Linq;
using System.Collections.Generic;
using System.Net;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GennadichGame.Enums;
using GennadichGame.Input;
using GennadichGame.Manager;
using GennadichGame.Scenes.Darts;
using GennadichGame.Sockets.Common;
using GennadichGame.Sockets.TCP;
using GennadichGame.Sockets.UDP;
using Microsoft.Xna.Framework.Input;

using GennadichGame.Controls;

namespace GennadichGame.Scenes.Lobby
{
    public sealed class GameLobby : Scene
    {
        private Lobby Lobby { get; set; } = new Lobby();
        private bool IsHosted { get; set; }
        private Menu LobbyMenu { get; }
        private TcpAsyncClient _client { get; set; }
        private string ClientName { get; set; }
        private String PlayersCount => $"{Lobby.PlayersCount}/{Lobby.MaxSize}";
        private SpriteFont LobbyNameFont => Game.FontManager[Fonts.RegularConsolas16];
        private SpriteFont PlayersCountFont => Game.FontManager[Fonts.RegularConsolas18];
        private SpriteFont PlayersNamesFont => Game.FontManager[Fonts.RegularConsolas16];

        public GameLobby(bool isHosted, string clientName)
        {
            IsHosted = isHosted;
            ClientName = clientName;

            if (IsHosted) LobbyMenu = new Menu
            (
                Position.Bottom, Align.Center, Fonts.RegularConsolas16,
                ("Start Game", ActionType.Update, () =>
                {
                    ((GDarts)Game.SceneManager[GameState.Game]).Client = _client;
                    ((GDarts)Game.SceneManager[GameState.Game]).ClientName = ClientName;
                    Game.SceneManager.ActiveState = GameState.Game;
                }
                ),
                ("Exit Lobby", ActionType.Update, ExitLobby)
            );
            else
            {
                LobbyMenu = new Menu(Position.Bottom, Align.Center, Fonts.RegularConsolas16, ("Exit Lobby", ActionType.Update, ExitLobby));
            }
        }
        private void ExitLobby()
        {
            if (IsHosted) TcpAsyncServer.StopInterfacesListening();
            _client = null;
            Game.SceneManager.ActiveState = GameState.MainMenu;
        }

        public override void Initialize()
        {
            if (IsHosted)
            {
                var udpServer = new UdpServer(12345, 11000, "GDarts");
                udpServer.StartListen();
                TcpAsyncServer.StartTcpServer();
            }

            var udpClient = new UdpClient("GDarts", 11000, 3000);
            if (udpClient.SendToBroadcast())
            {
                var connection = new ConnectionManager(IPAddress.Parse(udpClient.ServerAddress.Address.ToString()),
                    udpClient.TcpPort);
                _client = new TcpAsyncClient(connection);
                _client.handleConnection += AddPlayerInLobby;
                _client.SendFindLobbyMessage();
            }
            else
            {
                return;
            }

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            //if (GKeyboard.IsKeyPressed(Keys.Enter))
            //{
            //    ((GDarts) Game.SceneManager[GameState.Game]).Client = _client;
            //    ((GDarts) Game.SceneManager[GameState.Game]).ClientName = ClientName;
            //    Game.SceneManager.ActiveState = GameState.Game;
            //}

            LobbyMenu.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            var offset = new Vector2(30, 30);

            Game.SpriteBatch.Begin();
            Game.SpriteBatch.DrawString(LobbyNameFont, Lobby.Name, offset, Color.Black);
            Game.SpriteBatch.DrawString(PlayersCountFont, PlayersCount,
                new Vector2(Game.Window.ClientBounds.Width - PlayersCountFont.MeasureString(PlayersCount).X - offset.X,
                    offset.Y), Color.Black);

            var posY = offset.Y + Game.Window.ClientBounds.Height * 0.15f;
            foreach (var playerName in Lobby.Players.Select(player => player.Name))
            {
                var playerStrSize = PlayersNamesFont.MeasureString(playerName);
                Game.SpriteBatch.DrawString(PlayersNamesFont, playerName,
                    new Vector2(Game.Window.ClientBounds.Width / 2 - playerStrSize.X / 2, posY), Color.Black);
                posY += playerStrSize.Y * 1.3f;
            }

            Game.SpriteBatch.End();

            LobbyMenu.Draw(gameTime);
        }

        public void AddPlayerInLobby(List<string> players)
        {
            lock (Lobby.Players)
            {
                Lobby.Players.RemoveRange(0, Lobby.Players.Count);
                foreach (var player in players)
                {
                    Lobby.Players.Add(new Player(player));
                }
            }
        }
    }
}