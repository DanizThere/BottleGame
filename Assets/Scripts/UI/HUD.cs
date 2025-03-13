using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{

    [SerializeField] private GameObject Hud;
    [SerializeField] private GameObject Heal;
    [SerializeField] private GameObject Damage;

    private GameManager gameManager;

    private void Awake()
    {
        Hud = GameObject.Find("HUD");
    }

    private void Start()
    {
    }

    public async void HealAction()
    {
        Heal.SetActive(true);
        await Awaitable.WaitForSecondsAsync(.5f);
        Heal.SetActive(false);
    }
    public async void DamageAction()
    {
        Damage.SetActive(true);
        await Awaitable.WaitForSecondsAsync(.5f);
        Damage.SetActive(false);
    }
}
