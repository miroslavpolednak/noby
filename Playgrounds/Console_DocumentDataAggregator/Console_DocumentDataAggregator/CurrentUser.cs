namespace Console_CustomerService;

public class CurrentUser : CIS.Core.Security.ICurrentUser
{
    public int Id => 500;
    public string Login => "test";
}