using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MyNAS.Model;
using Newtonsoft.Json.Serialization;

namespace MyNAS.Site.Helper
{
    public class MyNASJsonContractResolver : CamelCasePropertyNamesContractResolver
    {
        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            return objectType.GetProperties()
                             .Where(pi => !Attribute.IsDefined(pi, typeof(JsonIgnoreSerializationAttribute)))
                             .ToList<MemberInfo>();
        }
    }
}