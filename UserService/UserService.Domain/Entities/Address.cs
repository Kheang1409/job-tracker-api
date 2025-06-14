using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobTracker.UserService.Domain.Entities;

public class Address
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; private set; } = string.Empty;
    public string Address1 { get; private set; } = string.Empty;
    public string Address2 { get; private set; } = string.Empty;
    public int PostalCode { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string County { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    public Address() { }

    private Address(string address1, string address2, int postalCode, string city, string county, string state, string country)
    {
        Id = ObjectId.GenerateNewId().ToString();
        Address1 = address1;
        Address2 = address2;
        PostalCode = postalCode;
        City = city;
        County = county;
        State = state;
        Country = country;
    }

    public static Address Create(string address1, string address2, int postalCode, string city, string county, string state, string country)
    {
        return new Address(address1, address2, postalCode, city, county, state, country);
    }

    public void Update(string address1, string address2, int postalCode, string city, string county, string state, string country)
    {
        Address1 = address1;
        Address2 = address2;
        PostalCode = postalCode;
        City = city;
        County = county;
        State = state;
        Country = country;
    }
}