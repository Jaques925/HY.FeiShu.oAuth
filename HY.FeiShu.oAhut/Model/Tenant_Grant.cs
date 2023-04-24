namespace HY.FeiShu.oAuth.Model;

public class Tenant_Grant
{
    public string grant_type { get; set; } = "authorization_code";
    public string code { get; set; }
}
