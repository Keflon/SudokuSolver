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
        - Each cell is in a row, column and grid, so each x, y should map to 3 NumberGroup instances.

        NumberGroup.CanHave(number) : Returns whether that number group already contains the given nuumber.
        NumberGroup.Add(number)     : Adds a number to a NumberGroup.
        NumberGroup.Remove(number   : Removes a number from a NumberGroup.

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

            for (int x = 0; x < 9; x++)
            {
                groupMap[y * 9 + x].Add(group);
                if (startingGrid[y * 9 + x] != 0)
                    group.Add(startingGrid[y * 9 + x]);
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
                    group.Add(startingGrid[y * 9 + x]);
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
                    boxGroups[yindex * 3 + xindex].Add(startingGrid[y * 9 + x]);
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

        if (_startingGrid[startIndex] == 0)
        {
            for (int c = 1; c < 10; c++) // Try 1..9
            {
                if (Try(startIndex, c) == true) // c might work at current index ...
                {
                    AddTry(startIndex, c);
                    if (Solve(startIndex + 1) == false) // c ultimately didn't work.
                        RemoveTry(startIndex, c);
                    else
                        return true;
                }
            }
            return false;
        }
        else
            return Solve(startIndex + 1);
    }

    private void AddTry(int index, int c)
    {
        foreach (var numberGroup in _groupMap[index])
            numberGroup.Add(c);

        _startingGrid[index] = c;
    }

    private void RemoveTry(int index, int c)
    {
        foreach (var numberGroup in _groupMap[index])
            numberGroup.Remove(c);

        _startingGrid[index] = 0;
    }

    private bool Try(int index, int c)
    {
        foreach (var numberGroup in _groupMap[index])
            if (numberGroup.Contains(c)) // Number is already present.
                return false;

        return true;
    }
}
