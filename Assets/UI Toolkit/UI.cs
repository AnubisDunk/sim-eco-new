using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;

public class UI : MonoBehaviour
{
    private int herbivoreCount,carnivoreCount, food,time;
    private Label LTime,LHerbivores,LCarnivores, LFood;

    private VisualElement pause;

    private bool isPaused = false;
    private void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        LHerbivores = root.Q<Label>("Herbivore");
        LCarnivores = root.Q<Label>("Carnivore");
        LTime = root.Q<Label>("Time");
        LFood = root.Q<Label>("Food");
        pause = root.Q<VisualElement>("PauseContainer");
        GlobalEventManager.OnHerbivoreBorn.AddListener(HerbivoreBorn);
        GlobalEventManager.OnTime.AddListener(TimeSim);
        GlobalEventManager.OnCarnivoreBorn.AddListener(CarnivoreBorn);
        GlobalEventManager.OnHerbivoreKilled.AddListener(HerbivoreKilled);
        GlobalEventManager.OnCarnivoreKilled.AddListener(CarnivoreKilled);
        GlobalEventManager.OnFood.AddListener(Food);
        GlobalEventManager.OnPause.AddListener(Pause);

    }
    private void HerbivoreKilled()
    {
        herbivoreCount--;
        Utils.herbivoreCount--;
        LHerbivores.text = $"Rabbits:{herbivoreCount}";
    }
    private void CarnivoreKilled()
    {
        carnivoreCount--;
        Utils.carnivoreCount--;
        LCarnivores.text = $"Foxes:{carnivoreCount}";
    }
    private void HerbivoreBorn()
    {
        herbivoreCount++;
        Utils.herbivoreCount++;
        LHerbivores.text = $"Rabbits:{herbivoreCount}";
    }
    private void CarnivoreBorn()
    {
        carnivoreCount++;
        Utils.carnivoreCount++;
        LCarnivores.text = $"Foxes:{carnivoreCount}";
    }
    private void Food()
    {
        food++;
        LFood.text = $"Food plants:{food}";
    }
     private void TimeSim()
    {
        time++;
        LTime.text = $"Time:{time}";
    }
    private void Pause()
    {
        pause.visible = !pause.visible;
        isPaused = !isPaused;
        if (isPaused) Time.timeScale = 0;
        else Time.timeScale = 1;
    }
}
