using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API;
using TVTrack.API.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ShowDetailViewModel: ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVMazeClient _client;

        public Show Show { get; set; }

        public ShowDetailViewModel(TVMazeClient client)
        {
            _client = client;
        }

        public override async Task OnAppearingAsync()
        {
            Show = await _client.GetShowDetails(Id);
        }
    }
}
