using SocialNetwork.Models;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class UserRepository
{
    private string filePath = "Repositories/users.csv";
    public List<User> Users { get; set; } = new List<User>();

    public void Load()
    {
        if (!File.Exists(filePath)) return;

        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var parts = line.Split(',');
            int id = int.Parse(parts[0]);
            // string username = parts[1]; // ignorisati ako nije u konstruktoru
            string name = parts[2];
            string surname = parts[3];
            DateTime dob = DateTime.ParseExact(parts[4], "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Users.Add(new User(id, name, surname, dob));
        }
    }


    public void Save()
    {
        var lines = Users.Select(u => $"{u.Id},,{u.Name},{u.Surname},{u.DateOfBirth.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}");
        File.WriteAllLines(filePath, lines);
    }
}
