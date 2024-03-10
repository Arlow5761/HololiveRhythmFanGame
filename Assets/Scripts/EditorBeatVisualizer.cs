using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Displays beat lines in the editor
public class EditorBeatVisualizer : MonoBehaviour
{
    [SerializeField] private Sprite lineSprite;
    private List<SpriteRenderer> lines = new();

    void Update()
    {
        int usedLineCount = 0;
        double timestamp = EditorTimeController.instance.GetNextStep(GetTimestamp(GameplayLayout.noteDespawnX));

        while (timestamp < Song.GetAudioSourceTime() + Song.Instance.noteTime)
        {
            if (usedLineCount >= lines.Count)
            {
                GameObject newLineObject = new("BeatLine", typeof(SpriteRenderer));
                SpriteRenderer newLine = newLineObject.GetComponent<SpriteRenderer>();
                newLineObject.transform.localScale = new(0.05f, 20, 1);
                newLine.sprite = lineSprite;
                newLine.color = new(0.7f, 1f, 0.7f, 0.1f);
                newLine.sortingOrder = -4;

                lines.Add(newLine);
            }

            lines[usedLineCount].transform.position = new(GetXPosition(timestamp), 0);
            lines[usedLineCount].gameObject.SetActive(true);

            timestamp = EditorTimeController.instance.GetNextStep(timestamp);

            usedLineCount++;
        }

        for (int lineRemainder = usedLineCount - 1; lineRemainder < lines.Count; lineRemainder++)
        {
            lines[lineRemainder].gameObject.SetActive(false);
        }
    }

    private double GetTimestamp(float xPos)
    {
        double relativeDistance = (double) (xPos - GameplayLayout.hitPosX) / (GameplayLayout.noteSpawnX - GameplayLayout.hitPosX);
        double relativeTime = relativeDistance * Song.Instance.noteTime;
        double timestamp = Song.GetAudioSourceTime() + relativeTime;

        return timestamp;
    }

    private float GetXPosition(double timestamp)
    {
        double relativeTime = timestamp - Song.GetAudioSourceTime();
        double relativeDistance = relativeTime / Song.Instance.noteTime;
        float xPos = (float) (relativeDistance * (GameplayLayout.noteSpawnX - GameplayLayout.hitPosX) + GameplayLayout.hitPosX);

        return xPos;
    }
}
