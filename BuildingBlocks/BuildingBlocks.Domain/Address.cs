namespace JobTracker.SharedKernel.Domain;

public sealed class Address
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

    public sealed class Builder
    {
        internal string Country { get; private set; } = string.Empty;
        internal string State { get; private set; } = string.Empty;
        internal string City { get; private set; } = string.Empty;
        internal string Street { get; private set; } = string.Empty;
        internal int PostalCode { get; private set; }

        public Builder SetCountry(string country) { Country = country?.Trim() ?? string.Empty; return this; }
        public Builder SetState(string state) { State = state?.Trim() ?? string.Empty; return this; }
        public Builder SetCity(string city) { City = city?.Trim() ?? string.Empty; return this; }
        public Builder SetStreet(string street) { Street = street?.Trim() ?? string.Empty; return this; }
        public Builder SetPostalCode(int postalCode) { PostalCode = postalCode; return this; }

        public Address Build()
        {
            if (string.IsNullOrWhiteSpace(Country))
                throw new ArgumentException("Country is required.", nameof(Country));
            if (PostalCode <= 0)
                throw new ArgumentOutOfRangeException(nameof(PostalCode), "Postal code must be positive.");
            return new Address(this);
        }
    }
}
