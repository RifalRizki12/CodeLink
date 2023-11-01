using API.DTOs.Accounts;
using API.DTOs.Employees;
using API.DTOs.Tokens;
using API.Models;
using API.Utilities.Handler;
using CLIENT.Models;


namespace CLIENT.Contract
{
    public interface IAccountRepository : IRepository<AccountDto, Guid>
    {
        Task<ResponseOKHandler<ClaimsDto>> GetClaimsAsync(string token);
        Task<ResponseOKHandler<TokenDto>> Login(LoginDto login);
        Task<ResponseOKHandler<ForgotPasswordDto>> ForgotPassword(string email, ForgotPasswordDto forgotDto);
        Task<ResponseOKHandler<ChangePasswordDto>> ChangePassword(string email, ChangePasswordDto changePsswdDto);
        
    }
}
