namespace SudokuSolver
{
    internal class NumberGroup
    {
        int _group;
        public NumberGroup()
        {
            _group = 0;
        }

        public bool Contains(int mask)
        {
            return (_group & mask) != 0;
        }

        public void Add(int mask)
        {
#if DEBUG
            if (Contains(mask) == true)
                throw new InvalidOperationException("Number already present");
#endif
            _group |= mask;
        }

        public void Remove(int mask)
        {
#if DEBUG
            if (Contains(mask) == false)
                throw new InvalidOperationException("Number not present");
#endif
            _group ^= mask;
        }
    }
}
