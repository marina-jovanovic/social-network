namespace SocialNetwork.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public DateTime DateOfBirth { get; set; }

        public List<Group> Groups { get; set; } = new List<Group>();

        public User (int id, string name, string surname, DateTime dateOfBirth)
        {
            Id = id;
            Name = name;
            Surname = surname;
            DateOfBirth = dateOfBirth;
        }
    }
}
