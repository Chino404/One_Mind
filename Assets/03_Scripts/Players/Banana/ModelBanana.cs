using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModelBanana : Characters
{
    //[Header("Camara")]
    //[Range(1f, 1000f), SerializeField] private float _mouseSensivility = 100f;

    //[Header("Componentes")]
    //[SerializeField] private Transform _headTransform;
    //[SerializeField] private Image _visorImage;

    [Header("Valores Perosnaje")]
    [SerializeField, Tooltip("Rango para evitar pegarme al objeto de _moveMask")] private float _moveRange = 0.75f; //Rango para el Raycast para evitar q el PJ se pegue a la pared
    [SerializeField] private float _speed = 5f;
    [SerializeField] private LayerMask _moveMask; //Para indicar q layer quierp q no se acerque mucho
    [SerializeField] private float _speedUp = 2f;

    //private float _mouseRotationX;

    //[SerializeField] private FPCamera _camera;
    private Rigidbody _rb;

    private Ray _moveRay;

    //Constructores
    private ViewBanana _view;
    private ControllerBanana _controller;

    [Header("Bullet")]
    public BulletBanana bulletBanana;
    [SerializeField] int _bulletQuantity;
    Factory<BulletBanana> _factory;
    ObjectPool<BulletBanana> _objectPool;

    private void Awake()
    {
        //_visorImage.gameObject.SetActive(false);

        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation; //Me bloquea los 3 ejes a al vez
        //_camera = GetComponentInChildren<FPCamera>();


        _view = new ViewBanana();
        _controller = new ControllerBanana(this);
    }

    private void Start()
    {
        GameManager.instance.players[1] = this;
        //_camera.gameObject.GetComponent<Camera>().enabled = false;

        _factory = new BulletFactory(bulletBanana);
        _objectPool = new ObjectPool<BulletBanana>(_factory.GetObj, BulletBanana.TurnOff, BulletBanana.TurnOn, _bulletQuantity);
    }

    //public void ActivarVisor()
    //{
    //    StartCoroutine(AnimVisor());
    //}

    private void OnDisable()
    {
        //_visorImage.gameObject.SetActive(false);
    }

    private void Update()
    {
        //_controller.ArtificialUpdate();

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.instance.PlayMonkeySFX(AudioManager.instance.shoot);
            var bullet = _objectPool.Get();
            bullet.AddReference(_objectPool);
            //bullet.transform.position = _camera.transform.position;
            //bullet.transform.rotation = _camera.transform.rotation;
            //bulletBanana.transform.forward = _camera.transform.forward;

        }
    }

    private void FixedUpdate()
    {
        _controller.ListenFixedKeys();
    }

    //IEnumerator AnimVisor()
    //{
    //    Color color = _visorImage.color;

    //    _visorImage.gameObject.SetActive(true);
    //    AudioManager.instance.PlaySFX(AudioManager.instance.visorActive);

    //    color.a = 0.5f;
    //    _visorImage.color = color;

    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0.2f;
    //    _visorImage.color = color;


    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0.5f;
    //    _visorImage.color = color;


    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0.2f;
    //    _visorImage.color = color;


    //    yield return new WaitForSeconds(0.1f);
    //    color.a = 0.5f;
    //    _visorImage.color = color;

    //}

    #region Movement
    public void Movement(float xAxis, float zAxis)
    {
        var dir = (transform.right * xAxis + transform.forward * zAxis).normalized;


        if (IsBlocked(dir) || DistTarget(dir)) return; //Si hay algo en frente o salgo del rango no sigo

        _rb.MovePosition(transform.position + dir * _speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// Si el Ray choca con un objeto de la _moveMask, me lo indica con un TRUE
    /// </summary>
    /// <param name="dir"></param>
    /// <returns></returns>
    private bool IsBlocked(Vector3 dir)
    {
        _moveRay = new Ray(transform.position, dir);
        Debug.DrawRay(transform.position, dir * _moveRange, Color.red);

        return Physics.Raycast(_moveRay, _moveRange, _moveMask);
    }

    //public void Rotation(float x, float y)
    //{
    //    _mouseRotationX += x * _mouseSensivility * Time.deltaTime; 

    //    if(_mouseRotationX >= 360 || _mouseRotationX <= -360)
    //    {
    //        _mouseRotationX -= 360 * Mathf.Sign(_mouseRotationX); //Si excedo tal valor, me lo resta
    //    }

    //    y *= _mouseSensivility * Time.deltaTime;

    //    transform.rotation = Quaternion.Euler(0f, _mouseRotationX, 0); //Para que me rote en base al eje y

    //    _camera?.RotationCamera(_mouseRotationX, y);
    //}
    #endregion

    private bool DistTarget(Vector3 dir)
    {
        return Vector3.Distance(transform.position + dir.normalized, gameObject.GetComponent<BananaGuide>().target.position) >= gameObject.GetComponent<BananaGuide>().RangoRadius;
    }

    public void FlyingUp()
    {
        var dir = Vector3.up;

        if(DistTarget(dir)) return;

        //_rb.velocity = Vector3.up * _speedUp * Time.fixedDeltaTime;

        _rb.MovePosition(transform.position + dir * _speedUp * Time.fixedDeltaTime);
    }

    public void StopFly() => _rb.velocity = Vector3.zero;

    public void FlyingDown()
    {
        var dir = Vector3.down;

        if (DistTarget(dir)) return;

        //_rb.velocity = Vector3.down * _speedUp * Time.fixedDeltaTime;

        _rb.MovePosition(transform.position + dir * _speedUp * Time.fixedDeltaTime);
    }

    public override void Save()
    {
        
    }

    public override void Load()
    {
        
    }
}
