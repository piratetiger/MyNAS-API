using System.Collections.Generic;

namespace MyNAS.Model
{
    public interface INASFilterRequest : IDateFilterRequest, IOwnerFilterRequest
    {
        new string Cate { get; }
    }
}