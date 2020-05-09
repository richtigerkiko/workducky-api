using System.ComponentModel.DataAnnotations;
using WorkduckyLib.Enums;
using WorkduckyLib.Interfaces;

namespace WorkduckyLib.DataObjects.RequestObjects
{
    public class UserLoginEmailPasswordMachineRequest : IEmailPasswordRequest
    {
        
        public string Uid { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string MachineId { get; set; }

        [Required]
        public AuthenticationTypes AuthType { get; set; }
    }
}