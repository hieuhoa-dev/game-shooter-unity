using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    [SerializeField] TMP_Text enemiesLeftText;
    [SerializeField] GameObject WinGameMenu;
    [SerializeField] GameObject Crosshair;
    int enemiesLeft = 0;

    const string ENEMIES_LEFT_STRING = "Enemies Left: ";


    StarterAssetsInputs starterAssetsInputs;

    void Awake()
    {
        starterAssetsInputs = FindFirstObjectByType<StarterAssetsInputs>();
    }
    void Start()
    {
        Crosshair.SetActive(true);
        starterAssetsInputs.SetCursorState(true);
      
    }

    public void AdjustEnemiesLeft(int amount)
    {
        enemiesLeft += amount;
        enemiesLeftText.text = ENEMIES_LEFT_STRING + enemiesLeft.ToString();

        if (enemiesLeft <= 0)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        WinGameMenu.SetActive(true);
        Crosshair.SetActive(false);
        starterAssetsInputs.SetCursorState(false);
    }

    public void RestartLevelButton()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }

    public void QuitButton()
    {
        Debug.LogWarning("Does not work in the Unity Editor!  You silly goose!");
        Application.Quit();
    }
}