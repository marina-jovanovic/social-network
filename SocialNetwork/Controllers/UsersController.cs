using Microsoft.AspNetCore.Mvc;
using SocialNetwork.Models;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly UserRepository _repo;

    public UsersController()
    {
        _repo = new UserRepository();
        _repo.Load();
    }

    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_repo.Users);
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
}
