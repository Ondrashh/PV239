# TvTrackServer

Please note that the implementation of the server is not perfect and is lacking the proper structure, because the aim of this project was to concentrate on the mobile app instead and this server’s role is only to support data to it.

## Known limitations

- The server DOES NOT check the validity of tvMazeIds supplied in POST requests. This may result in unexpected errors when the server tries to match the invalid tvMazeIds with actual TvMaze response in endpoints such as GET shows.

## Data diagram

Of the data stored in our databases (not from TvMaze api).

```mermaid
erDiagram
	User {
		string Username
	}
	ShowList {
		string name
		string Description
		bool Default
	}
	ShowListItem {
		int TvMazeId
	}
	ShowActivity {
		int TvMazeId
		bool Notifications
		bool Calendar
		bool UserRated
		int UserRating
	}
	EpisodeActivity {
		int TvMazeId
		bool Watched
	}
	UserRatedShow {
		int TvMazeId
		int UserRatingCount
		float UserRating
	}
	User 1--many(0) ShowList: owns
	ShowList 1--many(0) ShowListItem: contains
	User 1--many(0) ShowActivity: has
	ShowActivity 1--many(0) EpisodeActivity: contains
```

