using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private string hoverAudioName;
    [SerializeField] private string clickAudioName;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (clickAudioName == null) return;
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            AudioHandler.instance.GetSFX(clickAudioName).PlayOneShot();
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (hoverAudioName == null) return;
        AudioHandler.instance.GetSFX(hoverAudioName).PlayOneShot();
    }
}
