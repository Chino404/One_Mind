
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    private Animator _myAnimator;
    private Button _button;

    public Image[] _imageCollectable;

    private void Awake()
    {
        _myAnimator = GetComponent<Animator>();
        _button = GetComponent<Button>();
    }

    public void PointEnterFunc()
    {
        if (!_button.interactable) return;

        for (int i = 0; i < _imageCollectable.Length; i++)
        {
            _imageCollectable[i].gameObject.SetActive(true);
        }
    }

    public void PointExitFunc()
    {
        if (!_button.interactable) return;

        _myAnimator.SetTrigger("PointExit");

        for (int i = 0; i < _imageCollectable.Length; i++)
        {
            _imageCollectable[i].gameObject.SetActive(false);
        }

    }
}
