
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
	! => CHECK WIN/LOSE/QUIT instead!
	- Minimum:
		- EndLevelOp op (+LoadChild)
	- Optional:
		- EndLevelFailOp op (+LoadChild)
		- Quit:
			- QuitOp op (or LoadChild)

- EndAnim: Auto (idem "BeginAnim")
- EndAnimFail: Auto (idem "BeginAnim")

- Debriefing: Auto (idem "Briefing") (! or else if children...!)
- DebriefingFail: Auto (idem "Briefing") (! or else if children...!)

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
    => In "map" if state exists (auto generated)
    GetGameData().SetLevel(level);
    + 'makeTransition'

- EndLevelOpSuccess:
    => In "level" (auto generated)
    gameData.SetLevelCompleted()
    (EndProcessWin() => To implement/override)
    gameData.CommitChanges()
    + 'win transition'

- EndLevelFailOp:
    => In "level" (auto generated)
    gameData.LoseLife()
    (EndProcessLose() => To implement/override)
    gameData.CommitChanges()
    + 'lose transition'

- QuitLevelOp:
    => In "level" (auto generated)
    + 'quit level transition'

- QuitGameOp:
    => In "level" (auto generated)
    + 'quit game transition'


### GameOperations

Additional game specific operations.

- Check Game Complete: In last state related to level in "success" branch (or default if no success/fail branches).
    => In "level", if transition 'WIN_STOP_TRANSITION_STATE' (auto generated)
    bool GetGameData().IsGameComplete()

- Check Game Over: In last state related to level in "fail" branch.
    bool GetGameData().IsGameOver()

- Check Continue: After "Check Game Over".
    bool GetGameData().CanContinue()

- Use Continue
    int GetGameData().LoseContinue()
        + do it here (+call CanContinue):
            gameData.SetLevel(-1)
            and in post level states, use 'latestLevel' to get the level (as current level is now '-1').

- Save
    GameSessionManager.Instance.SaveGame(str|int?)

