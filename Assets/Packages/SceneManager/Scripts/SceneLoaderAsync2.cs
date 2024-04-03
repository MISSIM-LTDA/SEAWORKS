using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderAsync2 : Singleton<SceneLoaderAsync2>
{

    // Loading Progress: private setter, public getter
    private float _loadingProgress;
    public float LoadingProgress { get { return _loadingProgress; } }
    public Scene Loading;

    public void LoadScene()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder());
    }

    private IEnumerator LoadScenesInOrder()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading_Screen");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene("Interacao_Equip"));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }



    public void LoadScene2()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder2());
    }

    private IEnumerator LoadScenesInOrder2()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading_Screen");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene2("Sonar_Tritech_Interact"));
    }

    private IEnumerator LoadScene2(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public void LoadScene3()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder3());
    }

    private IEnumerator LoadScenesInOrder3()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading_Screen");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene3("Menu_Loggado_SISTAC"));
    }

    private IEnumerator LoadScene3(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public void LoadScene4()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder4());
    }

    private IEnumerator LoadScenesInOrder4()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading_Screen");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene4("Falcon_Tiger_Operacao_HD"));
    }

    private IEnumerator LoadScene4(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    public void LoadScene5()
    {
        // kick-off the one co-routine to rule them all
        StartCoroutine(LoadScenesInOrder5());
    }

    private IEnumerator LoadScenesInOrder5()
    {
        // LoadSceneAsync() returns an AsyncOperation, 
        // so will only continue past this point when the Operation has finished
        yield return SceneManager.LoadSceneAsync("Loading_Screen");

        // as soon as we've finished loading the loading screen, start loading the game scene
        yield return StartCoroutine(LoadScene5("Falcon_Tiger_Interact_baked"));
    }

    private IEnumerator LoadScene5(string sceneName)
    {
        var asyncScene = SceneManager.LoadSceneAsync(sceneName);

        // this value stops the scene from displaying when it's finished loading
        asyncScene.allowSceneActivation = false;

        while (!asyncScene.isDone)
        {
            // loading bar progress
            _loadingProgress = Mathf.Clamp01(asyncScene.progress / 0.9f) * 100;

            // scene has loaded as much as possible, the last 10% can't be multi-threaded
            if (asyncScene.progress >= 0.9f)
            {
                // we finally show the scene
                asyncScene.allowSceneActivation = true;
            }

            yield return null;
        }
    }


}
