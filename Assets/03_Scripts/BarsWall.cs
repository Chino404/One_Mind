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
    [SerializeField] private GameObject _particles;
    public Transform waypoint;
    


    [SerializeField] bool _isStartActive;

    

    private void Start()
    {
        
        if (!_isStartActive)
        {
            foreach (var item in bars)
            {
                float delay = Random.Range(_minDelay, _maxDelay);
                float speed = Random.Range(_minSpeed, _maxSpeed);
                StartCoroutine(DesactiveBars(item, delay, speed));
            }
        }
    }

    public override void Active()
    {
        if (!gameObject.activeInHierarchy)
        {
            _shouldCloseOnEnable = true;
            return;
        }
        if (_isStartActive) return;
        _isActing = true;

        AudioManager.instance.Play(SoundId.IronBars);

        foreach (var item in bars)
        {
            StartCoroutine(ActiveBars(item, 0, _maxSpeed * 2));
        }
        
        _isStartActive = true;
        StartCoroutine(EndTransition(0.5f));
    }

    IEnumerator ActiveBars(GameObject bar, float delay, float speed)
    {
        
        Vector3 startpos = bar.transform.position;
        Vector3 targetPos = startpos - new Vector3(0, waypoint.position.y, 0);
        yield return new WaitForSeconds(delay);

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

    public override void Desactive()
    {
        if (!gameObject.activeInHierarchy)
        {
            _shouldOpenOnEnable = true;
            return;
        }
        if (!_isStartActive) return;
        _isActing = true;

        AudioManager.instance.Play(SoundId.IronBars);


        foreach (var item in bars)
        {
            float delay = Random.Range(_minDelay, _maxDelay);
            float speed = Random.Range(_minSpeed, _maxSpeed);
            StartCoroutine(DesactiveBars(item, delay, speed));
            
        }
        StartCoroutine(EndTransition(1.5f));
        _isStartActive = false;
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
    IEnumerator EndTransition(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        _isActing = false;
    }
   

    public override void Save()
    {
        base.Save();
        List<Vector3> barsPositions = new();
        foreach (var item in bars)
        {
            barsPositions.Add(item.transform.position);
        }
        _currentState.Rec(barsPositions,_isStartActive,_isActing);
    }

    public override void Load()
    {
        base.Load();
        if (!_currentState.IsRemember()) return;
        var col = _currentState.Remember();
        List<Vector3> barsPositions = (List<Vector3>)col.parameters[0];
        _isStartActive = (bool)col.parameters[1];
        _isActing = (bool)col.parameters[2];

        for (int i = 0; i < bars.Length; i++)
        {
            if (i < barsPositions.Count)
                bars[i].transform.position = barsPositions[i];
        }
    }
}
