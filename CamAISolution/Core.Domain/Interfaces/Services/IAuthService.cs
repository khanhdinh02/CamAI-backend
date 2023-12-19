using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domain.Models.dtos.auth;

namespace Core.Domain.Interfaces.Services;
public interface IAuthService
{
    Task<TokenResponseDTO> getTokensByUsernameAndPassword(string username, string password);
    public Guid test();
}
