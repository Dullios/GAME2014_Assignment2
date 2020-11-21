using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneChange;
    }

    public void ChangeScene(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void OnSceneChange(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "TitleScene":
                break;
            case "InstructionScene":
                FindObjectOfType<Button>().onClick.AddListener(delegate { ChangeScene(0); });
                break;
            case "MainGameScene":
                FindObjectOfType<Button>().onClick.AddListener(delegate { ChangeScene(3); });
                break;
            case "GameOverScene":
                Button[] sceneButtons = FindObjectsOfType<Button>();
                foreach (Button b in sceneButtons)
                {
                    if (b.gameObject.name == "Restart Button")
                        b.onClick.AddListener(delegate { ChangeScene(2); });
                    else if (b.gameObject.name == "Title Button")
                        b.onClick.AddListener(delegate { ChangeScene(0); });
                }
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
