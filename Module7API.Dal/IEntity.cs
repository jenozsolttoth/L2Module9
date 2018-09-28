
using System;

namespace Module7API.Dal
{
    public interface IEntity
    {
        Guid Id { get; set; }
        DateTime CreateDate { get; set; }
    }
}
