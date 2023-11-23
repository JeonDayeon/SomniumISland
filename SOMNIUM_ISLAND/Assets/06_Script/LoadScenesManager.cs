using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMiniGame1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadMiniGame2()
    {
        SceneManager.LoadScene(2);
    }
    public void LoadMiniGame3()
    {
        SceneManager.LoadScene(3);
    }
    public void LoadMiniGame4()
    {
        SceneManager.LoadScene(4);
    }

    public void LoadPlaza()
    {
        SceneManager.LoadScene("Plaza");
    }
}
