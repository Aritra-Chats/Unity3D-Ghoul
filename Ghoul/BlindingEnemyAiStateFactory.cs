using System.Collections.Generic;

enum EBlindingEnemyAiStates
{
    Hiding,
    Roaming,
    Pursuing,
    Attacking,
}

public class BlindingEnemyAiStateFactory
{
    BlindingEnemyAiStateMachine _context;
    Dictionary<EBlindingEnemyAiStates, BlindingEnemyAiBaseState> _states = new Dictionary<EBlindingEnemyAiStates, BlindingEnemyAiBaseState>();

    public BlindingEnemyAiStateFactory( BlindingEnemyAiStateMachine currentState)
    {
        _context = currentState;
        _states[EBlindingEnemyAiStates.Hiding] = new BlindingEnemyHidingState(_context, this); 
        _states[EBlindingEnemyAiStates.Roaming] = new BlindingEnemyRoamingState(_context, this); 
        _states[EBlindingEnemyAiStates.Pursuing] = new BlindingEnemyPursuingState(_context, this); 
        _states[EBlindingEnemyAiStates.Attacking] = new BlindingEnemyAttackState(_context, this); 
    }

    public BlindingEnemyAiBaseState Hiding()
    {
        return _states[EBlindingEnemyAiStates.Hiding];
    }
    public BlindingEnemyAiBaseState Roaming()
    {
        return _states[EBlindingEnemyAiStates.Roaming];
    }
    public BlindingEnemyAiBaseState Pursuing()
    {
        return _states[EBlindingEnemyAiStates.Pursuing];
    }
    public BlindingEnemyAiBaseState Attacking()
    {
        return _states[EBlindingEnemyAiStates.Attacking];
    }
}
