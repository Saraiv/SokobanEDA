using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        char[,] map;
        List<Vector2> objectivePointsPos;
        const int tileSize = 64;
        int width, height;
        Texture2D wallTexture, groundTexture, boxTexture, objectiveTexture;


        KeyboardManager km;
        Player player;
        LevelManager lm;



        //PLAYER
        Vector2 playerPos;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            km = new KeyboardManager();
            lm = new LevelManager();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);


            player = new Player(km, _spriteBatch, Content);
            player.OnObjectiveReach += IsLevelFinished;


            wallTexture = Content.Load<Texture2D>("Wall_Black");
            groundTexture = Content.Load<Texture2D>("GroundGravel_Grass");
            boxTexture = Content.Load<Texture2D>("Crate_Beige");
            objectiveTexture = Content.Load<Texture2D>("EndPoint_Black");

            lm.LoadLevel(ref height, ref width, _graphics, ref map, tileSize, ref objectivePointsPos, ref playerPos);
            player.SetPlayerPos(playerPos);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            km.Update();


            player.Update(gameTime, map, objectivePointsPos);



            if (km.IsKeyPressed(Keys.R))
            {
                lm.LoadLevel(ref height, ref width, _graphics, ref map, tileSize, ref objectivePointsPos, ref playerPos);

                player.SetPlayerPos(playerPos);

            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                {
                    char currentSymbol = map[x, y];
                    Console.Write(currentSymbol);


                    switch (currentSymbol)
                    {
                        case 'X':
                            _spriteBatch.Draw(wallTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;

                        case ' ':
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case 'B':
                            _spriteBatch.Draw(boxTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case '.':
                            _spriteBatch.Draw(objectiveTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;

                        default:
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                    }
                }

            player.Draw(gameTime, tileSize);
            // TODO: Add your drawing code here
            _spriteBatch.End();
            base.Draw(gameTime);
        }




        bool IsLevelFinished()
        {

            foreach (Vector2 pos in objectivePointsPos)
            {
                if (map[(int)pos.X, (int)pos.Y] != 'B') return false;
            }

            lm.NextLevel(ref height, ref width, _graphics, ref map, tileSize, ref objectivePointsPos, ref playerPos);
            player.SetPlayerPos(playerPos);

            return true;
        }
    }
}