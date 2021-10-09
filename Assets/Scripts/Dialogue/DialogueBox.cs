using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(CanvasGroup), typeof(Animator))]
public class DialogueBox : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI _textMesh;

    Animator _animator;
    Coroutine _typeCoroutine;

    float _timeForDestruction = 2;

    const string AppearTrigger = "Appear";
    const string DissappearTrigger = "Disappear";

    public bool isTextComplete { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _textMesh.text = "";

        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

    }

    public void StartDialogue(Vector3 position, string text, float delay)
    {
        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

        _typeCoroutine = StartCoroutine(TypeText(text, delay));

        transform.position = position;

        _animator.SetTrigger(AppearTrigger);
    }

    IEnumerator TypeText(string text, float delay)
    {
        isTextComplete = false;
        _textMesh.text = text;
        _textMesh.maxVisibleCharacters = 0;

        while (_textMesh.maxVisibleCharacters < text.Length)
        {
            _textMesh.maxVisibleCharacters++;

            yield return new WaitForSeconds(delay);
        }

        isTextComplete = true;

    }

    public void Complete()
    {
        isTextComplete = true;

        if (_typeCoroutine != null)
            StopCoroutine(_typeCoroutine);

        _textMesh.maxVisibleCharacters = _textMesh.text.Length;

    }

    public void Clear()
    {
        _animator.SetTrigger(AppearTrigger);

        StartCoroutine(DestroyCountDown(_timeForDestruction));
    }

    IEnumerator DestroyCountDown(float time)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }


}
