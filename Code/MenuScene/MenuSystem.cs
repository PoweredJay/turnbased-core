using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    private AudioSource menuMusic;
    public GameObject mainMenu;
    public GameObject optionsMenu;

    // Start is called before the first frame update
    void Start()
    {
        menuMusic = GetComponent<AudioSource>();
        menuMusic.Play();
        menuMusic.loop = true;
    }

    public void MainStartButton()
    {
        SceneManager.LoadScene("combatScene");
    }

    public void MainOptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void MainQuitButton()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    public void OptionsBackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
