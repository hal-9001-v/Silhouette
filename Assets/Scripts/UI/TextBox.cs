using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{

    [Header("References")]
    [SerializeField] CanvasGroup _textPanel;
    [SerializeField] TextMeshProUGUI _textMesh;
    [SerializeField] Animator _cutInAnimator;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void DisplayText(string text, float delay)
    {

        if (_textMesh != null)
        {
            StartCoroutine(WriteText(text, delay));
        }
        else
        {
            Debug.LogWarning("No Text Mesh in " + name);
        }


    }

    IEnumerator WriteText(string text, float delay)
    {
        Show();

        _textMesh.text = text;
        _textMesh.maxVisibleCharacters = 0;

        for (int i = 0; i < _textMesh.text.Length + 1; i++)
        {
            _textMesh.maxVisibleCharacters = i;

            yield return new WaitForSeconds(delay);
        }

    }

    public void Hide()
    {
        _textPanel.alpha = 0;
        if (_cutInAnimator != null)
        {
            _cutInAnimator.SetBool("isDisplayed", false);

        }
    }

    public void Show()
    {
        _textPanel.alpha = 1;

        if (_cutInAnimator != null)
        {
            Debug.Log("Show");
            _cutInAnimator.SetBool("isDisplayed", true);
        }
    }

}
