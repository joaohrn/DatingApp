using System;
using System.Security.Cryptography;
using System.Text;
using Api.Data;
using Api.DTOs;
using Api.Entities;
using Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class AccountController(DataContext context, ITokenService tokenService): BaseApiController
{
    [HttpPost("register")]
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto){
        if(await UserExists(registerDto.UserName)) return BadRequest("Username taken");
        //using var hmac = new HMACSHA512();
        //var user = new AppUser{
        //    UserName = registerDto.UserName.ToLower(),
        //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
        //    PasswordSalt = hmac.Key
        //};
        //context.Add(user);
        //await context.SaveChangesAsync();
        //return Ok(new UserDto{
        //    UserName = registerDto.UserName,
        //    Token = tokenService.CreateToken(user)
        //});
        return Ok();
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto){
        var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName.ToLower());
        if(user == null) return Unauthorized("invalid username");
        var hmac = new HMACSHA512(user.PasswordSalt);
        var ComputeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        for (int i = 0; i < ComputeHash.Length; i++)
        {
            if(user.PasswordHash[i] != ComputeHash[i]) return Unauthorized("invalid password");
        }
        return Ok(new UserDto{
            UserName = loginDto.UserName,
            Token = tokenService.CreateToken(user)
        });
    }
    public async Task<bool> UserExists(string username){
        return await context.Users.AnyAsync(x => x.UserName.ToLower() == username.ToLower());
    }
}
