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

    private void Awake()
    {
        _myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        _crystalWall.SetActive(false);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.GetComponent<Enemy>())
    //        _crystalWall.SetActive(true);
    //}

    public void DesactivarColision() => _myCollider.enabled = false;


    public void DesactivarMuro()
    {
        _point.action = false;
        _crystalWall.SetActive(false);
        wallIsActivate = false;
    }

    public void ActivateWall()
    {
        _crystalWall.SetActive(true);
        wallIsActivate = true;
    }
    
}
