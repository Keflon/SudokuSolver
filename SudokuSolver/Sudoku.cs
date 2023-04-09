using SudokuSolver;

internal class Sudoku
{
    private int[] _startingGrid;
    private readonly List<NumberGroup> _numberGroups;
    private readonly List<NumberGroup>[] _groupMap;

    public Sudoku(int[] startingGrid)
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
        - Each cell is in a row, column and grid, so each x, y should map to 3 NumberGroup instances

        Brute-force every value for x, y.
        Use recursion.
        Unwind if a path fails.

         */

        // 9 rows, 9 columns and 9 'boxes'.
        _numberGroups = new List<NumberGroup>(9 * 3);

        // 9 * 9 Soduku cells. This maps each cell to 3 NumberGroups.
        _groupMap = new List<NumberGroup>[9 * 9];

        for (int c = 0; c < 9 * 9; c++)
            _groupMap[c] = new List<NumberGroup>();

        AddRows(_numberGroups, _groupMap, startingGrid);
        AddColumns(_numberGroups, _groupMap, startingGrid);
        AddBoxes(_numberGroups, _groupMap, startingGrid);
    }

    private void AddRows(List<NumberGroup> numberGroups, List<NumberGroup>[] groupMap, int[] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            var group = new NumberGroup();
            int mask = 1;
            for (int x = 0; x < 9; x++)
            {
                mask >>= 1;
                groupMap[y * 9 + x].Add(group);
                if (startingGrid[y * 9 + x] != 0)
                    group.Add(1 << startingGrid[y * 9 + x]);
            }
            numberGroups.Add(group);
        }
    }
    private void AddColumns(List<NumberGroup> numberGroups, List<NumberGroup>[] groupMap, int[] startingGrid)
    {
        for (int x = 0; x < 9; x++)
        {
            var group = new NumberGroup();

            for (int y = 0; y < 9; y++)
            {
                groupMap[y * 9 + x].Add(group);
                if (startingGrid[y * 9 + x] != 0)
                    group.Add(1 << startingGrid[y * 9 + x]);
            }
            numberGroups.Add(group);
        }
    }
    private void AddBoxes(List<NumberGroup> numberGroups, List<NumberGroup>[] groupMap, int[] startingGrid)
    {
        var boxGroups = new List<NumberGroup>(9);

        for (int c = 0; c < 9; c++)
            boxGroups.Add(new NumberGroup());

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                var boxGroup = boxGroups[y / 3 * 3 + x / 3];
                groupMap[y * 9 + x].Add(boxGroup);
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
        foreach (var group in boxGroups)
            numberGroups.Add(group);
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
                if (Try(startIndex, mask) == true) // c might work at current index ...
                {
                    AddTry(startIndex, mask);
                    if (Solve(startIndex + 1) == false) // c ultimately didn't work.
                        RemoveTry(startIndex, mask);
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
    private void AddTry(int index, int mask)
    {
        _groupMap[index][0].Add(mask);
        _groupMap[index][1].Add(mask);
        _groupMap[index][2].Add(mask);
    }
    private void RemoveTry(int index, int mask)
    {
        _groupMap[index][0].Remove(mask);
        _groupMap[index][1].Remove(mask);
        _groupMap[index][2].Remove(mask);
    }
    private bool Try(int index, int mask)
    {
        if (_groupMap[index][0].Contains(mask))
            return false;
        if (_groupMap[index][1].Contains(mask))
            return false;
        if (_groupMap[index][2].Contains(mask))
            return false;

        return true;
    }
}
