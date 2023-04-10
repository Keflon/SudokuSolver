internal unsafe class Sudoku
{
    private int[]? _outputGrid;
    private int* _endCell;
    private int[] _groupMapOffsets;

    public Sudoku()
    {
        // The map is the same for each input, but we cannot precalculate using pointers, 
        // because the groups are not 'fixed' in memory until Solve(..) is called.
        // Instead, precalculate offsets, and translate those offsets to pointers
        // as soon as the input is fixed
        _groupMapOffsets = new int[9 * 9 * 3];

        fixed (int* map = &_groupMapOffsets[0])
        {
            AddRows(map);
            AddColumns(map);
            AddBoxes(map);
        }

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
    public bool Solve(int[] startingGrid)
    {
        _outputGrid = new int[9 * 9];

        // 9 rows, 9 columns and 9 'boxes'. 27 groups.
        int[] numberGroups = new int[9 * 3];

        // 9 * 9 Soduku cells. This maps each cell to 3 groups.
        int*[] groupMap = new int*[9 * 9 * 3];

        fixed (int* inputGrid = &startingGrid[0])
        fixed (int* outputGrid = &_outputGrid[0])
        fixed (int* groups = &numberGroups[0])
        fixed (int* mapOffsets = &_groupMapOffsets[0])
        fixed (int** map = &groupMap[0])
        {
            _endCell = outputGrid + 9 * 9;
            BuildMap(groups, mapOffsets, map);
            MapInput(inputGrid, outputGrid, map);

            //PrintMap(map, 0);
            //PrintMap(map, 1);
            //PrintMap(map, 2);
            return Solve(outputGrid, map);
        }
    }

    /// <summary>
    /// Map mapOffsets to fixed addresses.
    /// </summary>
    /// <param name="groups"></param>
    /// <param name="mapOffsets"></param>
    /// <param name="map"></param>
    private void BuildMap(int* groups, int* mapOffsets, int** map)
    {
        for (int c = 0; c < 9 * 9 * 3; c++)
            //*map++ = &groups[*mapOffsets++]; // Same. 
            *map++ = groups + (*mapOffsets++); // Same.
    }

    /// <summary>
    /// Feed the map with the starting grid and initialise the output.
    /// </summary>
    /// <param name="cell"></param>
    /// <param name="map"></param>
    private void MapInput(int* inputCell, int* outputCell, int** map)
    {
        while (outputCell != _endCell)
        {
            if (*inputCell != 0)
            {
                Try(map, 1 << *inputCell);
                *outputCell = *inputCell;
            }
            else if(*outputCell != 0)
                throw new InvalidOperationException();
            outputCell++;
            inputCell++;
            map += 3;
        }
    }
    private void AddRows(int* groupMap)
    {
        for (int y = 0; y < 9; y++)
            for (int x = 0; x < 9; x++)
                groupMap[(y * 9 + x) * 3 + 0] = y;
    }
    private void AddColumns(int* groupMap)
    {
        for (int x = 0; x < 9; x++)
            for (int y = 0; y < 9; y++)
                groupMap[(y * 9 + x) * 3 + 1] = x + 9;
    }
    private void AddBoxes(int* groupMap)
    {
        for (int y = 0; y < 9; y++)
        {
            int yindex = y / 3;
            for (int x = 0; x < 9; x++)
            {
                int xindex = x / 3;
                // Get the boxGroup.
                // 012
                // 345
                // 678
                var boxGroupIndex = yindex * 3 + xindex;
                groupMap[(y * 9 + x) * 3 + 2] = 9 + 9 + boxGroupIndex;
            }
        }
    }

    public void PrintInput(int[] startingGrid)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
                Console.Write(startingGrid[y * 9 + x]);

            Console.WriteLine();
        }
    }
    public void PrintOutput()
    {
        PrintInput(_outputGrid);
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
#if DEBUG
                        if (*cell != 0)
                            throw new InvalidOperationException();
#endif
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
        //**map++ ^= mask;
        //**map++ ^= mask;
        //**map ^= mask;

        // It is quicker to index into map than to increment map.
        *map[0] ^= mask;
        *map[1] ^= mask;
        *map[2] ^= mask;
    }

    private bool Try(int** map, int mask)
    {
        // It is quicker to index into map than to increment map after each test.
        if ((*map[0] & mask) != 0)
            return false;

        if ((*map[1] & mask) != 0)
            return false;

        if ((*map[2] & mask) != 0)
            return false;

        *map[0] |= mask;
        *map[1] |= mask;
        *map[2] |= mask;

        return true;
    }
}
