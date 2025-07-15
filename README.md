# unity-state-engine-graphview
Using Unity Graphview to manage state graphs.


## State Properties

(- id: Unique state ID.)
- name: Unique state name.
- scene: Name of Unity scene associated to the state.
    - Should be obtained from state name
	- ! Special case for level scenes! (TODO: allow to specify scene name prefix)
- restartable (default: false): If state can be executed again when coming back from childen states.
- isLevel (default: false): If state is related to game levels.
	=> could be remmoved, and have derived class "LevelState"
(TODO: handled here?
	could be, but not really useful
- exit scene (optional): Name of Unity scene (in other graph) to open when leaving graph.
	? - TODO: use state ID instead of scene (get IDs by parsing other graph - need to specify graph)
	=> if specify state, limited to graph scenes, whereas if scene directly, could be a scene unrelated to grpahs... (what's best?)
)
- next (optional): Next state (see "Transitions").
- children (optional): Children states (see "Transitions").


## Transitions

- Children:
	- Call "LoadChildState(STATE_NAME)" to trigger transition to child state.
	- When expecting to get back from child state when done (state must be "restartable").
	! - State cannot be its own parent/child (even indirectly).

- Next:
	- For obvious/logical transition to next state.
	- Call "End" to trigger transition to next state.
		- If not next state, current state is popped from stack until find a "restartable" parent state.
	- If need more than 1 state, can use "Children" states, in which case the state should not be "restartable".
	- If "Next" state and no "Children" states, the next state is automatically pre-loaded for faster transition.

- Exit Scene:
	- To exit current graph and start state in another graph.
	- Call "Leave" to switch to exit scene.
		! - TODO: handle more than 1 exit scene per state ("LeaveGraph" connections?).


## Process

Create graph in Unity Editor:
- States + properties
- Transitions

Generally 2 graphs:
- Global: Main menu, intro, etc.
- Game: Levels, map, etc.
But flexible.

...

----
parse graph

for each state
	create (partial) class
	=> not needed (?) for states with no children
	(but create it anyway for every state?)
		- 1 function for each child state (LoadChildSTATE)
		+ empty class for state itself? (overrides, etc.)
	create scene
		- add GlobalManager or GameManager
		- replace StateController script if have one associated to state/scene

+ clean existing but unused states stuff...
----

## States & Operations

(see other doc "state_operations.md")


====

# Commands

- accessible from "Graph/State Graph" menu

- Save Graph
	- To 'asset' file
	- States can be referred to in other graphs as "exit scenes".
		=> Or should use scene name instead?
- Load Graph
	- From 'asset'
- Export Graph
	- To 'xml' file


UI Features:
- state
	- name edit
	- add/remove children nodes
- 'copy', 'paste', 'duplicate' for states (right-click menu)
- 'reframe'


TODO:
- [ ] Generate Scenes
	- [ ] add checks before generating xml
	- [ ] Scenes (+scripts) are generated for each state (with exit conditions and operations).
	- [ ] Existing scenes/scripts are updated (when possible)
		- Backup of modified scenes and unused scenes (if no more associated state).
			(specific directory, unique for each backup)
    - [ ] Report of new+modified scenes before starting process (need user confirmation).
UI:
- [ ] limit input characters in state name edit texfield?
- [ ] ? - add 'can exit graph' checkbox (+list of states?)
- [ ] add search window, with several state types ('basic', 'level', 'map', etc.)

LATER:
- [ ] add minimap
	- [ ] fix size issue
	- [x] hide text/legends?
	- [ ] add contextual options? (resize...)
- [ ] import graph (from xml)


====

# TODO

- add diagrams
	- example graphs
- create template scenes (eg: "preLevelState", "postLevelState", etc.) ("Map"?)
- create similar tool to create level tree.
	+ add infos from "LEVELS.TXT"


code (not graph generation):
- class diagrams
- see how to fix editor issues with derived classes

====

# LINKS

GraphView tut:
https://www.youtube.com/watch?v=7KHGH0fPL84
(+part 2)
https://www.youtube.com/watch?v=F4cTWOxMjMY
