﻿using TVTrack.Models.Database;
using TVTrack.Models.TvMaze;

namespace TVTrack.Mobile.Models
{
    public class UserShowsDetailModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool Default { get; set; } = false;
        public int UserId { get; set; }
        public User? User { get; set; }
        public List<Show> Shows { get; set; } = new();
    }
}
