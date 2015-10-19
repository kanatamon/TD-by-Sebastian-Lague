using UnityEngine;
using System.Collections;

[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {

    public enum State{Idle, Chasing, Attacking};
    State currentState;

    public ParticleSystem deathEffect;

    Color originalColor;

    Transform target;
    LivingEntity targetEntity;
    NavMeshAgent pathFinder;
    Material skinMaterial;

    float attackDistanceThreshold = 2f;
    float timeBetweenAttack = 1f;
    float nextAttackTime;
    float damage = 1f;
	
    float myColliderRadius;
    float targetColliderRadius;

    bool hasTarget;

    // Use this for initialization
    protected override void Start () {
        base.Start();

        currentState = State.Idle;
        hasTarget = GameObject.FindWithTag("Player") != null;

        if (hasTarget)
        {
            target = GameObject.FindWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            pathFinder = GetComponent<NavMeshAgent>();
            currentState = State.Chasing;
            
            StartCoroutine(UpdatePath());
            
            myColliderRadius = GetComponent<CapsuleCollider>().radius;
            targetColliderRadius = target.GetComponent<CapsuleCollider>().radius;
            
            skinMaterial = GetComponent<Renderer>().material;
            originalColor = skinMaterial.color;
        }
    }

    public override void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        base.TakeHit(damage, hitPoint, hitDirection);

        if (damage > health)
        {
            Destroy(Instantiate(deathEffect.gameObject, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection)) as GameObject, deathEffect.startLifetime);
        }
    }

    void OnTargetDeath(){
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update(){

        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDistToTarget = (transform.position - target.position).sqrMagnitude;
                float accualAttDistThreshold = myColliderRadius + targetColliderRadius + attackDistanceThreshold + 0.01f;   // 0.01 is a small gab allowed enemy attacking without staying at the purpose position  
                if (sqrDistToTarget <= Mathf.Pow(accualAttDistThreshold, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttack;
                    StartCoroutine(Attack());
                }
            }
        }

    }

    IEnumerator Attack(){

        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 dirToMe = (transform.position - target.position).normalized;
        Vector3 originalPosition = transform.position;
        //Vector3 attackPosition = target.position;
        Vector3 attackPosition = target.position + dirToMe * myColliderRadius;

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;

        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if(percent >= 0.5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent,2) + percent)*4;
            transform.position = Vector3.Slerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        skinMaterial.color = originalColor;

        pathFinder.enabled = true;
        currentState = State.Chasing;
    }

    IEnumerator UpdatePath(){
        float refreshRate = 0.5f;

        while (hasTarget)
        {
            if(currentState == State.Chasing){
                //Vector3 targetPosition = new Vector3(target.position.x, 0f, target.position.z);
                Vector3 dirToMe = (transform.position - target.position).normalized;
                Vector3 targetPosition = target.position + dirToMe * (myColliderRadius + targetColliderRadius + attackDistanceThreshold);
                //targetPosition = new Vector3(targetPosition.x, 0f, targetPosition.z);

                if(!isDead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }

            yield return new WaitForSeconds(refreshRate);
        }
    }

}
