using System;

namespace MyNAS.Model
{
    public abstract class NASInfoModel : IKeyNameModel, IDateModel, IOwnerModel
    {
        public virtual long Id { get; set; }
        public abstract string Type { get; }
        public virtual string KeyName { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual string Owner { get; set; }
        public virtual string FileName { get; set; }
        public virtual bool IsPublic { get; set; }
        public virtual string Cate { get; set; }

        public static T FromModel<T>(NASInfoModel model) where T : NASInfoModel, new()
        {
            var result = new T();
            result.Id = model.Id;
            result.KeyName = model.KeyName;
            result.Date = model.Date;
            result.Owner = model.Owner;
            result.FileName = model.FileName;
            result.IsPublic = model.IsPublic;
            result.Cate = model.Cate;

            return result;
        }
    }
}