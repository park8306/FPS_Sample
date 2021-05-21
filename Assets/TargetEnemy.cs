using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public partial class TargetEnemy : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public NavMeshAgent agent;
    public List<Transform> wayPoints;

    public int wayPointIndex = 0;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        moveSpeed = agent.speed;
        /// 웨이 포인트를 무한 반복하게 만들자.
        /// 페트롤 : 지정된 웨이 포인트 이동
        /// 페트롤이 끝나는 조건
        /// 1. 시야 범위 안에 적이 들어 옴 -> 추적으로 전환.
        /// 2. 소리 듣는 범위 안에서 총소리 발생하면 해당 방향으로 이동 -> 지정위치 이동으로 전환

        // 첫번째 웨이 포인트로 가자

        yield return StartCoroutine(PetrolCo());
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawWireSphere(transform.position, viewingDistance);

        // 시야각표시
        // 호 포시.

        Transform tr = transform;
        float halfAngle = viewingAngle * 0.5f;
        Handles.DrawWireArc(tr.position, tr.up, tr.forward.AngleToYDirection(-halfAngle), viewingAngle, viewingDistance);
        // 오른쪽 왼쪽 표시
        Handles.DrawLine(tr.position, tr.position + tr.forward.AngleToYDirection(-halfAngle) * viewingDistance);
        Handles.DrawLine(tr.position, tr.position + tr.forward.AngleToYDirection(halfAngle) * viewingDistance);
    }

    public float viewingDistance = 3; // 거리
    public float viewingAngle = 90f; // 시야각
    IEnumerator PetrolCo()
    {
        animator.Play("run");
        while (true)
        {
            if (wayPointIndex >= wayPoints.Count)
                wayPointIndex = 0;
            agent.destination = wayPoints[wayPointIndex].position;
            yield return null; // agent의 remainingDistance가 제대로 적용되려면 한프레임 쉬어야됨
            while (true)
            {
                if (agent.remainingDistance == 0) // 얼만큼의 거리가 남았는지 표시가 됨
                {
                    Debug.Log("도착");
                    // 2번째 웨이 포인트로 이동
                    break;
                }
                // 플레이어 탐지.
                // 플레이어와 나와의 위치를 구하자.
                float distance = Vector3.Distance(transform.position, player.position);
                if(distance < viewingDistance)
                {
                    // 시야각에 들어왔다면 찾아라
                    bool insideViewingAngle = false;
                    // 시야각에 들어왔는지 확인하는 로직 넣자.
                    Vector3 targetDir = player.position - transform.position;
                    targetDir.Normalize();
                    float angle = Vector3.Angle(targetDir, transform.forward);
                    if(Mathf.Abs(angle) <= viewingAngle * 0.5f)
                    {
                        insideViewingAngle = true;
                    }


                    if(insideViewingAngle)
                    {
                        Debug.LogWarning("찾았다 -> 추적 상태로 전환");
                    }
                }

                yield return null; // null은 한프레임을 기다림
            }
            wayPointIndex++;
        }
    }
    public GameObject attackedEffect;
    public GameObject destroyEffect;
    public int hp = 3;
    private float moveSpeed;

    //internal void OnHit()
    //{
    //    Debug.Log("OnHit" + name, transform);

    //    hp--;

    //    if (hp > 0)
    //    {
    //        StartCoroutine(ChangeSpeed(0.3f));
    //        Instantiate(attackedEffect, transform.position, transform.rotation);
    //    }
    //    else
    //    {
    //        Instantiate(destroyEffect, transform.position, transform.rotation);

    //        Destroy(gameObject);
    //    }
    //    // 총알에 맞으면 잠시 0.3초 멈추자.
    //    // 총알 맞을 때 이펙트 보여주자.
    //    // HP를 추가해서 3대 맞으면 폭팔.
    //}

    //private IEnumerator ChangeSpeed(float stopTime)
    //{
    //    agent.speed = 0;
    //    yield return new WaitForSeconds(stopTime);
    //    agent.speed = moveSpeed;
    //}
}
