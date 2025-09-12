using UnityEngine;

public class AttackState : BaseState
{
   

    private float moveTimer;
    private float attackTimer;
    private float shotTimer;
    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Perform()
    {
        if (enemy.CanSeePlayer())
        {
            attackTimer = 0;
            moveTimer += Time.deltaTime;
            shotTimer += Time.deltaTime;
            enemy.transform.LookAt(enemy.Player.transform);
          
            if (moveTimer > Random.Range(3, 7))//무작위로 움직이는 시간
            {

                enemy.Agent.SetDestination(enemy.transform.position + (Random.insideUnitSphere * 5));
                moveTimer = 0;
            }
            enemy.LastKnowPos = enemy.Player.transform.position;
        }
        else
        {
            attackTimer += Time.deltaTime;
            if (attackTimer > 8)
            {
                stateMachine.ChangeState(new SearchState());
            }
        }
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

