﻿namespace LabyrinthGameEngine
{
    using System;

    internal abstract class LabyrinthFactoryAbstract
    {
        protected char[,] currentMatrix = null;

        protected int labyrinthRows = 0;
        protected int labyrinthCols = 0;

        protected int centerOfCols = 0;
        protected int centerOfRows = 0;

        internal virtual ILabyrinth CreateLabyrinth(int labyrinthRows, int labyrinthCols)
        {
            this.labyrinthRows = labyrinthRows;
            this.labyrinthCols = labyrinthCols;

            this.centerOfCols = this.labyrinthCols / 2;
            this.centerOfRows = this.labyrinthRows / 2;

            int initialPlayerPositionX = centerOfCols;
            int initialPlayerPositionY = centerOfRows;

            this.currentMatrix = this.GenerateMatrix();

            while (!CheckIfAnyExit((char[,])this.currentMatrix.Clone(), initialPlayerPositionX, initialPlayerPositionY))
            {
                this.currentMatrix = this.GenerateMatrix();
            }

            return new Labyrinth(this.currentMatrix);
        }

        protected abstract char[,] GenerateMatrix();

        protected virtual bool CheckIfAnyExit(char[,] labyrinth, int positionX, int positionY)
        {
            char visitedSymbol = '0';

            int botStartPositionX = positionX;
            int botStartPositionY = positionY;

            labyrinth[botStartPositionY, botStartPositionX] = visitedSymbol;

            // Border cases START
            if (botStartPositionX == 0 ||
                botStartPositionX == this.labyrinthRows - 1 ||
                botStartPositionY == 0 ||
                botStartPositionY == this.labyrinthCols - 1)
            {
                return true;
            }

            if (labyrinth[botStartPositionY + 1, botStartPositionX] == Labyrinth.WALL_SYMBOL &&
                labyrinth[botStartPositionY, botStartPositionX + 1] == Labyrinth.WALL_SYMBOL &&
                labyrinth[botStartPositionY - 1, botStartPositionX] == Labyrinth.WALL_SYMBOL &&
                labyrinth[botStartPositionY, botStartPositionX - 1] == Labyrinth.WALL_SYMBOL)
            {
                return false;
            }
            // Border cases END

            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                if (IsEmptyCell(labyrinth, botStartPositionX, botStartPositionY, direction, visitedSymbol))
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual bool IsEmptyCell(char[,] labyrinth, int botStartPositionX, int botStartPositionY, Direction direction, char visitedSymbol)
        {
            bool isEmpty = false;

            int nextPositionY = botStartPositionY;
            int nextPositionX = botStartPositionX;

            switch (direction)
            {
                case Direction.Up:
                    nextPositionY = botStartPositionY - 1;
                    break;
                case Direction.Right:
                    nextPositionX = botStartPositionX + 1;
                    break;
                case Direction.Down:
                    nextPositionY = botStartPositionY + 1;
                    break;
                case Direction.Left:
                    nextPositionX = botStartPositionX - 1;
                    break;
            }

            if (labyrinth[nextPositionY, nextPositionX] == Labyrinth.BLANK_SYMBOL)
            {
                labyrinth[nextPositionY, nextPositionX] = visitedSymbol;

                if (CheckIfAnyExit(labyrinth, nextPositionX, nextPositionY))
                {
                    isEmpty = true;
                }
            }
            return isEmpty;
        }
    }
}
