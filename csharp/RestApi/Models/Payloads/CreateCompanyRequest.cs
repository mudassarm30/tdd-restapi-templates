namespace RestApi.Models.Payloads;
public class CreateCompanyRequest
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Web { get; set; }
    public string? Logo { get; internal set; }
    public string? Description { get; internal set; }
}