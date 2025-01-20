using System;
using Api.DTOs;
using Api.Entities;

namespace Api.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id); 
    Task<AppUser?> GetUserByUserNameAsync(string username); 
    Task<IEnumerable<MemberDto>> GetMembersAsync();
    Task<MemberDto?> GetMemberAsync(string username);
}
