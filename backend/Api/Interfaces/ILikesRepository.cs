using System;
using Api.DTOs;
using Api.Entities;
using Api.Helpers;

namespace Api.Interfaces;

public interface ILikesRepository
{
    Task<UserLike?> GetUserLike(int sourceId, int targetId);
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);
    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> SaveChanges();

}
