# MarbleDuels
This project is being used to upload videos to the [MarbleDuels YouTube channel](https://www.youtube.com/@MarbleDuels) using Godot to generate the videos.
It attempts to be modular in order to make it easy to customize for different YouTube channels and different video generators.

## Configuration

### Client Secrets
To obtain a `client_secrets.json` file, go to [YouTube's guide](https://developers.google.com/youtube/registering_an_application), and follow the instructions for obtaining OAuth 2.0 credentials. Download and place `client_secrets.json` in the root project folder.

---

### Config
To work with the configuration, add a `config.json` file to the root project folder.

---

To work with the YouTube interface, `config.json` must have a `youtube` object containing a `user`.
For example:
```
"youtube": {
	"user": "your_user",
}
```
The `user` does not need to be anything in particular. It is used to avoid authenticating every time the YouTube interface is used.

---

To work with the Godot video creator, `config.json` must have a `godot` object and a `max_video_seconds`.
For example:
```
"godot": {
	"example_project_path": "path/to/example/project"
},
"max_video_seconds": 2100
```
The `godot` object is used to contain file paths to Godot projects. These paths are used for executing [Godot's movie maker utility](https://docs.godotengine.org/en/stable/tutorials/animation/creating_movies.html). The `godot` object can be used for other configuration values if you choose.
`max_video_seconds` is used to cause Godot to quit after the given number of seconds. If this threshold is reached, the video creation process is considered to have failed. Make sure your Godot project [exits on its own](https://docs.godotengine.org/en/stable/tutorials/animation/creating_movies.html#quitting-movie-maker-mode) to have the creation process succeed.

---

### Godot Command Line
In order to work with Godot, follow [Godot's guide](https://docs.godotengine.org/en/stable/tutorials/editor/command_line_tutorial.html#path) for adding `godot` or `godot-mono` to your `path` environment variable.