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
        string url = $"https://api.themoviedb.org/3/search/movie?query={FilmName.Text}&api_key=&include_adult=false&language=en-US&page=1";
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
        for (int i = MainGrid.Children.Count - 1; i >= 0; i--)
        {
            var child = MainGrid.Children[i];
            if (MainGrid.GetRow(child) > 0) // Remove apenas os elementos da lista de filmes (mantém a entrada e o botão)
            {
                MainGrid.Children.RemoveAt(i);
            }
        }
        var row = 1;
        var column = 0;

        list.results = list.results.OrderByDescending(m => m.popularity).ToList();

        foreach (Movie movie in list.results)
        {

            var imageUri = $"https://image.tmdb.org/t/p/w500{movie.poster_path}";


            var filmGrid = new Grid
            {
                RowDefinitions = new RowDefinitionCollection
                {
                    new RowDefinition { Height = new GridLength(400) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions = new ColumnDefinitionCollection
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                }
            };

            var image = new Image
            {
                WidthRequest = 200,
                HeightRequest = 400,
                Source = new UriImageSource
                {
                    Uri = new Uri(imageUri),
                    CachingEnabled = true,
                    CacheValidity = TimeSpan.FromDays(1)
                }
            };

            var title = new Label
            {
                Text = movie.title,
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                HorizontalOptions = LayoutOptions.Center
            };

            var score = new Label
            {
                Text = $"Score: {movie.vote_average}",
                FontSize = 15,
                HorizontalOptions = LayoutOptions.Center
            };

            Grid.SetRow(image, 0);
            Grid.SetColumn(image, 0);
            filmGrid.Children.Add(image);

            Grid.SetRow(title, 1);
            Grid.SetColumn(title, 0);
            filmGrid.Children.Add(title);

            Grid.SetRow(score, 2);
            Grid.SetColumn(score, 0);
            filmGrid.Children.Add(score);

            Grid.SetRow(filmGrid, row);
            Grid.SetColumn(filmGrid, column);
            MainGrid.Children.Add(filmGrid);

            // Atualiza a posição para a próxima célula
            column++;
            if (column >= 3) // 3 colunas
            {
                column = 0;
                row++;
            }

            // Para se não houver mais linhas disponíveis
            if (row >= 4) // 4 linhas no total
                break;
        }
    }
}
