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
- exit scene (optional): Name of Unity scene (in other graph) to open when leaving graph.
	- TODO: use state ID instead of scene (get IDs by parsing other graph - need to specify graph)
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

In "Graph/State Graph" menu:
- [ ] Save Graph
	- To XML
	- States can be referred to in other graphs as "exit scenes".
- [ ] Load Graph
	- From XML
- [ ] Generate Scenes
	- [ ] Scenes (+scripts) are generated for each state (with exit conditions and operations).
	- [ ] Existing scenes/scripts are updated (when possible)
		- Backup of modified scenes and unused scenes (if no more associated state).
			(specific directory, unique for each backup)
    - [ ] Report of new+modified scenes before starting process (need user confirmation).


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
