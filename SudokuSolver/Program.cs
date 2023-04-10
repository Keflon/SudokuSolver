#define PERF_TEST
using System.Diagnostics;

// Number of times to run the test. Make this high enough to let the CPU go turbo, giving quicker average results.
const int loopCount = 100000;

// Average times per solution as optimisations are added, for data starting 0, 0, 0, 0, 0, 0, 0, 2, 6, ...
// Test machine is an Intel i7-10510U laptop running a Release build.
// Milliseconds:0.2142
// Milliseconds:0.1044
// Milliseconds:0.0482
// Milliseconds:0.01454
// Milliseconds:0.0113
Console.WriteLine("Hello, Sudoku!");

int[] startingGrid = new[]
{
#if false
0, 0, 5, 8, 0, 6, 4, 0, 0,
0, 0, 0, 7, 0, 0, 2, 1, 5,
0, 9, 7, 5, 0, 0, 6, 0, 3,
0, 5, 1, 9, 8, 0, 0, 6, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 7, 0, 0, 4, 5, 9, 2, 0,
6, 0, 9, 0, 0, 2, 7, 3, 0,
7, 8, 2, 0, 0, 9, 0, 0, 0,
0, 0, 3, 6, 0, 8, 1, 0, 0
#else
#if false
7, 0, 1, 2, 8, 0, 0, 0, 0,
0, 0, 8, 0, 0, 6, 5, 7, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 8, 0, 0, 0, 0, 7, 1, 2,
0, 1, 0, 0, 0, 0, 0, 5, 0,
3, 6, 5, 0, 0, 0, 0, 9, 0,
0, 0, 0, 0, 0, 0, 0, 0, 0,
0, 9, 2, 4, 0, 0, 6, 0, 0,
0, 0, 0, 0, 1, 8, 2, 0, 5,
#else
#if true
0, 0, 0, 0, 0, 0, 0, 2, 6,
0, 0, 0, 4, 7, 0, 0, 3, 9,
0, 0, 1, 9, 3, 0, 0, 7, 0,
6, 0, 0, 0, 0, 5, 0, 9, 0,
7, 0, 0, 0, 0, 0, 0, 0, 5,
0, 5, 0, 1, 0, 0, 0, 0, 2,
0, 4, 0, 0, 1, 3, 2, 0, 0,
1, 7, 0, 0, 6, 4, 0, 0, 0,
5, 2, 0, 0, 0, 0, 0, 0, 0
 
#endif
#endif
#endif
};

var game = new Sudoku();

#if PERF_TEST
var sw = new Stopwatch();
sw.Start();

for (int c = 0; c < loopCount; c++)
{
    game.Solve(startingGrid);
}
sw.Stop();

Console.WriteLine($"Total iterations: {loopCount}");
Console.WriteLine($"Milliseconds per Solve:{sw.ElapsedMilliseconds / (double)loopCount}  <--=");
Console.WriteLine($"Seconds per Solve:{sw.ElapsedMilliseconds / (double)loopCount / 1000.0}");
#endif
Console.WriteLine();
Console.WriteLine("Input:");
game.PrintInput(startingGrid);
Console.WriteLine();

if (game.Solve(startingGrid) == false)
    Console.WriteLine("Cannot solve. Partial result:");
else
    Console.WriteLine("Solution:");

Console.WriteLine();

Console.WriteLine("Output:");
game.PrintOutput();


return 0;