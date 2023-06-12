using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public abstract class AIControllerBase : MonoBehaviour
{
    public AIData data;
    

    protected virtual void Awake()
    {
        data.anim= GetComponent<Animator>();
    }
    protected virtual void FixedUpdate()
    {

    }

    public void Walk() => RunAgent(RandomNavmeshDestination(), data.walkSpeed);
    public void Idle() => RunAgent(RandomNavmeshDestination(), data.walkSpeed);
    public void Chase() { }
    public void Scared()
    {
        data.anim.SetBool("die", false);
        data.anim.SetBool("walk", false);
        data.anim.SetBool("hit", true);
        if (data.target != null)
        {
            Vector3 destination = -data.target.position + transform.position;
            destination = destination.normalized;

            RunAgentImmediate(transform.position + destination * 100, data.runSpeed);
        }
    }
    public void Death()
    {
        data.anim.SetBool("die", true);

    }
    private void TargetSpotted() { }

    protected void SetState(AIState state)
    {
        if (data.state == state) return;
        data.prevState = data.state;
        data.state = state;
    }
    
    private void RunAgent(Vector3 destination, float speed) 
    {

        if(data.agent !=null && data.agent.remainingDistance <= data.agent.stoppingDistance )
        {
            data.agent.speed = speed;
            data.agent.SetDestination(destination);
        }
    }
    private void RunAgentImmediate(Vector3 destination, float speed)
    {
        Vector3 finalDestination = destination;
        if (NavMesh.SamplePosition(destination, out NavMeshHit hit, data.radius * 10, 1))
        {
            finalDestination = hit.position;
        }
        if (data.agent != null )
        {
            data.agent.speed = speed;
            data.agent.SetDestination(finalDestination);
        }
    }

    public void RunState()
    {
        switch (data.state)
        {
            case AIState.Idle:
                Idle();
                break;
            case AIState.Chase:
                Chase();
                break;
            case AIState.Scared:
                Scared();
                break;
            case AIState.TargetSpotted:
                TargetSpotted();
                break;
            case AIState.Walk:
                Walk();
                break;
            default:
                Debug.Log("non state");
                break;

        }
    }
   
    protected Vector3 RandomNavmeshDestination()
    {
        Vector3 finalDestination = transform.position;
        Vector3 randomDestination = Random.insideUnitSphere * data.radius*100 + finalDestination  ;
        if(NavMesh.SamplePosition(randomDestination, out NavMeshHit hit, data.radius *10,1))
        {
            finalDestination = hit.position;
        }

        return finalDestination;
    }
}
