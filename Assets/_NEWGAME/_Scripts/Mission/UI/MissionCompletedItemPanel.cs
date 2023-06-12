using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionCompletedItemPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Button   x3Button;
    private                  float    amount;
    private                  bool     HasX3;
    public float finalAmount => amount * (HasX3 ? 3 : 1);

    public void ResetData(float amount, string x1, string x3)
    {
        this.amount = amount;
        
        HasX3 = false;
        amountText.SetText(x1);
        x3Button.gameObject.SetActive(true);
        
        x3Button.onClick.RemoveAllListeners();
        x3Button.onClick.AddListener(() =>
        {
            HasX3 = true;
            amountText.SetText(x3);
            x3Button.gameObject.SetActive(false);
        });
    }
}