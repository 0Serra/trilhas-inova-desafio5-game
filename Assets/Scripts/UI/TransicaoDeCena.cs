using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransicaoDeCena : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    public void TransicionarParaCena(string nomeCena)
    {
        StartCoroutine(FazerTransicao(nomeCena));
    }

    private IEnumerator FazerTransicao(string nomeCena)
    {
        // Fade in
        yield return StartCoroutine(Fade(0, 1));

        // Carregar nova cena
        SceneManager.LoadScene(nomeCena);
    }

    private IEnumerator Fade(float start, float end)
    {
        float tempo = 0f;
        while (tempo < fadeDuration)
        {
            tempo += Time.deltaTime;
            float alpha = Mathf.Lerp(start, end, tempo / fadeDuration);
            fadePanel.alpha = alpha;
            yield return null;
        }

        fadePanel.alpha = end;
    }
}
