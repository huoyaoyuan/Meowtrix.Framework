using System;
using System.Runtime.CompilerServices;

namespace Meowtrix
{
    public static class ExceptionHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception Combine(Exception a, Exception b)
        {
            if (a == null) return b;
            else if (b == null) return a;
            else return new AggregateException(a, b).Flatten();
        }
    }
}
