using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[DefaultExecutionOrder(-1)]
[RequireComponent(typeof(Rigidbody))]
public class ModelFrank : Characters
{
    //Referencias
    private ControllerFrank _controller;
    private ViewFrank _view;
    public static ModelFrank instance;

    //[Tooltip("Esta haciendo la animación")] public bool IsDoingAnimation { get { return _isDoingAnimation; } }

    public override void Awake()
    {
        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        GameManager.instance.modelFrank = instance;
        base.Awake();


        _view = new ViewFrank(this, _animPlayer);
        _controller = new ControllerFrank(this, _view);

        _currentState = new MementoState();
    }

    public override void Start()
    {
        base.Start();

        //ActualMove = NormalMovement;
    }

    public override void Update()
    {
        if (GameManager.instance.isNotMoveThePlayer) return;

        //Si Bongo esta ahciendo laguna animacion pero yo no, me detengo igual.
        if (!IsDoingAnimation && GameManager.instance.modelBongo.IsDoingAnimation) _animPlayer.SetBool("Walk", false);

        foreach (var item in cinematics)
        {
            if (item.state == PlayState.Playing) return;
        }

        base.Update();

        

        _controller.ArtificialUpdate();
    }

    public override void FixedUpdate()
    {

        base.FixedUpdate();

        if (GameManager.instance.isNotMoveThePlayer)
        {
            _rbCharacter.velocity = new Vector3(0, _rbCharacter.velocity.y, 0);

            return;
        }

        //Si estoy en alguna cinematica / animacion, seteo la animacion de caminata.
        if (_isDoingAnimation || GameManager.instance.isNotMoveThePlayer)
        {
            if (_rbCharacter.velocity.x != 0 || _rbCharacter.velocity.z != 0) _view.Walking(true);
            else if (_rbCharacter.velocity.x == 0 && _rbCharacter.velocity.z == 0) _view.Walking(false);
        }

        foreach (var item in cinematics)
        {
            if (item.state == PlayState.Playing)
            {
                _rbCharacter.velocity = new Vector3(0, _rbCharacter.velocity.y, 0);
                return;
            }
        }


        _controller.ListenFixedKeys();
    }

    public override void Attack()
    {
        base.Attack();
    }

    public override void DeadByWater()
    {
        base.DeadByWater();

        CamerasManager.instance.DeathCamera(CharacterTarget.Frank);
    }
}
