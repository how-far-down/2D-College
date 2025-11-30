using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float gameTime;
    public bool gameActive;

    void Awake()
    {
        if (Instance != null && Instance != this){
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    void Start()
    {
        gameActive = true;
    }

    void Update()
    {
        if (gameActive){
            gameTime += Time.deltaTime;
            UIController.Instance.UpdateTimer(gameTime);

            if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.P))){
                Pause();
            }
        }
    }
    
    public void GameOver()
    {
        gameActive = false;
        StartCoroutine(ShowGameOverScreen());
    }

    //Displays game over screen with .5 second delay
    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(0f);
        UIController.Instance.gameOverPanel.SetActive(true);
    }

    //Loads 'Game' Scene, restarting the game
    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        if (UIController.Instance.pausePanel.activeSelf == false 
        && UIController.Instance.gameOverPanel.activeSelf == false)
        {
            UIController.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            UIController.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
