using System.Security.Claims;
using Api.DTOs;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users = await userRepository.GetMembersAsync();
        return Ok(users);
    }
    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        var user = await userRepository.GetMemberAsync(username);
        if(user == null) return NoContent();
        
        return Ok(user);
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if(username == null) return BadRequest("No user found in token");
        var user = await userRepository.GetUserByUserNameAsync(username);
        if(user == null) return BadRequest("Could not find user");
        mapper.Map(memberUpdateDto, user);
        if(await userRepository.SaveChangesAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }

}
