namespace HY.FeiShu.oAuth.Model;

public class Tenant_User_Access_Token_Data : FeiShu_User_Base
{
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string enterprise_email { get; set; }
    public int refresh_expires_in { get; set; }
    public string refresh_token { get; set; }
    public string sid { get; set; }
}
