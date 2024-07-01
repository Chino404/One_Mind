using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalWall : MonoBehaviour
{
    [SerializeField] GameObject _crystalWall; 
    [SerializeField] private WayPoints _point;

    [SerializeField]private int _cantEnemies;
    public List<Enemy> enemies = new List<Enemy>();

    private Collider _myCollider;
    public bool wallIsActivate;

    [SerializeField, Tooltip("M_Guybrush")] private Material[] _respawnMaterial;
    [SerializeField, Range(0, 3f)] private float _timeDissolve;
    private int _dissolveAmount = Shader.PropertyToID("_DisolveSlide");

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _crystalWall.SetActive(false);

        for (int i = 0; i < _respawnMaterial.Length; i++)
        {
            _respawnMaterial[i].SetFloat(_dissolveAmount, 1);
        }
    }
    

    public void DesactivarColision() => _myCollider.enabled = false;


    public void DesactivarMuro()
    {
        _point.action = false;

        StartCoroutine(WallDisolve());
        //_crystalWall.SetActive(false);
        wallIsActivate = false;
    }

    IEnumerator WallDisolve()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _timeDissolve)
        {
            elapsedTime += Time.deltaTime;

            float lerpDisolve = Mathf.Lerp(1f, 0f, (elapsedTime / _timeDissolve));

            for (int i = 0; i < _respawnMaterial.Length; i++)
            {
                _respawnMaterial[i].SetFloat(_dissolveAmount, lerpDisolve);
            }

            yield return null;
        }

        _crystalWall.SetActive(false);

    }

    public void ActivateWall()
    {
        _crystalWall.SetActive(true);
        wallIsActivate = true;
    }
    
}
