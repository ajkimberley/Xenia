namespace ScreenMedia.Xenia.WebApi.Dtos;

public record GetRoomsQueryParameters(DateTime From, DateTime To)
{
    public bool IsValid() => From <= To;
}
