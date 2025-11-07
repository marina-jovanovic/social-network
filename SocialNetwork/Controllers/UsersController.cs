using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using Microsoft.Data.Sqlite;
using SocialNetwork.Repositories;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repo;
    private readonly UserDbRepository _dbRepo;


    public UsersController()
    {
        _repo = new UserRepository();
        _repo.Load();

        _dbRepo = new UserDbRepository();
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _dbRepo.GetAll();
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

    [HttpGet("{id}")]
    public IActionResult GetUser(int id)
    {
        try
        {
            var user = _dbRepo.GetById(id);

            if (user == null) return NotFound();
            return Ok(user);
        }
        catch (SqliteException ex)
        {
            Console.WriteLine($"SQLite error fetching user {id}: " + ex.Message);
            return StatusCode(500, "Database error");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"General error fetching user {id}: " + ex.Message);
            return StatusCode(500, "Internal server error");
        }
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
