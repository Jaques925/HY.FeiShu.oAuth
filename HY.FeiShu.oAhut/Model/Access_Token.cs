namespace HY.FeiShu.oAuth.Model;

public class Access_Token
{
    /// <summary>
    /// 错误的时候输出内容，正确时候为null
    /// </summary>
    public string error { get; set; }
    /// <summary>
    /// 错误的时候输出内容，正确时候为null
    /// </summary>
    public string error_description { get; set; }
    public string access_token { get; set; }
    public string token_type { get; set; }
    public int expires_in { get; set; }
    public string refresh_token { get; set; }
    public int refresh_expires_in { get; set; }
}
