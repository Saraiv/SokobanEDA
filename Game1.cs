﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Sokoban
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        string levelPath = "../../../Content/SokobanLevels/level1.txt";
        char[,] map;
        const int tileSize = 64;
        int width, height;
        Vector2 playerPos;
        KeyboardManager km = new KeyboardManager();
        Texture2D playerTexture, wallTexture, groundTexture, endTexture, crateTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            LoadLevel();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            playerTexture = Content.Load<Texture2D>("Character4");
            wallTexture = Content.Load<Texture2D>("Wall_Black");
            groundTexture = Content.Load<Texture2D>("GroundGravel_Grass");
            crateTexture = Content.Load<Texture2D>("Crate_Beige");
            endTexture = Content.Load<Texture2D>("EndPoint_Black");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            km.Update();
            Movement();
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    char currentSymbol = map[x, y];

                    switch (currentSymbol)
                    {
                        case 'X':
                            _spriteBatch.Draw(wallTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case ' ':
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case 'B':
                            _spriteBatch.Draw(crateTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        case '.':
                            _spriteBatch.Draw(endTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                        default:
                            _spriteBatch.Draw(groundTexture, new Vector2(x, y) * tileSize, Color.White);
                            break;
                    }
                }
            }
            _spriteBatch.Draw(playerTexture, playerPos * tileSize, Color.White);
            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        void LoadLevel()
        {
            string[] lines = File.ReadAllLines(levelPath);
            map = new char[lines[0].Length, lines.Length];
            
            for(int x = 0; x < lines[0].Length; x++)
            {
                for(int y = 0; y < lines.Length; y++)
                {
                    string currentLine = lines[y];
                    map[x, y] = currentLine[x];
                    if (currentLine[x] == 'i')
                        playerPos = new Vector2(x, y);
                }
            }

            width = lines[0].Length;
            height = lines.Length;

            _graphics.PreferredBackBufferHeight = lines.Length * tileSize;
            _graphics.PreferredBackBufferWidth = lines[0].Length * tileSize;
            
            _graphics.ApplyChanges();
        }

        void Movement()
        {
            Vector2 newPos = playerPos;
            Vector2 dir = Vector2.Zero;

            if (km.IsKeyPressed(Keys.W))
            {
                newPos -= Vector2.UnitY;
                dir = -Vector2.UnitY;
            }
            if (km.IsKeyPressed(Keys.S))
            {
                newPos += Vector2.UnitY;
                dir = Vector2.UnitY;
            }
            if (km.IsKeyPressed(Keys.A))
            {
                newPos -= Vector2.UnitX;
                dir = -Vector2.UnitX;
            }
            if (km.IsKeyPressed(Keys.D))
            {
                newPos += Vector2.UnitX;
                dir = Vector2.UnitX;
            }

            if(map[(int)newPos.X, (int)newPos.Y] == 'X')
            {
                newPos = playerPos;
            }
            else if(map[(int)newPos.X, (int)newPos.Y] == 'B')
            {
                if(map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == ' ' || map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.')
                {
                    map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                    map[(int)(newPos.X), (int)(newPos.Y)] = ' ';
                }
                else
                {
                    newPos = playerPos;
                }
            }

            map[(int)playerPos.X, (int)playerPos.Y] = ' ';
            playerPos = newPos;
            map[(int)playerPos.X, (int)playerPos.Y] = 'i';
        }
    }
}