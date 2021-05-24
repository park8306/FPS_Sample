using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 100f;

    public Rigidbody rigidbody;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = transform.forward * speed;
    }
    // Update is called once per frame
    //void Update()
    //{
    //    transform.Translate(0, 0, speed * Time.deltaTime);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{collision.transform.name}이 총알과 충돌했음");
        Destroy(gameObject);

        var targetEnemyRoot = collision.transform.root;
        if (targetEnemyRoot == null)
            return;
        var targetEnemy = targetEnemyRoot.GetComponent<TargetEnemy>();
        if (targetEnemy)
            targetEnemy.OnHit();

        //collision.transform.root.GetComponent<TargetEnemy>().OnHit();
    }
}
