using System.Collections;
using System.Collections.Generic;

namespace GenericListLibrary
{
    public class GenericListEnumerator<T> : IEnumerator<T>
    {
        private readonly GenericList<T> _genericList;
        private int _currentIndex;

        public GenericListEnumerator(GenericList<T> genericList)
        {
            _currentIndex = -1;
            _genericList = genericList;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (_currentIndex + 1 >= _genericList.Count)
                return false;
            _currentIndex++;
            return true;
        }

        public void Reset()
        {
            _currentIndex = -1;
        }

        public T Current => _genericList.GetElement(_currentIndex);

        object IEnumerator.Current => Current;
    }
}