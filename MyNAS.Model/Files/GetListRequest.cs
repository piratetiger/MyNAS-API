using System;
using System.Collections.Generic;

namespace MyNAS.Model.Files
{
    public class GetListRequest : IOwnerFilterRequest
    {
        public List<string> Owner { get; set; }
        public string Cate { get; set; }
    }
}