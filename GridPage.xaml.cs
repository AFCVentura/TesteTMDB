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
        string url = $"https://api.themoviedb.org/3/search/movie?query={FilmName.Text}&api_key=89cca6eda8aa59f4040823111ab90dab&include_adult=false&language=en-US&page=1";
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
        var row = 1;
        var column = 0;

        foreach (Movie movie in list.results)
        {
            var label = new Label
            {
                Text = movie.title
            };

            Grid.SetRow(label, row);
            Grid.SetColumn(label, column);
            MainGrid.Children.Add(label);

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