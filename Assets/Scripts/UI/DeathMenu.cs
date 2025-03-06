using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathMenu : MonoBehaviour
{
    [SerializeField] private Button exit;
    [SerializeField] private Button restart;
    [SerializeField] private GameObject sliderGO;
    [SerializeField] private GameObject buttonsGO;
    [SerializeField] TMPro.TMP_Text bestScore;
    private Slider slider;
    private DNDClasses playerClass;
    private int points;
    private Func<SaveManager> saveManager;

    public void Init(DNDClasses playerClass, Func<SaveManager> saveMan)
    {
        this.playerClass = playerClass;
        saveManager = saveMan;
        points = saveManager().LoadPoints();
    }

    private void Start()
    {
        exit.onClick.AddListener(ToMainMenu);
        restart.onClick.AddListener(Restart);

        slider = sliderGO.GetComponentInChildren<Slider>();
        slider.maxValue = playerClass.pointsToNextLevel;
        slider.value = points;

        buttonsGO.SetActive(false);
        sliderGO.SetActive(false);
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Restart()
    {
        ServiceLocator.Instance.Get<EventBus>().Invoke(new UnsubscibeSignal());

        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public async void ShowInfo(DeathSignal signal)
    {
        await CurrentScore(signal.Score);
        await UpdatePoints(signal.Points);

        buttonsGO.SetActive(true);
    }


    private async Awaitable UpdatePoints(int points)
    {
        sliderGO.gameObject.SetActive(true);
        for (int i = 0; i < points; i++) {
            Debug.Log(points);
            slider.value++;
            await Awaitable.WaitForSecondsAsync(.5f);
        }
    }

    private async Awaitable CurrentScore(int signal)
    {
        for (int score = 0; score < signal; score++) {
            bestScore.text = $"Ваш результат: {score}";
            await Awaitable.WaitForSecondsAsync(0.2f / (int)Mathf.Log(signal));
        }
    }
}
