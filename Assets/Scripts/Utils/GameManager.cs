using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    float score = 0f;
    [SerializeField] Text scoreText;

    int lives = 3;
    [SerializeField] GameObject[] livesSprites;

    int bombs = 1;
    [SerializeField] GameObject[] bombSprites;

    [SerializeField] GameObject pauseScreen;
    bool paused = false;
    bool atHelpScreen = false;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text gameOverScore;

    float spawnLevelThreshold = 100f;
    float spawnDropMultiplier = 1;

    public float SpawnDropMultiplier { get { return spawnDropMultiplier; } }

    AudioManager audioManager;

    public bool Paused
    {
        get { return paused; }
    }

    [SerializeField] GameObject helpScreen;

    bool atStartMenu = false;
    public bool AtStartMenu
    {
        get { return atStartMenu; }
    }

    bool atGameOverMenu = false;
    public bool AtGameOverMenu
    {
        get { return atGameOverMenu; }
    }

    public int Bombs
    {
        get { return bombs; }
    }


    // Start is called before the first frame update
    void Awake()
    {
        audioManager = Camera.main.GetComponent<AudioManager>();
        scoreText.text = "0";

        pauseScreen.SetActive(false);

        int i = 0;
        foreach (GameObject l in livesSprites)
        {
            if (i < lives)
            {
                l.SetActive(true);
            }
            else
            {
                l.SetActive(false);
            }

            i++;
        }

        i = 0;
        foreach (GameObject b in bombSprites)
        {
            if (i < bombs)
            {
                b.SetActive(true);
            }
            else
            {
                b.SetActive(false);
            }

            i++;
        }

        //atStartMenu = true;
    }

    private void Update()
    {
        if (atStartMenu || atGameOverMenu)
        {
            return;
            // start game sequence
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void AddScore(float amt)
    {
        score += amt;
        scoreText.text = Mathf.RoundToInt(score).ToString();

        if (score > spawnLevelThreshold)
        {
            // make spawner spawn more frequently
            EnemySpawner spawner = GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<EnemySpawner>();
            spawner.SpawnTimeAmount = Mathf.Clamp(spawner.SpawnTimeAmount - (spawner.SpawnTimeAmount / 4f), 0.1f, 100f);

            // Display increase in difficulty (through visuals and sound queues)

            // exponentially increase new threshold
            spawnLevelThreshold *= 3f;
            spawnDropMultiplier = Mathf.Clamp(spawnDropMultiplier * .92f, 0.5f, 1f);
        }
    }

    public void AddLife()
    {
        lives = Mathf.Clamp(lives + 1, 0, 5);

        livesSprites[lives - 1].SetActive(true);
    }

    public void LoseLife()
    {
        if (lives <= 0)
        {
            Debug.Log("GAME OVER");
            // trigger GameOver
            atGameOverMenu = true;
            GoToGameOver();
        }
        else
        {
            livesSprites[lives - 1].SetActive(false);
            lives--;
        }
    }

    public void AddBomb()
    {
        bombs = Mathf.Clamp(bombs + 1, 0, 5);
        bombSprites[bombs - 1].SetActive(true);
    }

    public void LoseBomb()
    {
        if (bombs == 0)
        {
            return;
        }

        bombSprites[bombs - 1].SetActive(false);
        bombs--;
    }

    public void PauseGame()
    {
        if (!paused && !atHelpScreen)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanMove = false;

            audioManager.PlaySound(Sounds.PAUSE);
            audioManager.IsPaused = true;

            pauseScreen.SetActive(true);
            paused = true;
            Time.timeScale = 0;
        }
        else
        {
            UnpauseGame();
        }

    }

    public void UnpauseGame()
    {
        if (paused && !atHelpScreen)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CanMove = true;

            audioManager.PlaySound(Sounds.UNPAUSE);
            audioManager.IsPaused = false;

            pauseScreen.SetActive(false);
            paused = false;
            Time.timeScale = 1;
        }

        else
        {
            return;
        }
    }

    public void GoToHelpMenu()
    {
        audioManager.PlaySound(Sounds.CLICK);

        atHelpScreen = true;
        pauseScreen.SetActive(false);
        Time.timeScale = 0;

        helpScreen.SetActive(true);
    }

    public void ReturnFromHelpMenu()
    {
        audioManager.PlaySound(Sounds.CLICK_BACK);
        atHelpScreen = false;
        helpScreen.SetActive(false);
        pauseScreen.SetActive(true);
    }

    public void ExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #elif UNITY_STANDALONE
            Application.Quit();
        #elif UNITY_WEBGL
            Application.OpenURL("about:blank");
        #endif
    }

    public void GoToGameOver()
    {
        gameOverScore.text = "Final Score: " + Mathf.RoundToInt(score).ToString();
        //scoreText.gameObject.SetActive(false);
        gameOverScreen.SetActive(true);
        Camera.main.GetComponent<ScreenShake>().StopShake();
        Time.timeScale = 0;

    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }


    //public void DisplayPowerUp()
}
