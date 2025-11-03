using SocialNetwork.Models;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class UserRepository
{
    private string filePath = "Repositories/users.csv";
    private string groupsFile = "Repositories/groups.csv";
    private string membershipsFile = "Repositories/memberships.csv";
    public List<User> Users { get; set; } = new List<User>();
    public List<Group> Groups { get; set; } = new List<Group>();

    public void Load()
    {
        if (!File.Exists(filePath)) return;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            int id = int.Parse(parts[0]);
            string name = parts[2];      // jer parts[1] je prazno
            string surname = parts[3];
            DateTime dob = DateTime.ParseExact(parts[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Users.Add(new User(id, name, surname, dob));
        }
        if (File.Exists(groupsFile))
        {
            var groupLines = File.ReadAllLines(groupsFile);
            foreach (var line in groupLines)
            {
                var parts = line.Split(',');
                int id = int.Parse(parts[0]);
                string name = parts[1];
                DateTime creationDate = DateTime.ParseExact(parts[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);

                Groups.Add(new Group(id, name, creationDate));
            }
        }

        // Ucitavanje membership-a
        if (File.Exists(membershipsFile))
        {
            var membershipLines = File.ReadAllLines(membershipsFile);
            foreach (var line in membershipLines)
            {
                var parts = line.Split(',');
                int userId = int.Parse(parts[0]);
                int groupId = int.Parse(parts[1]);

                var user = Users.FirstOrDefault(u => u.Id == userId);
                var group = Groups.FirstOrDefault(g => g.Id == groupId);

                if (user != null && group != null)
                {
                    user.Groups.Add(group); // samo User zna svoje grupe
                }
            }
        }
    }


    public void Save()
    {
        var lines = Users.Select(u => $"{u.Id},,{u.Name},{u.Surname},{u.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}");
        File.WriteAllLines(filePath, lines);
    }
}
