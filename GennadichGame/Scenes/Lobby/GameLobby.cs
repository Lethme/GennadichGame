using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GennadichGame.Enums;

namespace GennadichGame.Scenes.Lobby
{
    public sealed class GameLobby : Scene
    {
        private Lobby Lobby { get; set; } = new Lobby(new Player("Test"), new Player("Dimasik karasik"), new Player("SHVABRA ZAEBALA"));
        private String PlayersCount => $"{Lobby.PlayersCount}/{Lobby.MaxSize}";
        private SpriteFont LobbyNameFont => Game.FontManager[Fonts.RegularConsolas16];
        private SpriteFont PlayersCountFont => Game.FontManager[Fonts.RegularConsolas18];
        private SpriteFont PlayersNamesFont => Game.FontManager[Fonts.RegularConsolas16];
        protected override void Initialize()
        {
            
        }
        public override void Update(GameTime gameTime)
        {
            
        }
        public override void Draw(GameTime gameTime)
        {
            var offset = new Vector2(30, 30);

            Game.SpriteBatch.Begin();
            Game.SpriteBatch.DrawString(LobbyNameFont, Lobby.Name, offset, Color.Black);
            Game.SpriteBatch.DrawString(PlayersCountFont, PlayersCount, new Vector2(Game.Window.ClientBounds.Width - PlayersCountFont.MeasureString(PlayersCount).X - offset.X, offset.Y), Color.Black);

            var posY = offset.Y + Game.Window.ClientBounds.Height * 0.15f;
            foreach (var playerName in Lobby.Players.Select(player => player.Name))
            {
                var playerStrSize = PlayersNamesFont.MeasureString(playerName);
                Game.SpriteBatch.DrawString(PlayersNamesFont, playerName, new Vector2(Game.Window.ClientBounds.Width / 2 - playerStrSize.X / 2, posY), Color.Black);
                posY += playerStrSize.Y * 1.3f;
            }

            Game.SpriteBatch.End();
        }
    }
}
