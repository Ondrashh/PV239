﻿using Newtonsoft.Json;

namespace TVTrack.Models.TvMaze;

public class Search
{
    [JsonProperty("score")]
    public double Score { get; set; }

    [JsonProperty("show")]
    public Show Show { get; set; }
}
