using System.ComponentModel.DataAnnotations;

namespace ZzzLab.Web.Models
{
    public class LoginRequest : RequestBase
    {
        [Required(ErrorMessage = "UserId는 필수값입니다.")]
        [StringLength(50, MinimumLength = 3)]
        public string? UserId { set; get; }

        [Required(ErrorMessage = "Password는 필수값입니다.")]
        [StringLength(200, MinimumLength = 6)]
        public string? Password { set; get; }

        public string? ClientId { set; get; }

        public bool IsValid()
            => (ValidUtils.IsNullOrWhiteSpaceOr(this.UserId, this.Password));
    }
}