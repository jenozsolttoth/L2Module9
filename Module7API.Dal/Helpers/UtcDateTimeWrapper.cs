using System;

namespace Module7API.Dal.Helpers
{
    public class UtcDateTimeWrapper : IDateTimeWrapper
    {
        public DateTime Now()
        {
            return DateTime.UtcNow;
        }
    }
}
