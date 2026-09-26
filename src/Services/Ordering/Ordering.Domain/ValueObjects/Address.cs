namespace Ordering.Domain.ValueObjects;

public record Address
{
    public string FirstName { get; } = string.Empty;
    public string LastName { get; } = string.Empty;
    public string EmailAddress { get; } = string.Empty;
    public string AddressLine { get; } = string.Empty;
    public string Country { get; } = string.Empty;
    public string State { get; } = string.Empty;
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
        ArgumentException.ThrowIfNullOrWhiteSpace(state);
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