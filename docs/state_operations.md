
# GLOBAL

## States

- Logo: Auto
- Intro: Auto

!- Menu
	- Minimum:
		- New: LoadChild(!) or NewOp op
		- Quit: LoadChild or QuitOp op
	- Optional:
		- Load: LoadChild(!) or LoadOp op
		- Continue: LoadChild or ContinueOp op
	- Others:
		- Credits: LoadChild or within same scene
		- Options: LoadChild or within same scene

- NewGame: NewOp op (not mandatory, useful if several difficulties)
- LoadGame: LoadOp op (not mandatory, useful if several save slots)

- Quit: QuitOp op
- Continue: ContinueOp op (useful if want confirmation screen)

- Options: Auto
- Credits: Auto


## Operations

- NewOp(int): New(difficulty) + Leave (game start state)
	+ SetLevel, if skip Map state (or do it anyway?)
- LoadOp(str|int?): Load(saved_file) + Leave (game state)
	+ SetLevel, if skip Map state (or do it anyway?)
- QuitOp: Leave (no state) (=> Terminate)
- ContinueOp: if (GetLevel) => Leave (game state)
- Auto: End (with/without next state)


## Remarks

- State has list of operations
	- No operation => Auto
	- Templates for states with specific operations
		New, Load, Continue, Quit
	(- Possibility to add/create new operations?)

- Operations allow to exit/leave graph
	- Associated to a state from another graph (or none for QuitOp in global)
	(- by default: - NewOp: Game start state(?))


----

# GAME

## States

- GameIntro: Auto

- Map:
	- Minimum:
		- StartLevelOp
		- Quit: Next|LoadChild or QuitOp op

- GameQuit: QuitOp op

- BeginAnim: Auto (get data from level description)
- Briefing: Auto (get data from level description)

!- Level
	- Minimum:
		- EndLevelOp op (+LoadChild)
	- Optional:
		- EndLevelFailOp op (+LoadChild)
		- Quit:
			- QuitOp op (or LoadChild)

- EndAnim: Auto (idem "BeginAnim")
- EndAnimFail: Auto (idem "BeginAnim")

- Debriefing: Auto (idem "Briefing")
- DebriefingFail: Auto (idem "Briefing")

- GameEnd: Auto (idem "GameIntro")

- GameOver:
	- Minimum:
		- Quit: QuitOp op or LoadChild
	- Optional:
		- Continue: LoadChild

- GameCredits: Auto

- Continue:
	- Minimum:
		- Quit: QuitOp op LoadChild


## Operations

- StartLevelOp(int): SetLevel(int) + LoadChild
- QuitOp: Leave (global start state)
- EndLevelOp: SetLevelCompleted + ... + CommitChanges
	(+EndProcess) (+ optional LoadChild)
- EndLevelFailOp: LoseLife + ... + CommitChanges
	(+EndProcess) (+ optional LoadChild)
(? - Merge "EndLevelOp" & "EndLevelFailOp"?)


### GameOperations

Additional game specific operations.

- Save: Map, Level, Quit
	? - Briefing, Debriefing, (DebriefingFail)
	? - Continue? GameOver?
	=> SaveGame(str|int?)

- CheckGameComplete: in last state related to level
- CheckGameOver: in last state related to level if there is a "fail" branch
- CheckContinue
- UseContinue


## Remarks

...

