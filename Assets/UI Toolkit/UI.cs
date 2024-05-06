using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private int creatureCount, food;
    private Label LCreatures, LFood;

    private VisualElement pause;

    private bool isPaused = false;
    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        LCreatures = root.Q<Label>("Creatures");
        LFood = root.Q<Label>("Food");
        pause = root.Q<VisualElement>("PauseContainer");
        GlobalEventManager.OnCreatureBorn.AddListener(CreatureBorn);
        GlobalEventManager.OnCreatureKilled.AddListener(CreatureKilled);
        GlobalEventManager.OnFood.AddListener(Food);
        GlobalEventManager.OnPause.AddListener(Pause);

    }
    private void CreatureKilled()
    {
        creatureCount--;
        Utils.creatureCount--;
        LCreatures.text = $"Creatures:{creatureCount}";
    }
    private void CreatureBorn()
    {
        creatureCount++;
        Utils.creatureCount++;
        LCreatures.text = $"Creatures:{creatureCount}";
    }
    private void Food()
    {
        food++;
        LFood.text = $"Food plants:{food}";
    }
    private void Pause()
    {
        pause.visible = !pause.visible;
        isPaused = !isPaused;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
