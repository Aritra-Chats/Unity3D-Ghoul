public abstract class BlindingEnemyAiBaseState
{
    #region Variables
    private BlindingEnemyAiStateMachine _ctx;
    private BlindingEnemyAiStateFactory _factory;
    #endregion

    #region Getters & Setters
    public BlindingEnemyAiStateMachine CTX { get { return _ctx; } }
    public BlindingEnemyAiStateFactory Factory { get { return _factory; } }
    #endregion

    public BlindingEnemyAiBaseState(BlindingEnemyAiStateMachine currentContext, BlindingEnemyAiStateFactory playerStateFactory)
    {
        _ctx = currentContext;
        _factory = playerStateFactory;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract void CheckSwitchState();

    protected void SwitchState(BlindingEnemyAiBaseState newState)
    {
        //current state exits state
        ExitState();

        //new state enters state
        newState.EnterState();

        //Switch Current State of Context
        _ctx.CurrentState = newState;
    }
}
