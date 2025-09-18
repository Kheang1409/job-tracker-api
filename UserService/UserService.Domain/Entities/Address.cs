namespace JobTracker.UserService.Domain.Entities;

public class Address
{
    public string Country { get; private set; } = string.Empty;
    public string Street { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public int PostalCode { get; private set; }

    private Address(Builder builder)
    {
        Country = builder.Country;
        Street = builder.Street;
        City = builder.City;
        State = builder.State;
        PostalCode = builder.PostalCode;
    }

    public class Builder
    {
        public string Country { get; private set; } = string.Empty;
        public string State { get; private set; } = string.Empty;
        public string City { get; private set; } = string.Empty;
        public string Street { get; private set; } = string.Empty;
        public int PostalCode { get; private set; }

        public Builder SetCountry(string country)
        {
            Country = country;
            return this;
        }

        public Builder SetState(string state)
        {
            State = state;
            return this;
        }

        public Builder SetCity(string city)
        {
            City = city;
            return this;
        }


        public Builder SetStreet(string street)
        {
            Street = street;
            return this;
        }

        public Builder SetPostalCode(int postalCode)
        {
            PostalCode = postalCode;
            return this;
        }

        public Address Build()
        {
            if (string.IsNullOrEmpty(Country))
                throw new ArgumentNullException(nameof(Country), "Country cannot be null or empty.");
            if (PostalCode == 0)
                throw new ArgumentNullException(nameof(PostalCode), "PostalCode cannot be null or empty.");
            return new Address(this);
        }
    }
    public void UpdateAddress(string country, string state, string city, string street, int postalCode)
    {
        Country = country;
        State = state;
        City = city;
        Street = street;
        PostalCode = postalCode;
    }
}