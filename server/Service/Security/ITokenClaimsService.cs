namespace Service.Interfaces;

public interface ITokenClaimsService
{
    Task<string> GetTokenAsync(string userName);
}