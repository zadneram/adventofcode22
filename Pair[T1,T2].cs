using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode2022
{
    class Pair<T1, T2> : IEquatable<Pair<T1, T2>>
        where T1 : IEquatable<T1>
        where T2 : IEquatable<T2>
    {
        public T1 Item1 { get; set; }

        public T1 x
        {
            get { return Item1; }
            set { Item1 = value; }
        }

        public T2 Item2 { get; set; }

        public T2 y
        {
            get { return Item2; }
            set { Item2 = value; }
        }

        public Pair(T1 item1, T2 item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public override string ToString()
        {
            return $"({Item1},{Item2})";
        }

        public bool Equals(Pair<T1, T2> other)
        {
            if (other == null)
            {
                return false;
            }
            else if (this.Item1.Equals(other.Item1) && this.Item2.Equals(other.Item2))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public override int GetHashCode()
        {
            return Item1.GetHashCode() ^ Item2.GetHashCode();
        }
    }
}