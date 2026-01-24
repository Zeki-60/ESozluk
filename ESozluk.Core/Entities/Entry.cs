namespace ESozluk.Core.Entities
{
    //bunun kategoriye bağlanmasına gerek varmı
    public class Entry
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId {  get; set; }
        public User? User { get; set; }
        public string Description { get; set; }
        public int TopicId { get; set; }
        public Topic Topic { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public int LikeCount { get; set; }
        public List<Like>? Likes { get; set; }


    }
}
