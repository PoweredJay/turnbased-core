using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    private AudioSource menuMusic;

    public GameObject mainMenu;
    public GameObject optionsMenu;
<<<<<<< Updated upstream
=======
    public GameObject keybindsMenu;
    public GameObject audioMenu;
    GameObject lastSelect;
>>>>>>> Stashed changes

    public Slider volumeSlider;
    public Toggle musicToggle;
    public Toggle sfxToggle;

    public static float volume = 1;
    public static bool musicOn = true;
    public static bool sfxOn = true;

    void Start()
    {
        menuMusic = GetComponent<AudioSource>();
        menuMusic.Play();
        menuMusic.loop = true;
<<<<<<< Updated upstream
=======
        EventSystem.current.SetSelectedGameObject(mainMenu.transform.GetChild(0).gameObject);
        lastSelect = new GameObject();
    }

    void Update()
    {
        if (!musicOn)
        {
            menuMusic.volume = 0;
        } else
        {
            menuMusic.volume = volume;
        }

        if (EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(lastSelect);
        } else
        {
            lastSelect = EventSystem.current.currentSelectedGameObject;
        }
>>>>>>> Stashed changes
    }

    // button hell
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

    public void OptionsAudioButton()
    {
        optionsMenu.SetActive(false);
        audioMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(audioMenu.transform.GetChild(0).gameObject);
    }

    public void OptionsKeybindsButton()
    {
        optionsMenu.SetActive(false);
        keybindsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(keybindsMenu.transform.GetChild(0).gameObject);
    }

    public void OptionsBackButton()
    {
        optionsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void AudioBackButton()
    {
        audioMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsMenu.transform.GetChild(0).gameObject);
    }

    public void KeybindsBackButton()
    {
        keybindsMenu.SetActive(false);
        optionsMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(optionsMenu.transform.GetChild(0).gameObject);
    }

    // volume things
    public void VolumeSlider() => volume = volumeSlider.value;
    public void MusicToggle()
    {
        musicOn = musicToggle.isOn;
        Debug.Log("Music is " + musicToggle.isOn);
    }

    public void SFXToggle() => sfxOn = sfxToggle.isOn;
}
