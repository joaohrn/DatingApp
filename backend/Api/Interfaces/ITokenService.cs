using System;
using Api.Entities;

namespace Api.Interfaces;

public interface ITokenService
{
    public string CreateToken(AppUser user);
}
