using MovieApp.Models;
using System.Text.Json;

namespace MovieApp.Services
{
    public class TmdbService(IHttpClientFactory httpClientFactory, IConfiguration configuration) : ITmdbService
    {
        private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
        private readonly string? _apiKey = configuration["TmdbSettings:ApiKey"];
        private readonly string? _bearerToken = configuration["TmdbSettings:BearerToken"];
        private const string ApiBaseUrl = "https://api.themoviedb.org/3";
        private const string DefaultLanguage = "en-US";
        private const int DefaultPage = 1;

        private async Task<string> GetApiResponseAsync(string endpoint)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{ApiBaseUrl}{endpoint}"),
                Headers =
                {
                    { "Accept", "application/json" },
                    { "Authorization", $"Bearer {_bearerToken}" }
                },
            };

            try
            {
                using (var response = await _httpClient.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("An error occurred while calling the TMDB API.", ex);
            }
        }

        private async Task<T> GetApiDataAsync<T>(string endpoint)
        {
            var response = await GetApiResponseAsync(endpoint);
            return JsonSerializer.Deserialize<T>(response);
        }

        public async Task<TmdbMovieList> GetLatestMoviesAsync()
        {
            return await GetApiDataAsync<TmdbMovieList>($"/movie/now_playing?language={DefaultLanguage}&page={DefaultPage}");
        }

        public async Task<TmdbMovieList> GetTopMoviesAsync()
        {
            return await GetApiDataAsync<TmdbMovieList>($"/movie/top_rated?language={DefaultLanguage}&page={DefaultPage}");
        }

        public async Task<TmdbMovieList> SearchMoviesAsync(string genreQuery, string query)
        {
            var genreMovies = !string.IsNullOrEmpty(genreQuery)
                ? await GetApiDataAsync<TmdbMovieList>($"/discover/movie?with_genres={genreQuery}&language={DefaultLanguage}&page={DefaultPage}")
                : new TmdbMovieList { Results = new List<TmdbMovie>() };

            var titleMovies = !string.IsNullOrEmpty(query)
                ? await GetApiDataAsync<TmdbMovieList>($"/search/movie?query={query}&language={DefaultLanguage}&page={DefaultPage}")
                : new TmdbMovieList { Results = new List<TmdbMovie>() };

            var searchedMovies = FilterMovies(genreMovies.Results, titleMovies.Results);

            return new TmdbMovieList
            {
                Results = searchedMovies,
                TotalResults = searchedMovies.Count,
                Page = DefaultPage
            };
        }

        private static List<TmdbMovie> FilterMovies(List<TmdbMovie> genreMovies, List<TmdbMovie> titleMovies)
        {
            if (genreMovies.Any() && titleMovies.Any())
            {
                return genreMovies
                    .Where(genreMovie => titleMovies.Any(titleMovie => titleMovie.Id == genreMovie.Id))
                    .ToList();
            }

            return genreMovies.Any() ? genreMovies : titleMovies;
        }

        public async Task<TmdbMovieDetails> GetMovieDetailsAsync(int movieId)
        {
            return await GetApiDataAsync<TmdbMovieDetails>($"/movie/{movieId}?language={DefaultLanguage}");
        }

        public async Task<List<TmdbGenre>> GetGenresAsync()
        {
            var genreList = await GetApiDataAsync<TmdbGenreList>($"/genre/movie/list?language={DefaultLanguage}");
            return genreList?.Genres ?? [];
        }

        public async Task<TmdbMovieCredits> GetMovieCreditsAsync(int movieId)
        {
            return await GetApiDataAsync<TmdbMovieCredits>($"/movie/{movieId}/credits?language={DefaultLanguage}");
        }

        public async Task<TmdbMovieImages> GetMovieImagesAsync(int movieId)
        {
            return await GetApiDataAsync<TmdbMovieImages>($"/movie/{movieId}/images?include_image_language=en");
        }
    }
}
