namespace ScreenMedia.Xenia.HotelManagement.Domain;

// TODO:
// 1. Add more properties
// 2. Substitute or supplement GUID for user-friendly Id
// 3. Validation
public class Hotel
{
    private Hotel(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public static Hotel Create(string name) => new(Guid.NewGuid(), name);
}
