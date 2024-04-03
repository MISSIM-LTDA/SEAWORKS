using UnityEngine;

public class MenuFunction3 : MonoBehaviour

{

    public void Play()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene();
        Debug.Log("ueh");
    }

    public void Play2()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene2();
    }

    public void Play3()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene3();
    }

    public void Play4()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene4();
    }

    public void Play5()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene5();
    }


    public void Back()
    {
        // Synchronous version
        //SceneLoader.Instance.LoadScene();

        SceneLoaderAsync2.Instance.LoadScene3();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
