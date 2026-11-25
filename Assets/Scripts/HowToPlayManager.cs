using UnityEngine;
using UnityEngine.UI;

public class HowToPlayManager : MonoBehaviour
{
    [SerializeField] private Sprite[] images;
    [SerializeField] private Image screen;
    [SerializeField] private Button next_Button;
    [SerializeField] private Button previous_Button;
    private int showIndex = 0;

    private void Start()
    {
        ShowImage();
        next_Button.onClick.AddListener(NextImage);
        previous_Button.onClick.AddListener(PreviousImage);
    }

    public void ShowImage()
    {
        screen.sprite = images[0];
        showIndex = 0;
    }

    public void NextImage()
    {
        showIndex++;
        screen.sprite = images[showIndex];
        if (showIndex >= images.Length - 1)
        {
            next_Button.interactable = false;
        }
        previous_Button.interactable = true;
    }

    public void PreviousImage()
    {
        showIndex--;
        screen.sprite = images[showIndex];
        if (showIndex <= 0)
        {
            previous_Button.interactable = false;
        }
        next_Button.interactable = true;
    }
}