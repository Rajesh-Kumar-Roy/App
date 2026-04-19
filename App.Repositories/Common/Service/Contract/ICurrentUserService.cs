namespace App.Repositories.Common.Service.Contract
{
    public interface ICurrentUserService
    {
        long? GetCurrentUserId();
        string? GetCurrentUserName();
        string? GetCurrentUserEmail();
        List<string> GetCurrentUserRoles();
        bool IsInRole(string role);
    }
}
