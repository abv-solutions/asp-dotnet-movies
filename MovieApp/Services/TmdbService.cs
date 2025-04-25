using MovieApp.Models;
using System.Text.Json;


namespace MovieApp.Services
{
    public class TmdbService : ITmdbService
    {
        private readonly HttpClient _httpClient;
        private const string ApiKey = "47fe63537131d54145217ec3280654b2"; // Replace with your API key
        private const string BearerToken = "eyJhbGciOiJIUzI1NiJ9.eyJhdWQiOiI0N2ZlNjM1MzcxMzFkNTQxNDUyMTdlYzMyODA2NTRiMiIsIm5iZiI6MTc0NTU2NTMzOC44NjA5OTk4LCJzdWIiOiI2ODBiMzY5YTI3NmJmNjRlNDFhYmQzNTQiLCJzY29wZXMiOlsiYXBpX3JlYWQiXSwidmVyc2lvbiI6MX0.WwvN2HDxfiXNU7styvFSO5RxLMpk7tITYxD1ryHAI9w"; // Replace with your bearer token

        public TmdbService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<string> GetApiResponseAsync(string endpoint)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://api.themoviedb.org/3{endpoint}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {BearerToken}" }
                },
            };

            using (var response = await _httpClient.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<TmdbMovieList> GetLatestMoviesAsync()
        {
            var response = await GetApiResponseAsync("/movie/now_playing?language=en-US&page=1");
            return JsonSerializer.Deserialize<TmdbMovieList>(response);
        }

        public async Task<TmdbMovieList> GetTopMoviesAsync()
        {
            var response = await GetApiResponseAsync("/movie/top_rated?language=en-US&page=1");
            return JsonSerializer.Deserialize<TmdbMovieList>(response);
        }

        public async Task<TmdbMovieList> SearchMoviesAsync(string genreQuery, string query)
        {
            var genreMovies = !string.IsNullOrEmpty(genreQuery)
                ? JsonSerializer.Deserialize<TmdbMovieList>(await GetApiResponseAsync($"/discover/movie?with_genres={genreQuery}&language=en-US&page=1"))
                : new TmdbMovieList { Results = new List<TmdbMovie>() };

            var titleMovies = !string.IsNullOrEmpty(query)
                ? JsonSerializer.Deserialize<TmdbMovieList>(await GetApiResponseAsync($"/search/movie?query={query}&language=en-US&page=1"))
                : new TmdbMovieList { Results = new List<TmdbMovie>() };

            // If both are provided, intersect the results
            var searchedMovies = genreMovies.Results
                .Where(genreMovie => titleMovies.Results.Any(titleMovie => titleMovie.Id == genreMovie.Id))
                .ToList();

            // If only one query is provided, return the appropriate results
            if (string.IsNullOrEmpty(query)) searchedMovies = genreMovies.Results;
            if (string.IsNullOrEmpty(genreQuery)) searchedMovies = titleMovies.Results;

            return new TmdbMovieList
            {
                Results = searchedMovies,
                TotalResults = searchedMovies.Count,
                Page = 1
            };
        }

        public async Task<TmdbMovieDetails> GetMovieDetailsAsync(int movieId)
        {
            var response = await GetApiResponseAsync($"/movie/{movieId}?language=en-US");
            return JsonSerializer.Deserialize<TmdbMovieDetails>(response);
        }

        public async Task<List<TmdbGenre>> GetGenresAsync()
        {
            var response = await GetApiResponseAsync("/genre/movie/list?language=en");
            var genreList = JsonSerializer.Deserialize<TmdbGenreList>(response);
            return genreList?.Genres ?? [];
        }
    }
}
