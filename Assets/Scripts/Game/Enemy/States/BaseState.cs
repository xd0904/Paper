public abstract class BaseState //FSM하는겨야이ㅏㅓㅁ나!! 
{
    public Enemy enemy;
    public StateMachine stateMachine;
    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();
}
