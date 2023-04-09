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

        public bool Add(int mask)
        {
            //if (Contains(number) == true)
            //    throw new InvalidOperationException("Number already present");
            _group |= mask;
            return true;
        }

        public bool Remove(int mask)
        {
            //if (Contains(number) == false)
            //    throw new InvalidOperationException("Number not present");
            _group ^= mask;
            return true;
        }
    }
}
