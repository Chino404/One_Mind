using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarsWall : DesactiveWall
{
    public GameObject[] bars;
    [SerializeField] private float _minDelay = 0.2f;
    [SerializeField] private float _maxDelay = 1f;
    [SerializeField] private float _minSpeed = 1f;
    [SerializeField] private float _maxSpeed = 3f;
    public Transform waypoint;

    public override void Active()
    {
        base.Active();
    }

    public override void Desactive()
    {
        foreach (var item in bars)
        {
            float delay = Random.Range(_minDelay, _maxDelay);
            float speed = Random.Range(_minSpeed, _maxSpeed);
            StartCoroutine(DesactiveBars(item, delay, speed));
        }
    }

    IEnumerator DesactiveBars(GameObject bar, float delay, float speed)
    {
        yield return new WaitForSeconds(delay);
        Vector3 startpos = bar.transform.position;
        Vector3 targetPos = startpos - new Vector3(0, -waypoint.position.y, 0);
        float elapsedTime = 0f;
        float duration = waypoint.position.y / -speed;

        while (elapsedTime < duration)
        {
            bar.transform.position = Vector3.Lerp(startpos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bar.transform.position = targetPos;
    }
}
