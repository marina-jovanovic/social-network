namespace SocialNetwork.Models
{
    public class Group
    {
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public List<User> Users { get; set; } = new List<User>();

        public Group (string name, DateTime creationDate)
        {
            Name = name;
            CreationDate = creationDate;
        }
    }
}
