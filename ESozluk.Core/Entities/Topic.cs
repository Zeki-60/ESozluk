namespace ESozluk.Domain.Entities
{
    public class Topic
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public DateTime CreateDate { get; set; }
        public List<Entry>? Entries { get; set; }
    }
}
