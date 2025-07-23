namespace ApartmentWeb.Models;

public class SubmitResponse
{
    public bool isSuccess { get; set; }

    public string redirectUrl { get; set; }

    public bool hasValidationErrors { get; set; }
}