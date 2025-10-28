# TODO

## Main

- [ ] Add example resources files to repo (should add global and game `.asset` files)
- [ ] XML: add checks before generating xml file

- [ ] make first release 0.1
- [ ] create new Unity 6.2 project
- [ ] make package?


## UI

- [ ] add 'clear' button (reset graph)
- [ ] limit input characters in state name edit texfield?
- [ ] add search window, with several state types ('basic', 'level', 'map', etc.)


## Later

- [ ] import graph (from xml)
- [ ] add minimap
	- [ ] fix size issue
	- [x] hide text/legends?
	- [ ] add contextual options? (resize...)

## Maybe...

To automate scenes+scripts creation in state engine.

- [ ] Generate Scenes
	- [ ] Scenes (+scripts) are generated for each state (with exit conditions and operations).
	- [ ] Existing scenes/scripts are updated (when possible)
		- Backup of modified scenes and unused scenes (if no more associated state).
			(specific directory, unique for each backup)
    - [ ] Report of new+modified scenes before starting process (need user confirmation).
- [ ] create template scenes (eg: "preLevelState", "postLevelState", etc.) ("Map"?)
- [ ] create similar tool to create level tree.\
	+ add infos from "LEVELS.TXT"
