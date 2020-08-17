using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScreen : MonoBehaviour
{
    //float StartUpSequenceDuration;
    float TitleOffScreenYPos;
    float TitleFinalYPos = 0f;

    [SerializeField] GameObject Title;
    [SerializeField] GameObject PressKeyPrompt;
    [SerializeField] GameObject ControlsWindow;
    [SerializeField] GameObject HUDCanvas;

    GameObject playerShip;
    float playerOffScreenXPos;
    float playerInitXPos = -32f;
    
    bool canStartGame = false;
    bool displayingPressKeyPrompt = false;
    bool displayTitle = true;
    bool displayControls = false;
    bool hasClickedStart = false;
    bool startGame = false;

    bool stopAllStartSequenceFunctionality = false;

    // Start is called before the first frame update
    void Start()
    {

        BeginScreen();
    }

    public void BeginScreen()
    {
        Title.SetActive(true);
        PressKeyPrompt.SetActive(false);
        ControlsWindow.SetActive(false);

        playerShip = GameObject.FindGameObjectWithTag("Player");
        playerShip.transform.position = new Vector3(Camera.main.GetComponent<GameUtils>().CameraBounds.bottomLeftCorner.x - 10f, 8f);
        playerShip.GetComponent<PlayerController>().CanMove = false;


        TitleOffScreenYPos = Camera.main.GetComponent<GameUtils>().CameraBounds.bottomLeftCorner.y - 100f;
        Title.transform.position = new Vector3(transform.position.x, TitleOffScreenYPos, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (stopAllStartSequenceFunctionality)
        {
            return;
        }

        if (!canStartGame)
        {
            RectTransform rt = Title.GetComponent<RectTransform>();
            if (Input.anyKeyDown)
            {
                rt.localPosition = new Vector3(0, TitleFinalYPos, 0);
                canStartGame = true;
            }
            
            if (rt.localPosition.y < TitleFinalYPos)
            {
                rt.localPosition += new Vector3(0f, 70f * Time.deltaTime, 0f);
            }
            else
            {
                canStartGame = true;
            }
           
        }
        else if (canStartGame && !displayingPressKeyPrompt)
        {
            displayingPressKeyPrompt = true;
            PressKeyPrompt.SetActive(true);
        }

        else if (canStartGame && !hasClickedStart && displayingPressKeyPrompt)
        {
            if (Input.anyKeyDown)
            {
                hasClickedStart = true;
            }
        }

        else if (hasClickedStart)
        {
            if (displayTitle)
            {
                displayTitle = false;
                Title.SetActive(false);
                PressKeyPrompt.SetActive(false);
                ControlsWindow.SetActive(true);

                Camera.main.GetComponent<AudioManager>().PlaySound(Sounds.START_GAME);
                Camera.main.GetComponent<AudioManager>().IsAtStartScreen = false;
            }

            // ship position to starting pos

            // playership.EnterScene();
            if (playerShip.transform.position.x < playerInitXPos)
            {
                playerShip.transform.Translate(7.5f * Time.deltaTime, 0, 0);
            }
            else
            {
                StartCoroutine(FadeOutControls());
                HUDCanvas.SetActive(true);
                
                playerShip.GetComponent<PlayerController>().CanMove = true;
                stopAllStartSequenceFunctionality = true;

            }

        }
    }

    IEnumerator FadeOutControls()
    {
        yield return new WaitForSeconds(3f);
        GameObject.FindGameObjectWithTag("ObjectManager").GetComponent<EnemySpawner>().CanSpawn = true;
        ControlsWindow.SetActive(false);
    }
}
