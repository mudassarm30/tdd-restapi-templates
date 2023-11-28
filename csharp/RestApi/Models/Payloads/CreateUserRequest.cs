namespace RestApi.Models.Payloads;
public class CreateUserRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
}