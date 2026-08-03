namespace StructureExplorer.Services
{
    public interface IJsonFetcherService
    {
        Task<string> FetchFromUrlAsync(string url);
    }
}