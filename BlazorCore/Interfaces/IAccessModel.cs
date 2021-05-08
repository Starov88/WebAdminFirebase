namespace BlazorCore.Interfaces
{
    public interface IAccessModel
    {
        string GetAuthToken();
        string GetRefreshToken();
    }
}
