public abstract class BaseState //FSM�ϴ°ܾ��̤��ä���!! 
{
    public Enemy enemy;
    public StateMachine stateMachine;
    public abstract void Enter();

    public abstract void Perform();

    public abstract void Exit();
}
