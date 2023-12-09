namespace ScreenMedia.Xenia.WebApi.Errors;

public class NoVacanciesError : Error
{
    public NoVacanciesError() : base(id: Guid.NewGuid()) { }
    public NoVacanciesError(string message) : base(id: Guid.NewGuid(), message: message) { }
}