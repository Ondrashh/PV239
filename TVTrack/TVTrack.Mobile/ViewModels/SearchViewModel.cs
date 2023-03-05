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
        TVMazeClient client = new TVMazeClient();

        public ObservableCollection<Search> Results { get; set; } = new ObservableCollection<Search>();

        public string SearchInput { get; set; }

        [RelayCommand]
        public async Task Search()
        {
            if (string.IsNullOrWhiteSpace(SearchInput))
            {
                Results.Clear();
                return;
            }

            var results = await client.Search(SearchInput);

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
        public async Task OpenShowDetail()
        {
            int id = Results.FirstOrDefault().Show.Id;
            var result = await client.GetShowDetails(id);
        }
    }
}
