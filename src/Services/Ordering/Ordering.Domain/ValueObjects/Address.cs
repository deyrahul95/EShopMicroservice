using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.ValueObjects;

public record Address
{
    [MaxLength(50)]
    [Required]
    public string FirstName { get; } = string.Empty;
    [MaxLength(50)]
    [Required]
    public string LastName { get; } = string.Empty;
    [MaxLength(50)]
    public string EmailAddress { get; } = string.Empty;
    [MaxLength(180)]
    [Required]
    public string AddressLine { get; } = string.Empty;
    [MaxLength(50)]
    public string Country { get; } = string.Empty;
    [MaxLength(50)]
    public string State { get; } = string.Empty;
    [MaxLength(5)]
    public string ZipCode { get; } = string.Empty;

    protected Address()
    {

    }

    private Address(
        string first,
        string last,
        string email,
        string addressLine,
        string country,
        string state,
        string zip)
    {
        FirstName = first;
        LastName = last;
        EmailAddress = email;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zip;
    }

    public static Address Of(
        string firstName,
        string lastName,
        string emailAddress,
        string addressLine,
        string country,
        string state,
        string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddress);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(argument: state);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);

        return new Address(
            first: firstName,
            last: lastName,
            email: emailAddress,
            addressLine: addressLine,
            country: country,
            state: state,
            zip: zipCode);
    }
}