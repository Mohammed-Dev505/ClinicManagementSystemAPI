using ClinicManagement.API.Application.DTOs;
using FluentValidation;
namespace ClinicManagement.API.FluentValidation
{
    public class CreateRoomValidator : AbstractValidator<CreateRoomDto>
    {
        public CreateRoomValidator()
        {
            RuleFor(r => r.RoomNumber).NotEmpty().WithMessage("Room number is required").NotNull();
        }
    }
}
