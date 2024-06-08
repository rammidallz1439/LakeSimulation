using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUiHandler : MonoBehaviour
{
  
    public void OnStartButton()
    {
        SceneManager.LoadSceneAsync(1);

    }
}
