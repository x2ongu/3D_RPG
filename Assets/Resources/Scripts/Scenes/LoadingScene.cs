using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : BaseScene
{
    [SerializeField]
    Slider m_progressBar;
    [SerializeField]
    TextMeshProUGUI m_tipDisc;

    [SerializeField]
    string[] m_discriptions;

    static string m_nextScene;

    protected override void Init()
    {
        base.Init();
        SetTipDiscription();
    }

    private void Start()
    {
        StartCoroutine(LoadSceneProcess(m_nextScene));
    }

    public override void Clear()
    {
        //throw new System.NotImplementedException();
    }

    public static void LoadNextScene(string sceneName)
    {
        m_nextScene = sceneName;
        SceneManager.LoadScene("Loading");
    }

    void SetTipDiscription()
    {
        int num = Random.Range(0, m_discriptions.Length);
        m_tipDisc.text = m_discriptions[num];
    }

    IEnumerator LoadSceneProcess(string nextScene)
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0;
        float fakeProgress = 0f;
        m_progressBar.value = 0;

        while (!op.isDone)
        {
            yield return null;

            if (fakeProgress < 0.9f || op.progress < 0.9f)
            {
                fakeProgress += Time.deltaTime * 0.5f;
                m_progressBar.value = fakeProgress;
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                m_progressBar.value = Mathf.Lerp(0.9f, 1f, timer / 2f);

                if (m_progressBar.value >= 1f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }
}