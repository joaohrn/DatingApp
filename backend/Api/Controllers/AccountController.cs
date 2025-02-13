using System;
using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using Api.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper): BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
        if(await UserExists(registerDto.UserName)) return BadRequest("Username taken");
            using var hmac = new HMACSHA512();
            var user = mapper.Map<AppUser>(registerDto);

            user.UserName = registerDto.UserName.ToLower();
            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;
            context.Add(user);
            await context.SaveChangesAsync();
            return Ok(new UserDto{
                UserName = user.UserName,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender,
                KnownAs = user.KnownAs
            });
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user = await context.Users
            .Include(p => p.Photos)
            .FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
        if(user == null) return Unauthorized("invalid username");
        var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if(user.PasswordHash[i] != ComputeHash[i]) return Unauthorized("invalid password");
        }
        return Ok(new UserDto{
            UserName = loginDto.UserName,
            Token = tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        });
    }
    public async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
