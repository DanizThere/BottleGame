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
    private Slider slider;
    [SerializeField] TMPro.TMP_Text bestScore;
    private DNDClasses playerClass;
    private int points;

    public void Init(DNDClasses playerClass, SaveManager saveMan)
    {
        this.playerClass = playerClass;
        points = saveMan.LoadPoints();
    }

    private void OnEnable()
    {
        exit.onClick.AddListener(ToMainMenu);
        restart.onClick.AddListener(Restart);
        playerClass = GameObject.Find("Player").GetComponent<DNDClasses>();

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


    private async Task UpdatePoints(int points)
    {
        sliderGO.gameObject.SetActive(true);
        for (int i = 0; i < points; i++) {
            Debug.Log(points);
            slider.value++;
            await Task.Delay(5);
        }
    }

    private async Task CurrentScore(int signal)
    {
        for (int score = 0; score < signal; score++) {
            bestScore.text = $"��� ���������: {score}";
            await Task.Delay(40 / (int)Mathf.Log(signal));
        }
    }
}
