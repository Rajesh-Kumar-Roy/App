using App.Managers.IdentityDto;

namespace App.Managers.Contract
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
        Task<bool> LogoutAsync(long userId);
        Task<bool> ChangePasswordAsync(long userId, ChangePasswordDto changePasswordDto);
        Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<UserDto?> GetUserByIdAsync(long userId);
        Task<bool> AssignRoleAsync(long userId, string roleName);
        Task<bool> RemoveRoleAsync(long userId, string roleName);
        Task<List<string>> GetUserRolesAsync(long userId);
    }
}
