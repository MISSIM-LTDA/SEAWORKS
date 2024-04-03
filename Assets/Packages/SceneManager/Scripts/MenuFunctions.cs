using UnityEngine;

public class MenuFunctions : MonoBehaviour

{

    public void Play()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync.Instance.LoadScene();
    }

    public void Play2()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync.Instance.LoadScene2();
    }

  
    public void Back()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync.Instance.LoadScene3();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
