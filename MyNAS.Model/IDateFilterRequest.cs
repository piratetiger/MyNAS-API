using System;

namespace MyNAS.Model
{
    public interface IDateFilterRequest
    {
        string Start { get; }
        string End { get; }
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        string Cate { get; }
    }
}