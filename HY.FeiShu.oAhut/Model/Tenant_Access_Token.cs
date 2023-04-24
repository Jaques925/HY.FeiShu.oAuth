namespace HY.FeiShu.oAuth.Model;

public class Tenant_Access_Token
{
    public int code { get; set; }
    public string  msg { get; set; }
    public string tenant_access_token { get; set; }
    public int expire { get; set; }
}
