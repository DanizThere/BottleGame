using System.Threading.Tasks;
using UnityEngine;

public class MenuState : MonoBehaviour
{
    [SerializeField] private GameState gameState;
    private async void Start()
    {
        await Task.Delay(200);
        ServiceLocator.Instance.Get<GameManager>().SetGameState(gameState);
    }
}
