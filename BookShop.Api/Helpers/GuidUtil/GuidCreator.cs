using System;
using System.Linq;

namespace BookShop.Api.Helpers.GuidUtil
{
    public static class GuidCreator
    {
        public static Guid CreateGuid()
        {
            Guid guid;
            int result = NativeMethods.UuidCreateSequential(out guid);
            if (result == 0)
            {
                var bytes = guid.ToByteArray();
                var indexes = new int[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };
                return new Guid(indexes.Select(i => bytes[i]).ToArray());
            }
            else
                throw new Exception("Error generating sequential GUID");
        }
    }
}
