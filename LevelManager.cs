using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Sokoban
{
    class LevelManager
    {
        string[] levelsPath = {"../../../Content/SokobanLevels/level1.txt",
                                "../../../Content/SokobanLevels/level2.txt",
                                "../../../Content/SokobanLevels/level3.txt",
                                "../../../Content/SokobanLevels/level4.txt",
                                "../../../Content/SokobanLevels/level5.txt" };

        int currentLevel;


        public void NextLevel(ref int height, ref int width, GraphicsDeviceManager graphics, ref char[,] map, int tileSize, ref List<Vector2> objectivePointsPos, ref Vector2 playerPos)
        {
            currentLevel++;
            LoadLevel(ref height, ref width, graphics, ref map, tileSize, ref objectivePointsPos, ref playerPos);
        }

        public void LoadLevel(ref int height, ref int width, GraphicsDeviceManager graphics, ref char[,] map, int tileSize, ref List<Vector2> objectivePointsPos, ref Vector2 playerPos)
        {
            if (currentLevel >= levelsPath.Length) return;


            string[] lines = File.ReadAllLines(levelsPath[currentLevel]);
            map = new char[lines[0].Length, lines.Length];
            objectivePointsPos = new List<Vector2>();



            for (int x = 0; x < lines[0].Length; x++)
                for (int y = 0; y < lines.Length; y++)
                {
                    string currentLine = lines[y];
                    map[x, y] = currentLine[x];

                    if (currentLine[x] == '.')
                        objectivePointsPos.Add(new Vector2(x, y));

                    if (currentLine[x] == 'i')
                        playerPos = new Vector2(x, y);

                }




            height = lines.Length;
            width = lines[0].Length;




            graphics.PreferredBackBufferHeight = height * tileSize;
            graphics.PreferredBackBufferWidth = width * tileSize;
            graphics.ApplyChanges();


        }


    }
}