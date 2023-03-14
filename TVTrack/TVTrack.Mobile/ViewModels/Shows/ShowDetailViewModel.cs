using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TVTrack.API;
using TVTrack.API.Models;
using TVTrack.Mobile.Models;

namespace TVTrack.Mobile.ViewModels.Shows
{
    [QueryProperty(nameof(Id), "id")]
    public partial class ShowDetailViewModel: ViewModelBase
    {
        public int Id { get; set; }

        private readonly TVMazeClient _client;
        private readonly IMapper _mapper;

        public ShowDetailModel Show { get; set; }

        public ShowDetailViewModel(TVMazeClient client,
            IMapper mapper)
        {
            _client = client;
            _mapper = mapper;
        }

        public override async Task OnAppearingAsync()
        {
            var apiShow = await _client.GetShowDetails(Id);
            Show = _mapper.Map<ShowDetailModel>(apiShow);
        }
    }
}
