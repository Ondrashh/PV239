using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API;
using TVTrack.API.Models;

namespace TVTrack.Mobile.ViewModels
{
    public partial class SearchViewModel: ViewModelBase
    {
        private readonly TVMazeClient _client;

        public ObservableCollection<Search> Results { get; set; } = new ObservableCollection<Search>();

        public string SearchInput { get; set; }

        public SearchViewModel(TVMazeClient client)
        {
            _client = client;
        }

        [RelayCommand]
        public async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchInput))
            {
                Results.Clear();
                return;
            }

            var results = await _client.Search(SearchInput);

            Results.Clear();
            foreach(var searchResult in results)
            {
                if (!Results.Contains(searchResult))
                {
                    Results.Add(searchResult);
                }
            }
        }

        [RelayCommand]
        public async Task OpenShowDetailAsync(int id)
        {
            await Shell.Current.GoToAsync("detail", new Dictionary<string, object>
            {
                [nameof(id)] = id
            });
        }
    }
}
