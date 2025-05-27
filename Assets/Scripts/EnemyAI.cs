using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    
    [SerializeField] private float chaseRange;
    [SerializeField] private float turnSpeed;

    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private float _distanceToTarget;
    private bool _isProvoked;
    
    private Transform _target;
    
    private void Start()
    {
        _target = PlayerSingleton.Instance.transform;
        
        _isProvoked = false;
        _distanceToTarget = Mathf.Infinity;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("idle");
    }
    
    private void Update()
    {
        _distanceToTarget = Vector3.Distance(_target.position, transform.position);
        if (_isProvoked)
        {
            EngageTarget();
        }
        else if (_distanceToTarget <= chaseRange)
        {
            _isProvoked = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isProvoked)
        {
            FaceTarget();
        }
    }
    
    private void EngageTarget()
    {
        if (_distanceToTarget >= _navMeshAgent.stoppingDistance)
        {
            ChaseTarget();
        }

        if (_distanceToTarget <= _navMeshAgent.stoppingDistance)
        {
            AttackTarget();
        }
    }
    
    private void ChaseTarget()
    {
        _animator.SetBool("attack", false);
        _animator.SetTrigger("move");
        _navMeshAgent.SetDestination(_target.position);
    }

    private void AttackTarget()
    {
        _animator.SetBool("attack", true);
        Debug.Log("Attacking the target!");
    }

    private void FaceTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, turnSpeed * Time.fixedDeltaTime);
        _navMeshAgent.transform.LookAt(_target);
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}
