namespace MyNAS.Model.Files
{
    public class CreateFolderRequest
    {
        public string Cate { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
    }
}