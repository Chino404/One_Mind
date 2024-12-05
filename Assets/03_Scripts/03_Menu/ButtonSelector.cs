
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CollectableMenu
{
    //[Tooltip("Nombre del personaje al que pertenece el coleccionable")]public string playerCollectableName;
    [Tooltip("A quien pertenece este coleccionable")]public CharacterTarget trinketCharacter;
    [Tooltip("Imagen del coleccionble")] public Image imageCollectable;

    [Space(10),Tooltip("Color DESACTIVADO")] public Color deactiveColor;
    [Tooltip("Color ACTIVO")] public Color activeColor;

    [Space(10), Tooltip("Si fue agarrado")] public bool isTaken;
}

public class ButtonSelector : MonoBehaviour
{
    private Animator _myAnimator;
    private Button _button;

    [Tooltip("Index del nivel")] public int indexLevel;
    public CollectableMenu[] _collectables = new CollectableMenu[2];

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    public void PointEnterFunc()
    {
        if (!_button.interactable) return;


        foreach (var collectable in _collectables)
        {
            collectable.imageCollectable.gameObject.SetActive(true);


            if (collectable.trinketCharacter == CharacterTarget.Bongo) collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "BongoTrinket");

            else collectable.isTaken = CallJson.instance.refJasonSave.GetValueCollectableDict(indexLevel, "FrankTrinket");

            if (collectable.isTaken) collectable.imageCollectable.color = collectable.activeColor;
            else collectable.imageCollectable.color = collectable.deactiveColor;
        }
    }

    public void PointExitFunc()
    {
        if (!_button.interactable) return;

        _myAnimator.SetTrigger("PointExit");

        //for (int i = 0; i < _imageCollectable.Length; i++)
        //{
        //    _imageCollectable[i].gameObject.SetActive(false);
        //}

        foreach (var collectable in _collectables)
        {
            collectable.imageCollectable.gameObject.SetActive(false);
        }

    }
}
