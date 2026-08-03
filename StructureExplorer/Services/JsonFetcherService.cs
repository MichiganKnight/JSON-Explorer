namespace StructureExplorer.Services
{
    public class JsonFetcherService : IJsonFetcherService
    {
        private readonly HttpClient _httpClient;
        
        public JsonFetcherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        
        public async Task<string> FetchFromUrlAsync(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            return await response.Content.ReadAsStringAsync();
        }
    }
}