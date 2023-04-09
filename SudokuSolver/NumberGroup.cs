namespace SudokuSolver
{
    //internal class NumberGroup
    //{
    //    bool[] _group;
    //    public NumberGroup()
    //    {
    //        _group = new bool[10];
    //    }

    //    public bool Contains(int number)
    //    {
    //        return _group[number];
    //    }

    //    public bool Add(int number)
    //    {
    //        if (_group[number] == true)
    //            throw new InvalidOperationException("Number already present");
    //        _group[number] = true;
    //        return true;
    //    }

    //    public bool Remove(int number)
    //    {
    //        if (_group[number] == false)
    //            throw new InvalidOperationException("Number not present");
    //        _group[number] = false;
    //        return true;
    //    }
    //}

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
