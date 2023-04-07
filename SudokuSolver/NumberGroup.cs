using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuSolver
{
    internal class NumberGroup
    {
        HashSet<int> _group;
        public NumberGroup()
        {
            _group = new HashSet<int>(9);
        }

        public bool Contains(int number)
        {
            return _group.Contains(number);
        }

        public bool Add(int number)
        {
            if (_group.Add(number) == false)
                throw new InvalidOperationException("Number already present");
            return true;
        }

        public bool Remove(int number)
        {
            if (_group.Remove(number) == false)
                throw new InvalidOperationException("Number not present");
            return true;
        }
    }
}
