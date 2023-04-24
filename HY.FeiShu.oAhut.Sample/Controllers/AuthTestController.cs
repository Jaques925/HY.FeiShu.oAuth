using Microsoft.AspNetCore.Mvc;
using HY.FeiShu.oAuth;
using HY.FeiShu.oAuth.Model;

namespace HY.FeiShu.oAhut.Sample.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthTestController : ControllerBase
{
 

    private readonly ILogger<AuthTestController> _logger;
    private readonly IFSAuth _fsauth;
    public AuthTestController(ILogger<AuthTestController> logger, IFSAuth fsauth)
    {
        _logger = logger;
        _fsauth = fsauth;
    }

    [HttpGet("Tenant_Access_Token")]
    public async Task<Tenant_Access_Token> Get()
    {
        return await _fsauth.TenantAccessToken();
    }



    [HttpGet("Tenant_User_Access_Token")]
    public async Task<Tenant_User_Access_Token> Get1(string code)
    {
        Tenant_Access_Token_Code tenant_Access_Token_Code = new() { code = code };
        return await _fsauth.TenantUserAccessToken(tenant_Access_Token_Code);
    }

    [HttpGet("Tenant_Refresh_User_Access_Token")]
    public async Task<Tenant_User_Access_Token> Get2(string refresh_token)
    {
        Tenant_Access_Token_Code tenant_Access_Token_Code = new() { refresh_token = refresh_token };
        return await _fsauth.RefreshTenantUserAccessToken(tenant_Access_Token_Code);
    }


    [HttpPost("Access_Token")]
    public async Task<Access_Token> Get3(Access_Token_Input access_Token_Input)
    {
        return await _fsauth.AccessToken(access_Token_Input);
    }


    [HttpPost("Access_Token_User")]
    public async Task<Access_Token_User> Get4(Access_Token_Input access_Token_Input)
    {
        return await _fsauth.UserAccessToken(access_Token_Input);
    }

    [HttpGet("Access_Token_User_None_Input")]
    public async Task<Access_Token_User> Get5()
    {
        return await _fsauth.UserAccessToken();
    }

    [HttpGet("Refresh_Access_Token")]
    public async Task<Access_Token> Get6(string refreshtoken)
    {
        return await _fsauth.RefreshAccessToken(refreshtoken);
    }


}