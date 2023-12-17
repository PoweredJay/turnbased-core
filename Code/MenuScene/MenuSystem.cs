using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuSystem : MonoBehaviour
{
    private AudioSource menuMusic;
    public GameObject mainMenu;
    public GameObject optionsMenu;
    GameObject lastSelect;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        menuMusic = GetComponent<AudioSource>();
        menuMusic.Play();
        menuMusic.loop = true;
        EventSystem.current.SetSelectedGameObject(mainMenu.transform.GetChild(0).gameObject);
        lastSelect = new GameObject();
    }

    void Update()
    {
        if(!optionsMenu.activeInHierarchy)
        {
            Cursor.visible = false;
        }
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelect);
        } else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
    }

    public void MainStartButton()
    {
        SceneManager.LoadScene("combatScene");
    }

    public void MainOptionsButton()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsMenu.transform.GetChild(0).gameObject);
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
        EventSystem.current.SetSelectedGameObject(mainMenu.transform.GetChild(0).gameObject);
    }
}
