# TODO

## doc / readme

- [ ] add diagrams (look in 'docs/images')
	- UI
	- example graphs + generated XML files


## main

- [x] create working 'dev' branch
- [x] add 'meta' files to repo
- [ ] ? - add example resources files to repo?
- [ ] make first release 0.1

- [x] handle 'isLevel'

- [ ] Remove 'End' node => Useless and create invalid transition (to non existent 'End' state...)

## UI

- [ ] limit input characters in state name edit texfield?
- [x] add 'leavable' checkbox
- [ ] add search window, with several state types ('basic', 'level', 'map', etc.)

## LATER
- [ ] import graph (from xml)
- [ ] add minimap
	- [ ] fix size issue
	- [x] hide text/legends?
	- [ ] add contextual options? (resize...)
- [ ] Generate Scenes
	- [ ] add checks before generating xml
	- [ ] Scenes (+scripts) are generated for each state (with exit conditions and operations).
	- [ ] Existing scenes/scripts are updated (when possible)
		- Backup of modified scenes and unused scenes (if no more associated state).
			(specific directory, unique for each backup)
    - [ ] Report of new+modified scenes before starting process (need user confirmation).
- create template scenes (eg: "preLevelState", "postLevelState", etc.) ("Map"?)
- create similar tool to create level tree.
	+ add infos from "LEVELS.TXT"
