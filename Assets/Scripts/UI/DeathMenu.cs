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
    private Func<GameManager> gameManager;

    public void Init(DNDClasses playerClass, Func<SaveManager> saveMan, Func<GameManager> gameManager)
    {
        this.gameManager = gameManager;
        this.playerClass = playerClass;
        saveManager = saveMan;
        points = saveManager().LoadPoints();
    }

    //private void OnEnable()
    //{
    //    int points = gameManager().LevelPoints();
    //    int score = gameManager().pointsInGame;

    //    ShowInfo(score, points);
    //}

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
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void ShowDeadMenu()
    {
        int points = gameManager().LevelPoints();
        int score = gameManager().PointsInGame;

        ShowInfo(score, points);
    }

    public async void ShowInfo(int score, int points)
    {
        await CurrentScore(score);
        await UpdatePoints(points);

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
