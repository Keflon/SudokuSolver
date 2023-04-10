using SudokuSolver;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

internal unsafe class Sudoku
{
    private readonly int[] _startingGridCopy;
    private int* _endCell;

    public Sudoku(int[] startingGrid)
    {
        _startingGridCopy = (int[])startingGrid.Clone();


        /*
TODO:

Group 9 numbers into row instances.
Group 9 numbers into column instances.
Group 9 numbers into box instances.

The distinction between row, column and box instances should not matter,
=> create 27 NumberGroups.
Use an int for each numbergroup and set a bit for each 'number' that it contains.
        (if the group has a '4', set bit 4, ...)

Map each x, y of the starting grid into all relevant NumberGroup instances.
- Each cell is in a row, column and grid, so each x, y should map to 3 NumberGroup instances

Brute-force every value for x, y.
Use recursion.
Unwind if a path fails.

 */
    }
    public bool Solve()
    {
        // 9 rows, 9 columns and 9 'boxes'.
        int[] numberGroups = new int[9 * 3];

        // 9 * 9 Soduku cells. This maps each cell to 3 NumberGroups.
        int*[] groupMap = new int*[9 * 9 * 3];

        fixed (int* grid = &_startingGridCopy[0])
        fixed (int* groups = &numberGroups[0])
        fixed (int** map = &groupMap[0])
        {
            AddRows(groups, map, _startingGridCopy);
            AddColumns(groups, map, _startingGridCopy);
            AddBoxes(groups, map, _startingGridCopy);

            //PrintMap(map, 0);
            //PrintMap(map, 1);
            //PrintMap(map, 2);
            _endCell = grid + 9 * 9;
            return Solve(grid, map);
        }
    }
    private void PrintMap(int** groupMap, int type)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Console.Write($"{*groupMap[(y * 9 + x) * 3 + type]}  ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

        for (int c = 0; c < 9 * 9 * 3; c += 3)
        {
            if (c % 27 == 0)
                Console.WriteLine();
            Console.Write($"{**groupMap}  ");
            groupMap += 3;
        }
        Console.WriteLine();
        Console.WriteLine();
    }
    private void AddRows(int* numberGroups, int** groupMap, int[] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (startingGrid[y * 9 + x] != 0)
                    numberGroups[y] |= 1 << startingGrid[y * 9 + x];
                groupMap[(y * 9 + x) * 3 + 0] = &numberGroups[y];
            }
        }
    }
    private void AddColumns(int* numberGroups, int** groupMap, int[] startingGrid)
    {
        for (int x = 0; x < 9; x++)
        {
            for (int y = 0; y < 9; y++)
            {
                if (startingGrid[y * 9 + x] != 0)
                    numberGroups[x + 9] |= 1 << startingGrid[y * 9 + x];
                groupMap[(y * 9 + x) * 3 + 1] = &numberGroups[x + 9];
            }
        }
    }
    private void AddBoxes(int* numberGroups, int** groupMap, int[] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                int xindex = x / 3;
                int yindex = y / 3;

                var boxGroupIndex = yindex * 3 + xindex;

                if (startingGrid[y * 9 + x] != 0)
                {
                    // Get the boxGroup.
                    // 012
                    // 345
                    // 678
                    numberGroups[9 + 9 + boxGroupIndex] |= (1 << startingGrid[y * 9 + x]);
                }
                groupMap[(y * 9 + x) * 3 + 2] = &numberGroups[9 + 9 + boxGroupIndex];
            }
        }
    }

    public void PrintGrid()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
                Console.Write(_startingGridCopy[y * 9 + x]);

            Console.WriteLine();
        }
    }

    private bool Solve(int* cell, int** map)
    {
        if (cell == _endCell) 
            return true;

        if (*cell == 0) // If no seed value ...
        {
            int mask = 1;
            for (int c = 1; c < 10; c++) // Try 1..9
            {
                mask <<= 1;
                if (Try(map, mask) == true) // c might work at current index ...
                {
                    if (Solve(cell + 1, map + 3) == false) // c ultimately didn't work.
                        RemoveTry(map, mask);
                    else
                    {
                        // *cell is 0.
                        *cell = c;
                        return true;
                    }
                }
            }
            return false;
        }
        else
            return Solve(cell + 1, map + 3);
    }

    private void RemoveTry(int** map, int mask)
    {
        **map++ ^= mask;
        **map++ ^= mask;
        **map ^= mask;
    }

    private bool Try(int** map, int mask)
    {
        if ((**map & mask) != 0)
            return false;
        map++;
        if ((**map & mask) != 0)
            return false;
        map++;
        if ((**map & mask) != 0)
            return false;

        **map-- |= mask;
        **map-- |= mask;
        **map |= mask;

        return true;
    }
}
