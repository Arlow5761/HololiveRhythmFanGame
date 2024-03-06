using UnityEngine;
using UnityEngine.EventSystems;

// Class that handles playing Audio on buttons
public class ButtonAudio : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    [SerializeField] private string hoverAudioName;
    [SerializeField] private string clickAudioName;

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        if (clickAudioName == null) return;
        if (pointerEventData.button == PointerEventData.InputButton.Left)
        {
            //AudioHandler.instance.GetSFX(clickAudioName).PlayOneShot();
            AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", clickAudioName);
            audioSource.PlayOneShot(audioSource.clip);
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (hoverAudioName == null) return;
        AudioSource audioSource = AudioSystem.instance.GetAudio("sfx", hoverAudioName);
        audioSource.PlayOneShot(audioSource.clip);
    }
}
