using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomDecoration : MonoBehaviour
{
    public Transform Broom;
    public float delay = 5f;
    public float speed = 2f;

    public Transform startTransform;
    public Transform endTransform;
    private bool movingToTarget = true;
    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(MoveBroom());
    }

    IEnumerator MoveBroom()
    {
        while (true)
        {
            isMoving = true;
            Transform target = movingToTarget ? endTransform : startTransform;
            while (Vector3.Distance(Broom.position, target.position) > 0.01f)
            {
                Broom.position = Vector3.MoveTowards(Broom.position, target.position, speed * Time.deltaTime);
                yield return null;
            }

            isMoving = false;
            movingToTarget = !movingToTarget;
            yield return new WaitForSeconds(delay);
        }
    }

}
