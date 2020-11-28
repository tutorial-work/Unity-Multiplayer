# Real Time Strategy Demo
#### Foundation of a Multiplayer RTS game

## Builds
To play the current build of the game, please download the following <strong>.zip</strong> file to your corresponding operating system from <strong>Dropbox</strong>. Do not change any of the contents of the folder to run the game.
- ~~Mac OS Build Download (test *failed* on macOS Mojave 10.14.6, Steamworks cannot connect to Steam)~~
- [Windows Build Download (tested on Windows 10)](https://www.dropbox.com/s/hqdcxf1a9i8jc76/Windows.zip?dl=0 "Windows.zip download")

### Essential Packages 
These packages, along with a few others, come with this repository and are required to run the project in the <strong>Unity Editor</strong>.
- <strong>Mirror 26</strong> (download the latest version from the <strong>Unity Asset Store</strong>)
- [FizzySteamworks v2.6](https://github.com/Chykary/FizzySteamworks/releases "FizzySteamworks Steam Transport Repository")
	
### Todo
List of objectives to complete

- <strong>General</strong>
	- [x] complete GameDev.tv tutorial
	- [x] add Steam username to lobby
	- [x] add Steam profile picture to lobby
- <strong>Better Map Interaction</strong>
	- [x] add Steam profile picture to game
	- [x] update map with better environment
	- [x] spawn camera ontop of Unit Base
	- [x] camera zoom and camera rotation
	- [x] spectator mode after defeat
	- [x] select all units button 
	- [ ] ~~can delete building~~
- <strong>Buildings and Units</strong>
	- [x] add unit prefabs
	- [x] builder cars 
	- [x] units auto attack if they take damage and don't have a task
	- [x] resource storage increase max resources
	- [x] balance building attributes
	- [x] balance unit attributes
	- [x] resource generator personal space range
	- [x] resource storage cap
	- [ ] ~~unit population~~
- <strong>User Interface</strong>
	- [x] update building Icons
	- [x] buttons light up when you can afford building	
	- [x] description UI with controls/units/buildings
	- [ ] ~~resignation UI~~
- <strong>Quality of Life</strong>
	- [x] increase number of max players
	- [x] change health and selection colors to match team color
	- [x] post processing 
 	- [ ] ~~control groups~~
	- [ ] ~~team color picker~~
	- [ ] ~~teams~~
	- [ ] ~~left click on minimap doesn't deselect units~~
	- [ ] ~~left click on UI doesn't drag unit selection box~~
- <strong>Debugging</strong>
	- [x] unit base/player death causes crash
	- [x] muliple lobbies in one session crashes game
	- [x] health/selection colors not set on clients