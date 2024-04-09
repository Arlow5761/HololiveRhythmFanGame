using UnityEngine;

public class TestCharacter : CharacterInterface
{
    public void Setup()
    {
        Debug.Log("Test Character Setup Message");
        ScoreManager.instance.scorePipeline.AddStep(new(1, (int score) => { Debug.Log("TestCharacter Score input test"); return score; }));
        ScoreManager.instance.comboPipeline.AddStep(new(1, (int combo) => { Debug.Log("TestCharacter combo change test"); return combo; }));
    }
}