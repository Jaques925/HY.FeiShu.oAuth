namespace HY.FeiShu.oAuth.Model;

public class Access_Token_Input
{
    /// <summary>
    /// 跳转回来的code
    /// </summary>
    public string code { get; set; }
    /// <summary>
    /// 回调地址
    /// </summary>
    public string redirect_uri { get; set; }
}
