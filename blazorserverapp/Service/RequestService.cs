using System.Net.Http.Headers;
using System.Text;
namespace blazorserverapp.Service
{
    public class RequestService<T>
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string SHEME = "http";
        private const string HOST = "127.0.0.1";
        private const int PORT = 5015;
        private UriBuilder _uriBuilder;
        public RequestService (IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _uriBuilder = new(SHEME,HOST,PORT);

        }

        public async Task<T> Get(string resourcePath, Dictionary<string, string>? parameters)
        {
            
            _uriBuilder.Path = $"/api{resourcePath}";
            _uriBuilder.Query = BuildQueryParameters(parameters);
            
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; charset=UTF-8"));
            var response = await httpClient.GetFromJsonAsync<T>(_uriBuilder.Uri);
            return response;
        }
        public async Task<List<T>> GetMany(string resourcePath, Dictionary<string, string>? parameters)
        {
            
            _uriBuilder.Path = $"/api{resourcePath}";
            _uriBuilder.Query = BuildQueryParameters(parameters);
            
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; charset=UTF-8"));
            var response = await httpClient.GetFromJsonAsync<List<T>>(_uriBuilder.Uri);
            // T item = default;
            // if (response.IsSuccessStatusCode)
            // {
            //     var test1 = response.con
            //     return await response.Content.ReadFromJsonAsync<T>();
            // }
            // return item;
            return response;
        }
        public async Task Save<T>(string resourcePath, T resource)
        {
            
            _uriBuilder.Path = $"/api{resourcePath}";
            
            
            var httpClient = _httpClientFactory.CreateClient();
            httpClient.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json; charset=UTF-8"));
            var response = await httpClient.PostAsJsonAsync<T>(_uriBuilder.Uri,resource);
        }
        private string BuildQueryParameters(Dictionary<string, string> parameters)
        {
            string qp = "";
            if(parameters != null && parameters.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append('?');
                sb.AppendJoin("&", parameters.Select(p => $"{p.Key}={p.Value}"));
                qp = sb.ToString();
            }
            return qp;
            
        }
    }
}