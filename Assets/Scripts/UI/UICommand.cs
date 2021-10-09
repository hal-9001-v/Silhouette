using TMPro;
using UnityEngine;

public class UICommand : MonoBehaviour
{
    [Header("HP References")]
    [SerializeField] TextMeshProUGUI _healthMesh;
    [Header("Money References")]
    [SerializeField] TextMeshProUGUI _moneyMesh;

    [Header("Group References")]
    [SerializeField] CanvasGroup _binocucomGroup;
    [SerializeField] CanvasGroup _gameplayGroup;
    [SerializeField] CanvasGroup _textGroup;


    public void SetHealth(float health)
    {
        if (_healthMesh != null)
        {
            _healthMesh.text = health.ToString();
        }
    }

    public void SetMoney(float money)
    {
        if (_moneyMesh != null)
        {
            _moneyMesh.text = money.ToString();
        }

    }

    public void DisplayBinocucom(bool hideOthers)
    {
        if (_binocucomGroup)
        {
            _binocucomGroup.alpha = 1;
        }

        if (hideOthers)
        {
            HideGameplayGroup();
            HideTextGroup();
        }

    }

    public void HideBinocucom()
    {
        if (_binocucomGroup)
        {
            _binocucomGroup.alpha = 0;
        }
    }

    public void HideGameplayGroup()
    {
        if (_gameplayGroup)
        {
            _gameplayGroup.alpha = 0;
        }
    }

    public void DisplayGameplayGroup(bool hideOthers)
    {
        if (_gameplayGroup)
        {
            _gameplayGroup.alpha = 1;
        }

        if (hideOthers)
        {
            HideTextGroup();
            HideBinocucom();
        }
    }

    public void DisplayTextGroup(bool hideOthers)
    {
        if (_textGroup)
        {
            _textGroup.alpha = 1;
        }


        if (hideOthers)
        {
            HideGameplayGroup();
            HideBinocucom();
        }
    }

    public void HideTextGroup()
    {
        if (_textGroup)
        {
            _textGroup.alpha = 0;
        }
    }

}
