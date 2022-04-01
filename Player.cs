using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Sokoban
{
    class Player
    {
        private Vector2 position;
        private KeyboardManager km;

        private SpriteBatch spriteBatch;
        private Texture2D playerTexture;

        public delegate bool Verification();
        public event Verification OnObjectiveReach;

        public Player(KeyboardManager km, SpriteBatch spriteBatch, ContentManager content)
        {
            this.km = km;
            this.spriteBatch = spriteBatch;
            playerTexture = content.Load<Texture2D>("Character4");

        }


        public void SetPlayerPos(Vector2 startingPos)
        {
            this.position = startingPos;
        }

        void Movement(char[,] map, List<Vector2> objectivePointsPos)
        {

            Vector2 newPos = position;
            Vector2 dir = Vector2.Zero;

            if (km.IsKeyPressed(Keys.W))
            {
                newPos -= Vector2.UnitY;
                dir = -Vector2.UnitY;
            }
            if (km.IsKeyPressed(Keys.A))
            {
                newPos -= Vector2.UnitX;
                dir = -Vector2.UnitX;


            }
            if (km.IsKeyPressed(Keys.S))
            {
                newPos += Vector2.UnitY;
                dir = Vector2.UnitY;


            }
            if (km.IsKeyPressed(Keys.D))
            {
                newPos += Vector2.UnitX;
                dir = Vector2.UnitX;


            }



            if (map[(int)newPos.X, (int)newPos.Y] == 'X')
                newPos = position;


            //Box Behaviour
            else if (map[(int)newPos.X, (int)newPos.Y] == 'B')
            {

                if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == 'X' ||
                       map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == 'B')
                    newPos = position;
                else
                    BoxBehaviour(dir, newPos, map, objectivePointsPos);

            }



            map[(int)position.X, (int)position.Y] = ' ';
            position = newPos;
            map[(int)position.X, (int)position.Y] = 'i';






        }


        void BoxBehaviour(Vector2 dir, Vector2 newPos, char[,] map, List<Vector2> objectivePointsPos)
        {
            if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == 'X' ||
                        map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == 'B')
                return;


            if (map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == ' ' ||
               map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.')
            {
                bool isObjective = map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] == '.';

                map[(int)(newPos.X + dir.X), (int)(newPos.Y + dir.Y)] = 'B';
                map[(int)(newPos.X), (int)(newPos.Y)] = ' ';



                foreach (Vector2 pos in objectivePointsPos)
                {
                    if (pos.X == newPos.X + dir.X &&
                        pos.Y == newPos.Y + dir.Y)
                    {
                        OnObjectiveReach?.Invoke();
                        break;
                    }

                }




            }
        }

        public void Update(GameTime gameTime, char[,] map, List<Vector2> objectivePointsPos)
        {
            Movement(map, objectivePointsPos);
        }

        public void Draw(GameTime gameTime, int tileSize)
        {

            spriteBatch.Draw(playerTexture, position * tileSize, Color.White);

        }
    }
}