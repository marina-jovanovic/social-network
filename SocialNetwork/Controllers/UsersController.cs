using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;
using Microsoft.Data.Sqlite;
using SocialNetwork.Repositories;
using Microsoft.Extensions.Configuration;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repo;
    private readonly UserDbRepository _dbRepo;


    public UsersController(IConfiguration configuration)
    {
        _repo = new UserRepository();
        _repo.Load();

        _dbRepo = new UserDbRepository(configuration);
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        try
        {
            var users = _dbRepo.GetAll();
            return Ok(users);
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

            if (user == null) return NotFound($"User with the requested ID {id} was not found in the database.");
            return Ok(user);
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
        try
        {
            var createdUser = _dbRepo.Create(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error during user creation: " + ex.Message);
            return StatusCode(500, "Internal server error during creation");
        }
    }

    [HttpPut("{id}")]
    public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
    {
        try
        {
            updatedUser.Id = id;

            bool updated = _dbRepo.Update(updatedUser);

            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during user update {id}: " + ex.Message);
            return StatusCode(500, "Internal server error during update");
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            bool deleted = _dbRepo.Delete(id);

            if (!deleted)
            {
                return NotFound("User with the requested ID was not found in the database.");
            }
            return NoContent();
        }

        catch (Exception ex)
        {
            Console.WriteLine($"Error during user deletion {id}: " + ex.Message);
            return StatusCode(500, "Internal server error during deletion");
        }
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
