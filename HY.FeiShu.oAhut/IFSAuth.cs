using HY.FeiShu.oAuth.Model;

namespace HY.FeiShu.oAuth;

public interface IFSAuth
{
    /// <summary>
    /// 自建应用获取 tenant_access_token<br>
    /// tenant_access_token在有效期自行保存，过期再次获取</br>
    /// </summary>
    /// <returns></returns>
    Task<Tenant_Access_Token> TenantAccessToken();

    /// <summary>
    /// 获取 user_access_token<br>
    /// 如果用户开通了用户个人权限，可以拿到用户email,mobile等隐私数据</br>
    /// 可以直接调用，无须先调用TenantAccessToken获得tenant_access_token
    /// </summary>
    /// <param name="tenant_Access_Token_Code"></param>
    /// <returns></returns>
    Task<Tenant_User_Access_Token> TenantUserAccessToken(Tenant_Access_Token_Code tenant_Access_Token_Code);




    /// <summary>
    /// 刷新 user_access_token<br>
    /// 如果用户开通了用户个人权限，可以拿到用户email,mobile等隐私数据</br>
    /// </summary>
    /// <param name="tenant_Access_Token_Code"></param>
    /// <returns></returns>
    Task<Tenant_User_Access_Token> RefreshTenantUserAccessToken(Tenant_Access_Token_Code tenant_Access_Token_Code);



    /// <summary>
    /// Access_Token_Input<br>
    /// 开发者网页前端或客户端 获取 code 之后，需要把 code 传递给开发者的服务器，然后通过开发者服务器调用飞书服务器来获取可用于访问用户信息的 access_token。access_token 是开发者用户获取用户信息的唯一凭证，开发者服务器需要严格保证 access_token 的安全，并禁止把 access_token 传递给客户端</br>
    /// </summary>
    /// <param name="access_Token_Input"></param>
    /// <returns></returns>
    Task<Access_Token> AccessToken(Access_Token_Input access_Token_Input);
    /// <summary>
    /// 获取用户信息<br>
    /// 开发者获取 access_token 之后，可以通过 access_token 来调用飞书服务器来获取用户信息</br>
    /// </summary>
    /// <param name="access_Token_Input"></param>
    /// <returns></returns>
    Task<Access_Token_User> UserAccessToken(Access_Token_Input access_Token_Input);


    /// <summary>
    /// 获取用户信息<br>
    /// 如果已经获取了AccessToken，直接调用</br>
    /// 开发者获取 access_token 之后，可以通过 access_token 来调用飞书服务器来获取用户信息<br>
    /// </summary>
    /// <returns></returns>
    Task<Access_Token_User> UserAccessToken();
    /// <summary>
    /// 刷新已过期的 access_token
    /// </summary>
    /// <param name="refreshtoken"></param>
    /// <returns></returns>
    Task<Access_Token> RefreshAccessToken(string refreshtoken);

}
