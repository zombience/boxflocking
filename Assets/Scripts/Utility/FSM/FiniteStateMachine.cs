public class FiniteStateMachine <T>  
{
	private T owner;
	private FSMState<T> currentState;
	private FSMState<T> previousState;
	
	
	public void Awake()
	{
		currentState = null;
		previousState = null;
	}
	
	public void Initialize(T o, FSMState<T> initialState) 
	{
		owner = o;
		ChangeState(initialState);
	}
	
	public void  Update() 
	{
		
		if (currentState != null) currentState.Execute();
	}
	
	public void  ChangeState(FSMState<T> newState) 
	{
		previousState = currentState;
		if(previousState != null)
			previousState.Exit();

		currentState = newState;
		currentState.Enter(owner);
	}
	
	public void  RevertToPreviousState() 
	{
		if (previousState != null)
		  ChangeState(previousState);
	}
}
