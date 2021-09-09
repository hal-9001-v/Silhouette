using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(CanvasGroup))]
public class DialogueBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _textMesh;


    CanvasGroup _canvasGroup;
    Coroutine _typeCoroutine;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();

        _textMesh.text = "";

        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

    }


    bool _newDialogue;

    public void StartDialogue(Vector3 position, string text, float delay)
    {
        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

        _typeCoroutine = StartCoroutine(TypeText(text, delay));

        transform.position = position;
    }

    IEnumerator TypeText(string text, float delay)
    {
        _textMesh.text = text;
        _textMesh.maxVisibleCharacters = 0;

        while (_textMesh.maxVisibleCharacters < text.Length)
        {
            _textMesh.maxVisibleCharacters++;

            yield return new WaitForSeconds(delay);
        }


    }

    public void Clear()
    {
        Destroy(gameObject);
    }


}
