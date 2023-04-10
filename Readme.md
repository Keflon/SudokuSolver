# Sudoku solver.

## And a very fast one at that.
I got a bit carried away after a [friend](https://github.com/PhilSnape) challenged me to see how quickly I could solve 
a Sudoku puzzle in csharp.  
`Program.cs` has some test data you can play with and a `#define` to turn on / off perf testing.  
- Bad input isn't validated.
- Only the first solution is found. (A true Sudoku has one and only one solution)
- Unsolvable input is detected, as long as it is valid.
- It's very fast!

# Process:
For the 9x9 input grid, there are 9 rows, 9 columns and 9 3x3 'boxes', 
each of which is represented by a 'group'.  

Each of these 27 groups maintains the presence or absence of each number 1 through 9.  

To succeed, each group must contain all numbers 1..9.  

Each cell of the input affects 3 groups, (one row, one column and one box) so a mapping is built from 
each cell to each of its 3 groups.  

The algorithm starts at the first empty cell and considers putting each number 1..9 into it.  

If the test-number is not present in any of the 3 groups the cell maps to, the number is speculatively 
added to each group and the process recurses for the next empty cell.  

If the recursion succeeds, the number under test is correct, so it is written to the output. Otherwise, 
the number is removed from the groups and the next number is tested.  

Each group is represented by a bitfield, where bits 1..9 represent the presence or absence of the numbers 
1..9 in any one of the cells the group represents.  

I got a bit carried away with pointers and that let to a 20x increase in performance. 
You can follow the journey in the GIT history.

If you've got this far, please consider starring the repo.





