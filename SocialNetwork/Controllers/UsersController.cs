using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using Microsoft.Data.Sqlite;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repo;
    private string _connectionString = "Data Source=database/database.db";

    public UsersController()
    {
        _repo = new UserRepository();
        _repo.Load();
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = GetAllFromDatabase();
            return Ok(users);
        }
        catch (SqliteException ex)
        {
            Console.WriteLine("SQLite error: " + ex.Message);
            return StatusCode(500, "Database error");
        }
        catch (Exception ex)
        {
            Console.WriteLine("General error: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
    }

    private List<User> GetAllFromDatabase()
    {
        var users = new List<User>();

        using SqliteConnection connection = new SqliteConnection(_connectionString);
        connection.Open();

        string query = "SELECT Id, Username, Name, Surname, Birthday FROM Users";

        using var command = new SqliteCommand(query, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            var user = new User
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(2),
                Surname = reader.GetString(3),
                DateOfBirth = DateTime.Parse(reader.GetString(4))
            };
            users.Add(user);
        }

        return users;
    }

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        var user = _repo.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        _repo.Users.Add(user);
        _repo.Save();
        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        var user = _repo.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Surname = updatedUser.Surname;
        user.DateOfBirth = updatedUser.DateOfBirth;

        _repo.Save();
        return NoContent();
    }

    [HttpGet("group/{groupId}")]
    public IActionResult GetUsersByGroup(int groupId)
    {
        var usersInGroup = _repo.Users
            .Where(u => u.Groups.Any(g => g.Id == groupId))
            .ToList();

        if (!usersInGroup.Any())
            return NotFound();

        return Ok(usersInGroup);
    }
}
