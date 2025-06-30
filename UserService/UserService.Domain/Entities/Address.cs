using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class Address
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public int PostalCode { get; private set; }

    public Address() { }

    private Address(string country, string street, string city, string state, int postalCode)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Country = country;
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }

    public static Address Create(string country, string street, string city, string state, int postalCode)
    {
        return new Address(country, street, city, state, postalCode);
    }

    public void Update(string country, string street, string city, string state, int postalCode)
    {
        Country = country;
        Street = street;
        City = city;
        State = state;
        PostalCode = postalCode;
    }
}