using UnityEngine;
using System.Collections;

public class CrescedorDeBrotos : MonoBehaviour
{
    public static CrescedorDeBrotos Instance { get; private set; }

    [SerializeField] private float tempoParaCrescer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void ComecarCrescimento(Celula celula)
    {
        StartCoroutine(Crescer(celula));
    }

    private IEnumerator Crescer(Celula celula)
    {
        yield return new WaitForSeconds(tempoParaCrescer);

        if (celula.tipo == TipoDeCelula.Broto)
        {
            celula.DefinirTipo(TipoDeCelula.Arvore);
            celula.MudarSprite("spriteArvore");
        }
    }
}
