using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sudoku_Solver
{
    public class SudokuClass
    {
        public static long recursivityCounter = 0;
        public static int gridSize = 9;
        public static int gridSizeMinus = gridSize - 1;
        public static int minValue = 1;
        public static int maxValue = 9;
        public static int emptyValue = 0;
        public int[,] outputGrid = new int[gridSize, gridSize];

        private int[,] emptyGrid(int emptyValue)
        {
            int[,] grid = new int[gridSize, gridSize];

            for (int x = 0; x <= gridSizeMinus; x++)
            {
                for (int y = 0; y <= gridSizeMinus; y++)
                {
                    grid[x, y] = emptyValue;
                }
            }
            return grid;
        }

        private bool gridCheck(int[,] grid)
        {

            int[] stripe = new int[gridSize];

            for (int x = 0; x <= gridSizeMinus; x++)
            {
                for (int y = 0; y <= gridSizeMinus; y++)
                {
                    stripe[y] = grid[x, y];
                }
                if (checkForDoubles(stripe)) { return false; }
            }

            for (int y = 0; y <= gridSizeMinus; y++)
            {
                for (int x = 0; x <= gridSizeMinus; x++)
                {
                    stripe[x] = grid[x, y];
                }
                if (checkForDoubles(stripe)) { return false; }
            }

            if (checkHouse(0, 2, 0, 2, grid)) { return false; }
            if (checkHouse(3, 5, 0, 2, grid)) { return false; }
            if (checkHouse(6, 8, 0, 2, grid)) { return false; }
            if (checkHouse(0, 2, 3, 5, grid)) { return false; }
            if (checkHouse(3, 5, 3, 5, grid)) { return false; }
            if (checkHouse(6, 8, 3, 5, grid)) { return false; }
            if (checkHouse(0, 2, 6, 8, grid)) { return false; }
            if (checkHouse(3, 5, 6, 8, grid)) { return false; }
            if (checkHouse(6, 8, 6, 8, grid)) { return false; }

            return true;
        }

        private bool checkHouse(int xFrom, int xTo, int yFrom, int yTo, int[,] grid)
        {

            int[] stripe = new int[gridSize];
            int i = 0;


            for (int x = xFrom; x <= xTo; x++)
            {
                for (int y = yFrom; y <= yTo; y++)
                {
                    stripe[i] = grid[x, y];
                    i++;
                }
            }
            if (checkForDoubles(stripe)) { return true; }
            return false;
        }

        private bool checkForDoubles(int[] enter)
        {
            int[] check = new int[10];
            for (int i = 0; i < gridSize; i++)
            {
                check[enter[i]]++;
                if (check[enter[i]] >= 2 && enter[i] != emptyValue) { return true; }
            }
            return false;
        }

        private int[] findNextFreeCell(int[,] grid)
        {
            int[] returnedCell = new int[2];

            for (int x = 0; x <= gridSizeMinus; x++)
            {
                for (int y = 0; y <= gridSizeMinus; y++)
                {
                    if (grid[x, y] == 0) { returnedCell[0] = x; returnedCell[1] = y; return returnedCell; }
                }
            }

            returnedCell[0] = returnedCell[1] = -1;
            return returnedCell;
        }

        private void fillOutputGrid(int[,] grid)
        {

            for (int x = 0; x <= gridSizeMinus; x++)
            {
                for (int y = 0; x <= gridSizeMinus; y++)
                {
                    outputGrid[x, y] = grid[x, y];
                }
            }
        }

        public bool Sudoku(int[,] grid, Action<int[,], int, int> callBack, bool fullGraphicVersion, bool resetCounter)
        {

            if (resetCounter)
            {
                recursivityCounter = 0;
                resetCounter = false;
            }

            recursivityCounter++;
            int[] coordonatesOfFreeCell = new int[2];

            if (gridCheck(grid) == false) { return false; }

            coordonatesOfFreeCell = findNextFreeCell(grid);
            if (coordonatesOfFreeCell[0] == -1)
            {
                outputGrid = grid;
                return true;
            }

            for (int i = minValue; i <= maxValue; i++)
            {
                grid[coordonatesOfFreeCell[0], coordonatesOfFreeCell[1]] = i;
                if (fullGraphicVersion) {callBack(grid, coordonatesOfFreeCell[0], coordonatesOfFreeCell[1]);}
                
                bool isGridOk = Sudoku(grid, callBack, fullGraphicVersion, resetCounter);

                if (isGridOk) { return true; }
            }
            grid[coordonatesOfFreeCell[0], coordonatesOfFreeCell[1]] = 0;
            return false;
        }

        public int[,] getGrid()
        {
            return outputGrid;
        }

        public long getCounter()
        {
            return recursivityCounter;
        }
    }
}