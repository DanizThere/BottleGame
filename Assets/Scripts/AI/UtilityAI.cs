using UnityEngine;

public class UtilityAI : MonoBehaviour
{
    [SerializeField] private EnemyAction[] actions;

    private EnemyDecide utility;
    private Enemy enemy;
    private EnemyAction currentAction;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        utility = new EnemyDecide(enemy);
    }


    private void Start()
    {
        Runtime(utility, enemy);
    }

    private async void Runtime(EnemyDecide utility, Enemy enemy)
    {
        do
        {
            Debug.Log("New iteration");

            await Awaitable.WaitForSecondsAsync(2);
            Debug.Log("Decide to...");

            currentAction = utility.DecideAction(actions);
            currentAction.Action(enemy);
        } while (true);
    }
}
