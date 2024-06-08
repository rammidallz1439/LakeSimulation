using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimulationUiHandler : MonoBehaviour
{
    [Header("UI")]
    public GameObject Pause;
    public GameObject Play;
    public GameObject SprintIcon;
    public GameObject CameraViewIcon;
    public Slider RippleIntensitySlider;
    public Slider RippleScaleSlider;


    [Header("Flags")]
    public bool IsPaused;

    [Header("GameObjects")]
    public GameObject ExplorePlayer;
    public GameObject SimulationView;

    public void OnPlayButton()
    {
        Pause.SetActive(true);
        IsPaused = false;
        Time.timeScale = 1;
        Play.SetActive(false);
    }

    public void OnPauseButton()
    {
        Play.SetActive(true);
        IsPaused = true;
        Time.timeScale = 0;
        Pause.SetActive(false);
    }

    public void OnSprintButton()
    {
        ExplorePlayer.SetActive(true);
        SprintIcon.SetActive(false);
        CameraViewIcon.SetActive(true);
        SimulationView.SetActive(false);
    }

    public void OnCameraViewButton()
    {
        ExplorePlayer.SetActive(false);
        SprintIcon.SetActive(true);
        CameraViewIcon.SetActive(false);
        SimulationView.SetActive(true);
    }

    public void OnCloseButton()
    {
        Application.Quit();
    }

    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
