using System;

namespace MyNAS.Model
{
    public interface INASModel : IKeyNameModel, IDateModel, IOwnerModel
    {
        string FileName { get; }
        bool IsPublic { get; }
        new string Cate { get; }
    }
}