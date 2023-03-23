using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public FloatUnityEvent OnLoadPercentChanged => onLoadPercentChanged;

    [SerializeField] private string sceneToLoad;
    [SerializeField] private FloatUnityEvent onLoadPercentChanged;
    private YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();

    public void LoadScene(string name)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(name);
        StartCoroutine(LoadingScreen(op));
    }

    public void LoadScene() => LoadScene(sceneToLoad);

    private IEnumerator LoadingScreen(AsyncOperation op)
    {
        float lastPercent = 0.0f;
        while(!op.isDone)
        {
            if(op.progress > lastPercent)
            {
                lastPercent = op.progress;
                onLoadPercentChanged.Invoke(lastPercent);
            }

            yield return waitForEndOfFrame;
        }
    }
}
