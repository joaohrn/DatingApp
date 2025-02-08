using System.Security.Claims;
using Api.DTOs;
using Api.Entities;
using Api.Extensions;
using Api.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
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
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if(user == null) return BadRequest("Could not find user");
        mapper.Map(memberUpdateDto, user);
        if(await userRepository.SaveChangesAsync()) return NoContent();
        return BadRequest("Failed to update user");
    }
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if(user == null) return BadRequest("Could not get user.");

        var result = await photoService.AddPhotoAsync(file);

        if(result.Error != null) return BadRequest(result.Error.Message);

        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        if(user.Photos.Count == 0) photo.IsMain = true;
        user.Photos.Add(photo);
        if(await userRepository.SaveChangesAsync())
            return CreatedAtAction(nameof(GetUser), new {username = user.UserName}, mapper.Map<PhotoDto>(photo));
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());
        if(user == null) return BadRequest("Coud not find user");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if(photo == null || photo.IsMain) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if(currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if(await userRepository.SaveChangesAsync()) return NoContent();

        return BadRequest("Couldn't set main photo");
    }
    [HttpDelete("delete-photo/{photoid:int}")]
    public async Task<ActionResult> DeletePhoto(int photoid)
    {
        var user = await userRepository.GetUserByUserNameAsync(User.GetUsername());

        if(user == null) return BadRequest("Could not get user");

        var photo = user.Photos.FirstOrDefault((x) => x.Id == photoid);
        if(photo == null || photo.IsMain) return BadRequest("This photo could not be deleted");
        if(photo.PublicId != null) 
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error != null) return BadRequest(result.Error.Message);
        }
        user.Photos.Remove(photo);
        if(await userRepository.SaveChangesAsync()) return Ok();
        return BadRequest("Problem deleting photo");
    }
}
