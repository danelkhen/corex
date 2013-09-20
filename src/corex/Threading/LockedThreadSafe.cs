using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Corex.Threading
{
    public class LockedThreadSafe<T>
    {
        public LockedThreadSafe(T value)
        {
            Value = value;
        }

        T Value;
        object Entrance = new object();
        public void Access(Action<T> action)
        {
            lock (Entrance)
            {
                action(Value);
            }
        }
    }
}
