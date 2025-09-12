using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private StateMachine stateMachine;
    private NavMeshAgent agent;
    private Vector3 lastknowPos;

    public NavMeshAgent Agent { get => agent; }
    public GameObject Player { get => player; }

    public Vector3 LastKnowPos { get => lastknowPos; set => lastknowPos = value; } //받아서 값으로 

    [SerializeField]
    private string currentState;

    public Paths path;
    [Header("sight Values")]
    private GameObject player;
    public float sightDistance = 20f;
    public float fieldOfView = 80f;
    public float eyeHeight;


    void Start()
    {

        stateMachine = GetComponent<StateMachine>();
        agent = GetComponent<NavMeshAgent>();

        stateMachine.Initialize();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CanSeePlayer();
        currentState = stateMachine.activateState.ToString();
    }

    public bool CanSeePlayer()
    {
        if (player != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < sightDistance)
            {
                Vector3 targetDirection = player.transform.position - transform.position - (Vector3.up * eyeHeight); //각도계산
                float angleToPlayer = Vector3.Angle(targetDirection, transform.forward);
                if (angleToPlayer >= -fieldOfView && angleToPlayer <= fieldOfView)// 각도
                {
                    Ray ray = new Ray(transform.position + (Vector3.up * eyeHeight), targetDirection);
                    RaycastHit hitInfo = new RaycastHit();
                    if (Physics.Raycast(ray, out hitInfo, sightDistance))
                    {
                        if (hitInfo.transform.gameObject == player)
                        {
                            //다 아니면 보이는거다
                            Debug.DrawRay(ray.origin, ray.direction * sightDistance, Color.red);
                            return true;
                        }
                    }
                    Debug.DrawRay(ray.origin, ray.direction * sightDistance);
                }
            }
        }
        return false;

    }

    

   
}


