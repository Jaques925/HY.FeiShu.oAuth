using HY.FeiShu.oAuth.Model;
using LitJson;
using System.Net.Http.Headers;

namespace HY.FeiShu.oAuth;

public class FSAuth : IFSAuth
{
    /// <summary>
    /// 自建应用获取 tenant_access_token<br>
    /// tenant_access_token用户在飞书app内，免登录到自已的应用上。</br>
    /// tenant_access_token 的最大有效期是 2 小时。如果在有效期小于 30 分钟的情况下，调用本接口，会返回一个新的 tenant_access_token，这会同时存在两个有效的 tenant_access_token<br>
    /// 文档说明：https://open.feishu.cn/document/ukTMukTMukTM/ukDNz4SO0MjL5QzM/auth-v3/auth/tenant_access_token_internal</br>
    /// </summary>
    private const string tenant_access_token_url = "https://open.feishu.cn/open-apis/auth/v3/tenant_access_token/internal";
    /// <summary>
    /// 获取 user_access_token<br>
    /// 通过tenant_access_token获得user_access_token,用户在飞书app内，免登录到自已的应用上</br>
    /// 本接口用于网页应用免登录应用场景，小程序应用获取 user_access_token 的方法，请参考小程序应用提供的 code2session 接口。<br>
    /// 文档说明：https://open.feishu.cn/document/uAjLw4CM/ukTMukTMukTM/reference/authen-v1/access_token/create</br>
    /// </summary>
    private const string tenant_user_access_token_url = "https://open.feishu.cn/open-apis/authen/v1/access_token";

    /// <summary>
    /// 刷新 user_access_token<br>
    /// user_access_token 的最大有效期是 6900 秒。当 user_access_token 过期时，可以调用本接口获取新的 user_access_token</br>
    /// 文档说明：https://open.feishu.cn/document/uAjLw4CM/ukTMukTMukTM/reference/authen-v1/refresh_access_token/create<br>
    /// </summary>
    private const string tenant_refresh_user_access_token_url = "https://open.feishu.cn/open-apis/authen/v1/refresh_access_token";


    /// <summary>
    /// 获取 access_token<br>
    /// 开发者网页前端或客户端 获取 code 之后，需要把 code 传递给开发者的服务器，然后通过开发者服务器调用飞书服务器来获取可用于访问用户信息的 access_token。</br>
    /// 此access_token是用于，使用飞书帐号登录到应用程序上。<br>
    /// 文档说明：https://open.feishu.cn/document/common-capabilities/sso/api/get-access_token</br>
    /// </summary>
    private const string access_token_url = "https://passport.feishu.cn/suite/passport/oauth/token";


    /// <summary>
    /// 获取登录用户信息<br>
    /// 通过 access_token 获取登录用户的信息</br>
    /// 使用飞书帐号登录到应用程序上。<br>
    /// 此接口需要申请权限</br>
    /// 文档说明：https://open.feishu.cn/document/common-capabilities/sso/api/get-user-info<br></br>
    /// </summary>
    private const string get_user_info_url = "https://passport.feishu.cn/suite/passport/oauth/userinfo";


    /// <summary>
    /// 刷新已过期的 access_token<br>
    /// access_token 过期后，开发者需要使用 refresh_token 重新获取一个有效的 access_token。开发者服务器需要严格保证 refresh_token 的安全，并禁止把 refresh_token 传递给网页前端或客户端</br>
    /// 文档说明：https://open.feishu.cn/document/common-capabilities/sso/api/refresh-access_token<br></br>
    /// </summary>
    private const string refresh_access_token = "https://passport.feishu.cn/suite/passport/oauth/token";

    private readonly string _appid;
    private readonly string _secret;
    private Tenant_Access_Token _tenant_access_token;
    private Tenant_User_Access_Token _tenant_user_access_token;

    private Access_Token _access_token;
    public FSAuth(string appid, string secret)
    {
        _appid = appid;
        _secret = secret;
    }

    public async Task<Tenant_Access_Token> TenantAccessToken()
    {
        if (_tenant_access_token != null) { return _tenant_access_token; }
        AppId_Secret appId_Secret = new() { app_id = _appid, app_secret = _secret };
        _tenant_access_token = JsonMapper.ToObject<Tenant_Access_Token>(await PostJson(tenant_access_token_url, JsonMapper.ToJson(appId_Secret)));
        return _tenant_access_token;
    }

    public async Task<Tenant_User_Access_Token> TenantUserAccessToken(Tenant_Access_Token_Code tenant_Access_Token_Code)
    {
        Tenant_Grant_Code tenant_Grant_Code = new() { code = tenant_Access_Token_Code.code };
        tenant_Access_Token_Code.tenant_access_token ??= tenant_Access_Token_Code.tenant_access_token = TenantAccessToken().Result.tenant_access_token;
        _tenant_user_access_token = JsonMapper.ToObject<Tenant_User_Access_Token>(await PostJson(tenant_user_access_token_url, JsonMapper.ToJson(tenant_Grant_Code), tenant_Access_Token_Code.tenant_access_token));
        return _tenant_user_access_token;
    }


    public async Task<Tenant_User_Access_Token> RefreshTenantUserAccessToken(Tenant_Access_Token_Code tenant_Access_Token_Code)
    {
        Tenant_Grant_Refresh_Code tenant_Grant_Refresh_Code = new() { grant_type= "refresh_token", refresh_token = tenant_Access_Token_Code.refresh_token };
        tenant_Access_Token_Code.tenant_access_token ??= tenant_Access_Token_Code.tenant_access_token = TenantAccessToken().Result.tenant_access_token;
        _tenant_user_access_token = JsonMapper.ToObject<Tenant_User_Access_Token>(await PostJson(tenant_refresh_user_access_token_url, JsonMapper.ToJson(tenant_Grant_Refresh_Code), tenant_Access_Token_Code.tenant_access_token));
        return _tenant_user_access_token;
    }

    private async Task<string> PostJson(string url,string postdata, string authcontent = "")
    {
        HttpClient httpClient = new();


        HttpContent httpContent = new StringContent(postdata);

        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json")
        {
            CharSet = "utf-8"
        };

        if (authcontent != "")
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authcontent);
        }


        var response = await httpClient.PostAsync(url, httpContent);

        var responseBody = await response.Content.ReadAsStringAsync();

        httpClient.Dispose();
        return responseBody;

    }

    public async Task<Access_Token> AccessToken(Access_Token_Input access_Token_Input)
    {

        _access_token ??= JsonMapper.ToObject<Access_Token>(await PostData(access_Token_Input));
        return _access_token;
    }

    public async Task<Access_Token_User> UserAccessToken(Access_Token_Input access_Token_Input)
    {
        _access_token ??= JsonMapper.ToObject<Access_Token>(await PostData(access_Token_Input));
        return JsonMapper.ToObject<Access_Token_User>(await GetUser(get_user_info_url, _access_token.access_token));
    }

    public async Task<Access_Token_User> UserAccessToken()
    {
        if (_access_token == null) { return new Access_Token_User(); }
        return JsonMapper.ToObject<Access_Token_User>(await GetUser(get_user_info_url, _access_token.access_token));
    }

    public async Task<Access_Token> RefreshAccessToken(string refreshtoken) {
        var parm = new[] {
            new KeyValuePair<string, string>("grant_type", "refresh_token"),
            new KeyValuePair<string, string>("refresh_token", refreshtoken)
        };
        _access_token = JsonMapper.ToObject<Access_Token>(await PostData(new Access_Token_Input(), parm));
        return _access_token;
    }
        
    private async Task<string> PostData(Access_Token_Input access_Token_Input, KeyValuePair<string, string>[] parm = null)
    {
        parm ??= new[] {
            new KeyValuePair<string, string>("grant_type", "authorization_code"),
            new KeyValuePair<string, string>("client_id", _appid),
            new KeyValuePair<string, string>("client_secret", _secret),
            new KeyValuePair<string, string>("code", access_Token_Input.code),
            new KeyValuePair<string, string>("redirect_uri", access_Token_Input.redirect_uri)
        };

        HttpClient client = new();

        HttpRequestMessage request = new (HttpMethod.Post, access_token_url)
        {
            Content = new FormUrlEncodedContent(parm)
        };
        var result = await client.SendAsync(request);

        return await result.Content.ReadAsStringAsync();
    }

    private async Task<string> GetUser(string url, string authcontent = "")
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("ContentType", "application/json;charset=utf-8");
        if (authcontent != "")
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authcontent);
        }

        var response = httpClient.GetAsync(url).Result;

        string responseBody = await response.Content.ReadAsStringAsync();

        return responseBody;

    }
}