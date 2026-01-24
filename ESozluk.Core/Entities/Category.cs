using System.Diagnostics;

namespace ESozluk.Core.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Topic>? Topics { get; set; }

    }

    //public static class StringExtension
    //{
    //    public static string MontaginedenDenemeler(this string yazi)
    //    {
    //        Debugger.Break();
    //        return yazi;
    //    }
    //}

}
