namespace ESozluk.Core.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public int EntryId { get; set; }
        public Entry? Entry { get; set; }
        public DateTime LikeDate { get; set; }
    }
}
