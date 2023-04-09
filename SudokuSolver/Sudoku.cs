using SudokuSolver;
using System;
using System.Reflection;

internal unsafe class Sudoku
{
    private int[] _startingGrid;
    private readonly NumberGroup[] _numberGroups;
    private readonly NumberGroup[][] _groupMap;

    public Sudoku(int[] startingGrid)
    {
        _startingGrid = startingGrid;

        /*
        TODO:

        Group 9 numbers into row instances.
        Group 9 numbers into column instances.
        Group 9 numbers into box instances.

        The distinction between row, column and box instances should not matter,
        => create 27 NumberGroups.

        Map each x, y of the starting grid into all relevant NumberGroup instances.
        - Each cell is in a row, column and grid, so each x, y should map to 3 NumberGroup instances

        Brute-force every value for x, y.
        Use recursion.
        Unwind if a path fails.

         */

        // 9 rows, 9 columns and 9 'boxes'.
        _numberGroups = new NumberGroup[9 * 3];

        // 9 * 9 Soduku cells. This maps each cell to 3 NumberGroups.
        _groupMap = new NumberGroup[9 * 9][];

        for (int c = 0; c < 9 * 9; c++)
            _groupMap[c] = new NumberGroup[3];

        AddRows(_numberGroups, _groupMap, startingGrid);
        AddColumns(_numberGroups, _groupMap, startingGrid);
        AddBoxes(_numberGroups, _groupMap, startingGrid);
    }

    private void AddRows(NumberGroup[] numberGroups, NumberGroup[][] groupMap, int[] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            var group = new NumberGroup();
            int mask = 1;
            for (int x = 0; x < 9; x++)
            {
                mask >>= 1;
                groupMap[y * 9 + x][0]=(group);
                if (startingGrid[y * 9 + x] != 0)
                    group.Add(1 << startingGrid[y * 9 + x]);
            }
            numberGroups[y] = group;
        }
    }
    private void AddColumns(NumberGroup[] numberGroups, NumberGroup[][] groupMap, int[] startingGrid)
    {
        for (int x = 0; x < 9; x++)
        {
            var group = new NumberGroup();

            for (int y = 0; y < 9; y++)
            {
                groupMap[y * 9 + x][1]=(group);
                if (startingGrid[y * 9 + x] != 0)
                    group.Add(1 << startingGrid[y * 9 + x]);
            }
            numberGroups[x + 9] = group;
        }
    }
    private void AddBoxes(NumberGroup[] numberGroups, NumberGroup[][] groupMap, int[] startingGrid)
    {
        var boxGroups = new NumberGroup[9];

        for (int c = 0; c < 9; c++)
            boxGroups[c]=new NumberGroup();

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                var boxGroup = boxGroups[y / 3 * 3 + x / 3];
                groupMap[y * 9 + x][2]=(boxGroup);
                if (startingGrid[y * 9 + x] != 0)
                {
                    // Get the boxGroup.
                    // 012
                    // 345
                    // 678
                    int xindex = x / 3;
                    int yindex = y / 3;
                    boxGroups[yindex * 3 + xindex].Add(1 << startingGrid[y * 9 + x]);
                }
            }
        }
        for (int c = 0; c < 9; c++)
            numberGroups[c + 9 + 9] = boxGroups[c];
    }

    public void PrintGrid()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
                Console.Write(_startingGrid[y * 9 + x]);

            Console.WriteLine();
        }
    }

    internal bool Solve(int startIndex)
    {
        if (startIndex == 9 * 9)
            return true;

        if (_startingGrid[startIndex] == 0) // If no seed value ...
        {
            int mask = 1;
            for (int c = 1; c < 10; c++) // Try 1..9
            {
                mask <<= 1;
                if (Try(_groupMap[startIndex], mask) == true) // c might work at current index ...
                {
                    if (Solve(startIndex + 1) == false) // c ultimately didn't work.
                        RemoveTry(_groupMap[startIndex], mask);
                    else
                    {
                        _startingGrid[startIndex] = c;
                        return true;
                    }
                }
            }
            return false;
        }
        else
            return Solve(startIndex + 1);
    }

    private void RemoveTry(NumberGroup[] numberGroups, int mask)
    {
        numberGroups[0].Remove(mask);
        numberGroups[1].Remove(mask);
        numberGroups[2].Remove(mask);
    }

    private bool Try(NumberGroup[] numberGroups, int mask)
    {
        if (numberGroups[0].Contains(mask))
            return false;
        if (numberGroups[1].Contains(mask))
            return false;
        if (numberGroups[2].Contains(mask))
            return false;

        numberGroups[0].Add(mask);
        numberGroups[1].Add(mask);
        numberGroups[2].Add(mask);

        return true;
    }
}
