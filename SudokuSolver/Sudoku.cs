// See https://aka.ms/new-console-template for more information
using SudokuSolver;
using System.ComponentModel.Design;

internal class Sudoku
{
    private int[,] _startingGrid;
    private readonly List<NumberGroup> _numberGroups;
    private readonly List<NumberGroup>[,] _groupMap;

    public Sudoku(int[,] startingGrid)
    {
        _startingGrid = startingGrid;

        /*
        TODO:

        Group 9 numbers into row instances.
        Group 9 numbers into column instances.
        Group 9 numbers into box instances.

        The distinction between row, column and box instances should not matter,
        => create 27 NumberGroup managers.

        Map each x, y of the starting grid into all relevant NumberGroup instances.
        - Each cell is in a row, column and grid, so each x, y should map to 3 NumberGroup instances.

        NumberGroup.CanHave(number) : Returns whether that number group already contains the given nuumber.
        NumberGroup.Add(number)     : Adds a number to a NumberGroup.
        NumberGroup.Remove(number   : Removes a number from a NumberGroup.

        Brute-force every value for x, y.
        Use recursion.
        Unwind if a path fails.

         */

        _numberGroups = new List<NumberGroup>();

        _groupMap = new List<NumberGroup>[9, 9];

        for (int c = 0; c < 9 * 9; c++)
            _groupMap[c / 9, c % 9] = new List<NumberGroup>();

        AddRows(_numberGroups, _groupMap, startingGrid);
        AddColumns(_numberGroups, _groupMap, startingGrid);
        AddBoxes(_numberGroups, _groupMap, startingGrid);
    }

    private void AddRows(List<NumberGroup> numberGroups, List<NumberGroup>[,] groupMap, int[,] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            var group = new NumberGroup();

            for (int x = 0; x < 9; x++)
            {
                groupMap[y, x].Add(group);
                if (startingGrid[y, x] != 0)
                    group.Add(startingGrid[y, x]);
            }
            numberGroups.Add(group);
        }
    }
    private void AddColumns(List<NumberGroup> numberGroups, List<NumberGroup>[,] groupMap, int[,] startingGrid)
    {
        for (int x = 0; x < 9; x++)
        {
            var group = new NumberGroup();

            for (int y = 0; y < 9; y++)
            {
                groupMap[y, x].Add(group);
                if (startingGrid[y, x] != 0)
                    group.Add(startingGrid[y, x]);
            }
            numberGroups.Add(group);
        }
    }
    private void AddBoxes(List<NumberGroup> numberGroups, List<NumberGroup>[,] groupMap, int[,] startingGrid)
    {
        var boxGroups = new List<NumberGroup>(9);

        for (int c = 0; c < 9; c++)
            boxGroups.Add(new NumberGroup());

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                var boxGroup = boxGroups[y / 3 * 3 + x / 3];
                groupMap[y, x].Add(boxGroup);
                if (startingGrid[y, x] != 0)
                {
                    // Get the boxGroup.
                    // 012
                    // 345
                    // 678
                    int xindex = x / 3;
                    int yindex = y / 3;
                    boxGroups[yindex * 3 + xindex].Add(startingGrid[y, x]);
                }
            }
        }
        foreach (var group in boxGroups)
            numberGroups.Add(group);
    }

    public void PrintGrid()
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Console.Write(_startingGrid[y, x]);
            }
            Console.WriteLine();
        }
    }

    internal bool Solve(int startIndex)
    {
        if (startIndex == 9 * 9)
            return true;

        int x = startIndex % 9;
        int y = startIndex / 9;

        if (_startingGrid[y, x] == 0)
        {
            for (int c = 1; c < 10; c++) // Try 1..9
            {
                if (Try(x, y, c) == true) // c might work at x, y ...
                {
                    AddTry(x, y, c);
                    if (Solve(startIndex + 1) == false) // c ultimately didn't work.
                        RemoveTry(x, y, c);
                    else
                        return true;
                }
            }
            return false;
        }
        else
            return Solve(startIndex + 1);
    }

    private void AddTry(int x, int y, int c)
    {
        foreach (var numberGroup in _groupMap[y, x])
            numberGroup.Add(c);

        _startingGrid[y, x] = c;
    }

    private void RemoveTry(int x, int y, int c)
    {
        foreach (var numberGroup in _groupMap[y, x])
                numberGroup.Remove(c);
        
        _startingGrid[y, x] = 0;
    }

    private bool Try(int x, int y, int c)
    {
        foreach (var numberGroup in _groupMap[y, x])
            if (numberGroup.Contains(c)) // Number is already present.
                return false;

        return true;
    }
}
