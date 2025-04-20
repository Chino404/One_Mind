using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Collectible : MonoBehaviour
{
    public CharacterTarget trinketCharacter;
    private string _myName;

    [Space(10),SerializeField] private float _rotationSpeed;

    private void Awake()
    {
        _myName = trinketCharacter == CharacterTarget.Bongo ? "BongoTrinket" : "FrankTrinket";
    }

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        if (CallJson.instance.refJasonSave.GetValueCollectableDict(GameManager.instance.IndexLevel, _myName))
        {
            Debug.Log($"Ya fui recogido. (<color=yellow>{gameObject.name}</color>)");
            gameObject.SetActive(false);
        }

    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, _rotationSpeed, 0) * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Characters>())
        {
            Take();
            other.GetComponent<Characters>().TakingCollectible();
        }
    }


    /// <summary>
    /// Cuando el jugador toma el coleccionable
    /// </summary>
    private void Take()
    {
        Scene currentScene = SceneManager.GetActiveScene(); //GetActiveScene() es para averiguar en que escena estas

        if (trinketCharacter == CharacterTarget.Bongo)
        {
            GameManager.instance.isTakeCollBongo = true;
            GameManager.instance.nameCollBongo = _myName;


            //Aviso a la UI que tome el coleccionable
            GameManager.instance.UICollBongo.ShowUI();
            GameManager.instance.UICollBongo.UICollectibleTaken();

            //Debug.LogWarning("Coleccionable BONGO agarrado");
            gameObject.SetActive(false);
        }
        else
        {
            GameManager.instance.isTakeCollFrank = true;
            GameManager.instance.nameCollFrank = _myName;

            //Aviso a la UI que tome el coleccionable
            GameManager.instance.UICollFrank.ShowUI();
            GameManager.instance.UICollFrank.UICollectibleTaken();

            //Debug.LogWarning("Coleccionable FRANK agarrado");
            gameObject.SetActive(false);
        }

    }
}
