using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portals : MonoBehaviour
{
    public Portals otherPortal;
    public CameraTracker cameraBongo;
    public CameraTracker cameraFrank;
    public PointsForTheCamera pointsBongo;
    public PointsForTheCamera pointsFrank;
    public bool isEnabled;
    [SerializeField] private int _coolDown=10;
    
    private bool _isInCoolDown;
    private Transform _player;

    private void Update()
    {
        if (isEnabled && otherPortal.isEnabled && !_isInCoolDown)
        {
            Debug.Log("entro al if");
            Teleport(_player);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            isEnabled = true;
            _player = other.transform;
            Debug.Log("is enabled");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            isEnabled = false;
            _player = null;
            Debug.Log("is not enabled");
        }
    }

    private void Teleport(Transform player)
    {
        Debug.Log("se teletransporto");
        player.transform.position = otherPortal.transform.position;

        if (player.GetComponent<ModelBongo>())
        {
            pointsBongo.characterTarget = CharacterTarget.Frank;
            cameraBongo.MyCharacterTarget = CharacterTarget.Frank;
            cameraBongo.Target= GameManager.instance.frank.transform;
        }

        if (player.GetComponent<ModelFrank>())
        {
            pointsFrank.characterTarget = CharacterTarget.Bongo;
            cameraFrank.MyCharacterTarget = CharacterTarget.Bongo;
            cameraFrank.Target = GameManager.instance.bongo.transform;

        }

        _isInCoolDown = true;
        
    }
}
