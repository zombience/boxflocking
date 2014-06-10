abstract public class FSMState  <T>   
{
	public T owner;
	abstract public void Enter (T o);
	abstract public void Execute();
	abstract public void Exit();
}
