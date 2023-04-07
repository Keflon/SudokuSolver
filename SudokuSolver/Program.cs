// See https://aka.ms/new-console-template for more information
using System.Diagnostics;

Console.WriteLine("Hello, World!");

int[,] startingGrid = new[,]
{
#if false
{0, 0, 5, 8, 0, 6, 4, 0, 0},
{0, 0, 0, 7, 0, 0, 2, 1, 5},
{0, 9, 7, 5, 0, 0, 6, 0, 3},
{0, 5, 1, 9, 8, 0, 0, 6, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 7, 0, 0, 4, 5, 9, 2, 0},
{6, 0, 9, 0, 0, 2, 7, 3, 0},
{7, 8, 2, 0, 0, 9, 0, 0, 0},
{0, 0, 3, 6, 0, 8, 1, 0, 0}
#else
#if false
{7, 0, 1, 2, 8, 0, 0, 0, 0},
{0, 0, 8, 0, 0, 6, 5, 7, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 8, 0, 0, 0, 0, 7, 1, 2},
{0, 1, 0, 0, 0, 0, 0, 5, 0},
{3, 6, 5, 0, 0, 0, 0, 9, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 9, 2, 4, 0, 0, 6, 0, 0},
{0, 0, 0, 0, 1, 8, 2, 0, 5},
#else
#if true
{0, 0, 0, 0, 0, 0, 0, 2, 6},
{0, 0, 0, 4, 7, 0, 0, 3, 9},
{0, 0, 1, 9, 3, 0, 0, 7, 0},
{6, 0, 0, 0, 0, 5, 0, 9, 0},
{7, 0, 0, 0, 0, 0, 0, 0, 5},
{0, 5, 0, 1, 0, 0, 0, 0, 2},
{0, 4, 0, 0, 1, 3, 2, 0, 0},
{1, 7, 0, 0, 6, 4, 0, 0, 0},
{5, 2, 0, 0, 0, 0, 0, 0, 0}
#else
#if false
{7, 0, 1, 2, 8, 0, 0, 0, 0},
{0, 0, 8, 0, 0, 6, 5, 7, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 8, 0, 0, 0, 0, 7, 1, 2},
{0, 1, 0, 0, 0, 0, 0, 5, 0},
{3, 6, 5, 0, 0, 0, 0, 9, 0},
{0, 0, 0, 0, 0, 0, 0, 0, 0},
{0, 9, 2, 4, 0, 0, 6, 0, 0},
{0, 0, 0, 0, 1, 8, 2, 0, 5}
#endif
#endif
#endif
#endif
};
#if false
var sw = new Stopwatch();
sw.Start();

for (int c = 0; c < 10000; c++)
{
    var startingGridCopy = (int[,])startingGrid.Clone();
    new Sudoku(startingGridCopy).Solve(0);
}
sw.Stop();

Console.WriteLine($"Milliseconds:{sw.ElapsedMilliseconds / 10000.0}");
Console.WriteLine($"Seconds:{sw.ElapsedMilliseconds / 10000.0 /1000.0}");
#endif
var game = new Sudoku(startingGrid);
game.PrintGrid();
var solved = game.Solve(0);
Console.WriteLine();

if (game.Solve(0) == false)
    Console.WriteLine("Cannot solve");
else
    Console.WriteLine("Solution");

Console.WriteLine();

game.PrintGrid();
