using System;
using System.Collections.Generic;

namespace MyNAS.Model
{
    public interface IOwnerFilterRequest
    {
        List<string> Owner { get; }
        string Cate { get; }
    }
}