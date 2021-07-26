using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue Settings")]
    [SerializeField] [TextArea(3, 20)] string _quote;
    [SerializeField] [Range(0.01f, 1)] float _soundDelay;


    TextBox _textBox;

    CharacterMovement _player;

    private void Start()
    {
        _player = FindObjectOfType<CharacterMovement>();
        _textBox = FindObjectOfType<TextBox>();
    }

    public void PlayQuote()
    {
        if (_player != null && _textBox != null)
        {

            /*_player.PlayQuote(_quote, _soundDelay, () => {
                _textBox.Hide();
            });
            */

            string cleanQuote = "";
            char[] quoteLetters = _quote.ToCharArray(); ;

            for (int i = 0; i < quoteLetters.Length; i++)
            {
                if (quoteLetters[i] == '<')
                {
                    i++;
                    while (quoteLetters[i] != '>') {
                        i++;
                    }
                    i++;
                }

                cleanQuote += quoteLetters[i];

            }
            _textBox.DisplayText(cleanQuote, _soundDelay*0.75f);


        }
        else
        {
            Debug.LogWarning("Dialogue " + name + ": No CharacterAnimationCommand in scene!");
        }

    }



}
