using System;
using Api.DTOs;
using Api.Entities;
using Api.Helpers;

namespace Api.Interfaces;

public interface IUserRepository
{
    void Update(AppUser user);
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();
    Task<AppUser?> GetUserByIdAsync(int id); 
    Task<AppUser?> GetUserByUserNameAsync(string username); 
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto?> GetMemberAsync(string username);
}
