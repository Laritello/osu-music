![osu.Music](https://github.com/Laritello/osu-music/blob/main/.github/README/header.jpg?raw=true)

[![GitHub license](https://img.shields.io/github/license/Laritello/osu-music)](https://github.com/Laritello/osu-music/blob/main/LICENSE)
[![.NET](https://github.com/Laritello/osu-music/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Laritello/osu-music/actions/workflows/dotnet.yml)
[![CodeFactor](https://www.codefactor.io/repository/github/laritello/osu-music/badge)](https://www.codefactor.io/repository/github/laritello/osu-music)
[![GitHub issues](https://img.shields.io/github/issues/Laritello/osu-music)](https://github.com/Laritello/osu-music/issues)
[![Release](https://img.shields.io/github/v/release/laritello/osu-music?display_name=release)](https://github.com/Laritello/osu-music/releases/latest)

**osu.Music** is a simple music player for osu! players that want to access their beatmap library without launching the game. It is easy to use because player will automatically find osu! installation folder and import current list of beatmaps. Currently osu.Music is in aplha version, which means that it's only barebone player that offers only basic player functions. More features such as collections import, custom playlists and more are planned in the future releases.

<p align="center">
  <img width=800 src="https://i.imgur.com/75EMfs3.png">
</p>

## Features

* Audio visualization
* Auto-import from osu! library
* Basic music player features
* Customizable UI color scheme
* Discord RPC
* Global hotkeys
* Playlists
* Search

## Planned features

* More visualizations
* Mini player

## Requirements

* Windows 7 or higher
* .NET Core 3.1

## Build

* Get the code:
    ```
    git clone https://github.com/Laritello/osu-music.git
    ```
* Switch directory:
    ```
    cd osu-music
    ```
* Run build script: 
    ```
    build\build.bat
    ```
## Feedback

Since this is an alpha you ~~will~~ may encounter some bugs. I would appreciate you <a href="https://github.com/laritello/osu-music/issues">reporting these bugs</a>. Also, if you have an idea how to improve osu.Music you can <a href="https://github.com/laritello/osu-music/issues">suggest a feature</a>.

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Special thanks
* <a href="https://github.com/ppy">ppy</a> for <a href="https://osu.ppy.sh/home">an awesome game</a>
* <a href="https://github.com/HoLLy-HaCKeR">HoLLy-HaCKeR</a> for the <a href="https://github.com/HoLLy-HaCKeR/osu-database-reader">osu-database-reader</a> 
* Norberth Csorba for the <a href="https://stackoverflow.com/questions/55599743/naudio-fft-returns-small-and-equal-magnitude-values-for-all-frequencies">FrequencySpectrum class</a>
* amdpastrana for the <a href="https://www.aimp.ru/forum/index.php?topic=60001.0">AIMP Skin</a> which I used as a base for osu.Music UI
* <a href="https://github.com/meJevin">meJevin</a> for the <a href="https://www.youtube.com/watch?v=nwsEi0JZM3k">Squirrel update tutorial</a>

## Open source notices
Full list of used libraries and resources is available in "About" section within the program.
