namespace JobTracker.JobService.Domain.Entities;

public class Location
{
    public string Street { get; private set; } = string.Empty;
    public int PostalCode { get; private set; }
    public string City { get; private set; } = string.Empty;
    public string County { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    
    public Location() { }

    private Location(string street, int postalCode, string city, string county, string state, string country)
    {
        Street = street;
        PostalCode = postalCode;
        City = city;
        County = county;
        State = state;
        Country = country;
    }

    public static Location Create(string street,int postalCode, string city, string county, string state, string country)
    {
        return new Location(street, postalCode, city, county, state, country);
    }

    public void Update(string street, int postalCode, string city, string county, string state, string country)
    {
        Street = street;
        PostalCode = postalCode;
        City = city;
        County = county;
        State = state;
        Country = country;
    }
}