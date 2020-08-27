using System;
using System.Runtime.InteropServices;

namespace BookShop.Api.Helpers.GuidUtil
{
    public static class NativeMethods
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        public static extern int UuidCreateSequential(out Guid guid);
    }
}
