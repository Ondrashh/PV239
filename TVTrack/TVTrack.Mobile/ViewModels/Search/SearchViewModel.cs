﻿using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API.Client;
using TVTrack.Mobile.Models;
using TVTrack.TVMaze.Client;

namespace TVTrack.Mobile.ViewModels.Search
{
    public partial class SearchViewModel: ViewModelBase
    {
        private readonly TVTrackClient _client;

        public ObservableCollection<TVTrack.Models.TvMaze.Search> Results { get; set; } = new ObservableCollection<TVTrack.Models.TvMaze.Search>();

        public string SearchInput { get; set; }

        public SearchViewModel(TVTrackClient client,
            IMapper mapper): base(mapper)
        {
            _client = client;
        }

        [RelayCommand]
        public async Task Search()
        {
            LoadingStart();
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
            LoadingEnd();
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
