using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorWaveButton : MonoBehaviour, IPointerClickHandler
{
    public int number;
    public int cost;

    public TextMeshProUGUI costText;

    public Image image1;
    public Image image2;
    public Image image3;

    public StoreController controller;

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        print("Button: " + number);

        controller.BuyColorWave(number, cost);
    }
}
