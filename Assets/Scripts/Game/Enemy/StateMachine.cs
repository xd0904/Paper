using UnityEngine;

public class StateMachine : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public BaseState activateState;


    void Start()
    {
        ChangeState(new PatrolState());
    }
    public void Initialize()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (activateState != null)
        {
            activateState.Perform();
        }
    }

    public void ChangeState(BaseState newState)
    {
        if (activateState != null)
        {
            activateState.Exit(); //���� ���¿��� ������
        }

        activateState = newState; //���ο� ����

        if (activateState != null)
        {
            activateState.stateMachine = this;
            activateState.enemy = GetComponent<Enemy>();
            activateState.Enter();
        }

    }
}


