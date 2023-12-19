using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domain.Models.dtos.auth;
public class LoginDTO
{
    [Required]
    public  string Username { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;

}
