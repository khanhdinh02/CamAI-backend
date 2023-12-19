using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models.dtos.auth;
public class TokenResponseDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public string? RefreshToken { get; set; }

}
