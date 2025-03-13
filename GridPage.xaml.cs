using Aula2Rogerio.Models;
using Newtonsoft.Json;

namespace Aula2Rogerio;

public partial class GridPage : ContentPage
{
    public GridPage()
    {
        InitializeComponent();
    }

    public async void OnSubmit(object sender, EventArgs e)
    {
        string url = $"https://api.themoviedb.org/3/search/movie?query={FilmName}&api_key=89cca6eda8aa59f4040823111ab90dab&include_adult=false&language=en-US&page=1";
        var result = await FetchTMDBAPI(url);
        ShowFilms(result);
    }

    public async Task<MovieList> FetchTMDBAPI(string url)
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<MovieList>(json);
        }
    }

    public void ShowFilms(MovieList list)
    {
        foreach (Movie movie in list.results)
        {
            FilmsStack.Children.Add(
                new Label
                {
                    Text = movie.title
                });
        }
    }
}