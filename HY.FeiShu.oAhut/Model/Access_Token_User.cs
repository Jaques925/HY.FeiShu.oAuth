namespace HY.FeiShu.oAuth.Model;

public class Access_Token_User : FeiShu_User_Base
{
    public string sub { get; set; }
    public string picture { get; set; }
    public string employee_no { get; set; }
}
