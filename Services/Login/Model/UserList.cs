using System.ComponentModel.DataAnnotations;

public class UserList

{
    [Key]
    public int UserID { get; set; }
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailOrPhone { get; set; }
    public string Password { get; set; }
}
