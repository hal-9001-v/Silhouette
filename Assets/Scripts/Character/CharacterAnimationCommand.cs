using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationCommand : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Animator _animator;

    [SerializeField] [Range(0.1f, 2)] float _blinkTime = 1;
    [SerializeField] Transform _cigar;

    [Header("Stretch")]
    [SerializeField] Transform _body;
    [Range(0.1f, 1)]
    [SerializeField] float _bodyStretch = 0.2f;

    [Range(0.1f, 1)]
    [SerializeField] float _bodyStretchSpeed = 0.1f;
    const string IDLETRIGGER = "Idle";
    const string IDLINGBOOL = "isIdling";

    const string RUNNINGBOOL = "isRunning";
    const string RUNTRIGGER = "Run";

    const string JUMPTRIGGER = "Jump";
    const string JUMPINGBOOL = "isJumping";


    const string LANDTRIGGER = "Land";

    const string WALLINGBOOL = "isWalling";
    const string WALLMOVINGBOOL = "isWallMoving";

    const string CREEPTRIGGER = "Creep";
    const string CREEPINGBOOL = "isCreeping";

    const string ALIGNINGBOOL = "isAligning";

    const string WALLTRIGGER = "StickToWall";

    //Face
    //Expressions
    const string SMILETRIGGER = "Smile";
    const string SERIOUSTRIGGER = "Serious";
    const string ANGERTRIGGER = "Anger";
    const string COCKYTRIGGER = "Cocky";
    const string SURPRISEDTRIGGER = "Surprised";

    //Talk
    const string TALKINGBOOL = "isTalking";
    const string AEITRIGGER = "MouthAEI";
    const string BMPTRIGGER = "MouthBMP";
    const string ChJShTRIGGER = "MouthChJSh";
    const string FVTRIGGER = "MouthFV";
    const string LTRIGGER = "MouthL";
    const string OTRIGGER = "MouthO";
    const string RESTTRIGGER = "MouthRest";
    const string UTRIGGER = "MouthU";
    const string WQTRIGGER = "MouthWQ";


    //Blink
    const string BLINKTRIGGER = "Blink";

    Coroutine _talkCoroutine;

    //Stretch
    bool _stretch;
    Vector3 _bodyDefaultScale;

    private void Awake()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }

        if (_body != null)
        {
            _bodyDefaultScale = _body.localScale;
        }

        StartCoroutine(Blink());
    }

    private void FixedUpdate()
    {
        UpdateStretch();

    }
    IEnumerator Blink()
    {

        while (true)
        {
            _animator.SetTrigger(BLINKTRIGGER);
            yield return new WaitForSeconds(_blinkTime);

        }

    }

    public void Idle()
    {
        _animator.SetTrigger(IDLETRIGGER);
    }

    public void Run()
    {
        _animator.SetTrigger(RUNTRIGGER);
        _animator.SetBool(RUNNINGBOOL, true);
        _animator.SetBool(CREEPINGBOOL, false);

    }

    public void Creep()
    {
        _animator.SetTrigger(CREEPTRIGGER);
        _animator.SetBool(CREEPINGBOOL, true);
        _animator.SetBool(RUNNINGBOOL, false);
        _animator.SetBool(IDLINGBOOL, false);
    }


    public void Jump()
    {
        _animator.SetBool(JUMPINGBOOL, true);

        _animator.SetTrigger(JUMPTRIGGER);

        //_animator.SetTrigger(JUMPTRIGGER);
    }

    public void Land()
    {
        _animator.SetBool(JUMPINGBOOL, false);
    }

    public void WallIdle()
    {
        _animator.SetBool(WALLINGBOOL, true);
        _animator.SetBool(WALLMOVINGBOOL, false);
        _animator.SetBool(WALLMOVINGBOOL, false);

    }

    public void WallMove()
    {
        _animator.SetBool(IDLINGBOOL, false);

        _animator.SetBool(WALLINGBOOL, true);
        _animator.SetBool(WALLMOVINGBOOL, true);
    }

    public void Align()
    {
        _animator.SetBool(ALIGNINGBOOL, true);
    }

    public void StopAlign()
    {
        _animator.SetBool(ALIGNINGBOOL, false);

    }

    public void Talk()
    {
        _animator.SetBool(IDLINGBOOL, false);
        _animator.SetBool(RUNNINGBOOL, false);
        _animator.SetBool(JUMPINGBOOL, false);
        _animator.SetBool(CREEPINGBOOL, false);
        _animator.SetBool(WALLINGBOOL, false);
        _animator.SetBool(WALLMOVINGBOOL, false);

        _animator.SetBool(TALKINGBOOL, true);

        _cigar.gameObject.SetActive(true);
    }

    public void StopTalk()
    {
        _animator.SetBool(TALKINGBOOL, false);
        _cigar.gameObject.SetActive(false);
    }


    public void Smile()
    {
        _animator.SetTrigger(SMILETRIGGER);
    }

    public void Serious()
    {
        _animator.SetTrigger(SERIOUSTRIGGER);
    }

    public void Anger()
    {

        _animator.SetTrigger(ANGERTRIGGER);
    }

    public void Cocky()
    {

        _animator.SetTrigger(COCKYTRIGGER);
    }

    public void Surprise()
    {
        _animator.SetTrigger(SURPRISEDTRIGGER);
    }

    public void PlayQuote(string quote, float soundDelay, Action action)
    {

        if (_talkCoroutine != null)
        {
            StopCoroutine(_talkCoroutine);
        }


        _talkCoroutine = StartCoroutine(ProcessQuote(quote, soundDelay, action));
    }

    public void Stretch(Vector3 direction)
    {
        _stretch = true;

    }

    public void Unstretch()
    {
        _stretch = false;
    }

    void UpdateStretch()
    {
        if (_stretch)
        {

            if (_body.localScale.x > _bodyDefaultScale.x * _bodyStretch)
            {
                var scale = _body.localScale;

                scale.x -= _bodyStretchSpeed;

                _body.localScale = scale;
            }

        }
        else
        {
            if (_body.localScale.x < _bodyDefaultScale.x)
            {
                var scale = _body.localScale;

                scale.x += _bodyStretchSpeed;

                _body.localScale = scale;

            }

        }
    }

    IEnumerator ProcessQuote(string quote, float soundDelay, Action action)
    {

        Talk();
        string[] letters = SplitQuote(quote.ToLower().ToCharArray());

        for (int i = 0; i < letters.Length; i++)
        {
            string command = GetVoiceCommand(letters[i]);
            if (command == "skip")
                continue;

            if (command != "")
            {
                _animator.SetTrigger(command);

            }
            yield return new WaitForSeconds(soundDelay);
        }

        StopTalk();
        action.Invoke();
    }

    string[] SplitQuote(char[] letters)
    {
        List<string> particles = new List<string>();

        for (int i = 0; i < letters.Length; i++)
        {
            if (i < letters.Length - 1)
            {
                if (letters[i] == 'c' && letters[i + 1] == 'h')
                {
                    particles.Add("ch");
                    i++;
                    continue;
                }

                if (letters[i] == 's' && letters[i + 1] == 'h')
                {
                    particles.Add("sh");
                    i++;
                    continue;
                }

                if (letters[i] == '<')
                {
                    string special = "";
                    i++;
                    while (letters[i] != '>')
                    {
                        special += letters[i];
                        i++;
                    }

                    particles.Add(special);

                }
            }

            particles.Add(letters[i].ToString());


        }

        return particles.ToArray();
    }

    string GetVoiceCommand(string letters)
    {
        string command = "";
        switch (letters.ToLower())
        {
            case "a":
                command = AEITRIGGER;
                break;

            case "b":
                command = BMPTRIGGER;
                break;

            case "c":
                command = ChJShTRIGGER;
                break;

            case "e":
                command = AEITRIGGER;
                break;

            case "f":
                command = FVTRIGGER;
                break;


            case "i":
                command = AEITRIGGER;
                break;

            case "j":
                command = ChJShTRIGGER;
                break;


            case "l":
                command = LTRIGGER;
                break;

            case "m":
                command = BMPTRIGGER;
                break;


            case "o":
                command = OTRIGGER;
                break;

            case "p":
                command = BMPTRIGGER;
                break;

            case "q":
                command = WQTRIGGER;
                break;



            case "u":
                command = UTRIGGER;
                break;

            case "v":
                command = FVTRIGGER;
                break;

            case "w":
                command = WQTRIGGER;
                break;

            case " ":
                break;

            case "-":
                break;

            case ".":
                break;

            case "sh":
                command = ChJShTRIGGER;
                break;

            case "ch":
                command = ChJShTRIGGER;
                break;

            case "smile":
                Smile();
                command = "skip";
                break;

            case "anger":
                Anger();
                command = "skip";
                break;

            case "cocky":
                Cocky();
                command = "skip";
                break;

            case "surprise":
                Surprise();
                command = "skip";
                break;

            case "serious":
                Serious();
                command = "skip";
                break;


            default:
                command = RESTTRIGGER;
                break;

        }

        return command;
    }

}
