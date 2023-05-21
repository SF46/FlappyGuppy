using UnityEngine;
using UnityEngine.UI;

public class SwitchToggle : MonoBehaviour
{
    public Sprite bgActive;
    public Sprite bgInActive;
    public Sprite toggleActive;
    public Sprite toggleInActive;
   
    [SerializeField] RectTransform uiHandleRectTransform;

    Image backgroundImage, handleImage;

    Sprite backgroundDefault, handleDefault;

    Toggle toggle;

    Vector2 handlePosition;

    void Awake()
    {
        toggle = GetComponent<Toggle>();

        handlePosition = uiHandleRectTransform.anchoredPosition;

        backgroundImage = uiHandleRectTransform.parent.GetComponent<Image>();
        handleImage = uiHandleRectTransform.GetComponent<Image>();

        backgroundDefault = backgroundImage.GetComponent<Image>().sprite;
        handleDefault = handleImage.GetComponent<Image>().sprite;

        toggle.onValueChanged.AddListener(OnSwitch);

        if (toggle.isOn)
            OnSwitch(true);
    }

    void OnSwitch(bool on)
    {
        uiHandleRectTransform.anchoredPosition = on ? new Vector2(handlePosition.x * (-1),handlePosition.y) : handlePosition;
        backgroundImage.sprite = on ? bgActive : bgInActive;
        handleImage.sprite = on ? toggleActive : toggleInActive;
    }

    void OnDestroy()
    {
        toggle.onValueChanged.RemoveListener(OnSwitch);
    }
}