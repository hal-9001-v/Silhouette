using TMPro;
using UnityEngine;

public class UICommand : MonoBehaviour
{
    [Header("HP References")]
    [SerializeField] TextMeshProUGUI _healthMesh;
    [Header("Money References")]
    [SerializeField] TextMeshProUGUI _moneyMesh;

    public void SetHealth(float health)
    {
        if (_healthMesh != null) {
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

}
