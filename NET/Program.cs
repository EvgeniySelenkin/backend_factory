using System;
using System.Collections;
using System.Collections.Generic;

namespace NET
{
    class Program
    {
        static void Main(string[] args)
        {
            var numerable = new Numerable();
            foreach(var i in numerable)
            {
                Console.WriteLine(i);
            }
        }
    }
    
    public class Numerable
    {
        int max = 101;
        public Numerable()
        {

        }
        public NumerableEnumerator GetEnumerator()
        {
            return new NumerableEnumerator();
        }
    }

    public class NumerableEnumerator : IEnumerator
    {
        int i = -1;
        bool under100 = true;

        public NumerableEnumerator()
        {
        }
        public object Current
        {
            get
            {
                if (i == -1 || i > 100)
                    throw new InvalidOperationException();
                return i;
            }
        }
        public bool MoveNext()
        {
            if (i < 101 && under100)
            {
                i++;
                if (i == 100)
                    under100 = false;
                return true;

            }
            else if(i>0 && !under100)
            {
                i--;
                return true;
            }
            else
                return false;
        }
        public void Reset()
        {
            i = -1;

        }
        public void Dispose() { }
    }

}
