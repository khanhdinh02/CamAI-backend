using Core.Domain.DTO;

namespace Host.CamAI.API.Models;

public class CreateBrandWithImageDto
{
    public CreateBrandDto Brand { get; set; } = null!;

    public ControllerCreateImageDto? Logo { get; set; }

    public ControllerCreateImageDto? Banner { get; set; }
}
