using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;

namespace Meowtrix
{
    public static class DelegateHelper
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Exception TryInvokeEach<T>(this Action<T> @delegate, T arg)
        {
            if (@delegate == null) return null;
            var list = @delegate.GetInvocationList();
            if (list.Length == 1)
            {
                try
                {
                    @delegate(arg);
                }
                catch (Exception e)
                {
                    return e;
                }
                return null;
            }
            else
            {
                Exception ex = null;
                foreach (Action<T> handler in list)
                    try
                    {
                        handler(arg);
                    }
                    catch (Exception e)
                    {
                        ex = ExceptionHelper.Combine(ex, e);
                    }
                return ex;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvokeEach<T>(this Action<T> @delegate, T arg)
        {
            if (@delegate == null) return;
            var list = @delegate.GetInvocationList();
            if (list.Length == 1)
                @delegate(arg);
            else
            {
                Exception ex = null;
                int exceptionCount = 0;
                foreach (Action<T> handler in list)
                    try
                    {
                        handler(arg);
                    }
                    catch (Exception e)
                    {
                        ex = ExceptionHelper.Combine(ex, e);
                        exceptionCount++;
                    }
                if (exceptionCount == 0) return;
                if (exceptionCount == 1)
                {
                    var info = ExceptionDispatchInfo.Capture(ex);
                    info.Throw();
                }
                else throw ex;
            }
        }
    }
}
