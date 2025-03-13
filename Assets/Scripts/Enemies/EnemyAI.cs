using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private float attackCooldown = 5f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State 
    {
        Roaming,
        Attacking
    }
    
    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake() {
        state = State.Roaming;
        enemyPathfinding = GetComponent<EnemyPathfinding>();
    }
    
    private void Start() {
        roamPosition = GetRoamingPosition();
    }

    private void Update() {
        MovementStateControl();
    }

    private void MovementStateControl() {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
            break;

            case State.Attacking:
                Attacking();
            break;
        }
    }

    private void Roaming() {
        timeRoaming += Time.deltaTime;

        enemyPathfinding.MoveTo(roamPosition);

        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange) {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat) {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking() {
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange) {
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack && enemyType) {
            (enemyType as IEnemy).Attack();
            canAttack = false;

            if (stopMovingWhileAttacking) {
                enemyPathfinding.StopMoving();
            } else {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackRangeCooldown());
        }
    }

    private IEnumerator AttackRangeCooldown () {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }

    // Первая реализация рандомного патрулирования/шатания
    // private IEnumerator RoamingRoutine() {
    //     while (state == State.Roaming) {
    //         Vector2 roamPosition = GetRoamingPosition();
    //         enemyPathfinding.MoveTo(roamPosition);
    //         yield return new WaitForSeconds(roamChangeDirFloat);
    //     }
    // }
}
