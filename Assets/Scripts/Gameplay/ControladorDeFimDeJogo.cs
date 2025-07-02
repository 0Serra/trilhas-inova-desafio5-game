using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorDeFimDeJogo : MonoBehaviour
{
    public static ControladorDeFimDeJogo Instance { get; private set; }

    [SerializeField] TransicaoDeCena transicaoDeCena;

    private int arvoresVivas = 0;
    private int brotos = 0;
    private int celulasEmChamas = 0;
    private bool jogoFinalizado = false;
    [SerializeField] ControladorDeGrid grid;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void RegistrarCelula(TipoDeCelula tipo)
    {
        switch (tipo)
        {
            case TipoDeCelula.Arvore:
                arvoresVivas++;
                break;
            case TipoDeCelula.Broto:
                brotos++;
                break;
            case TipoDeCelula.Fogo:
                celulasEmChamas++;
                break;
        }
    }

    public void AlterarTipoCelula(TipoDeCelula antigo, TipoDeCelula novo)
    {
        AtualizarContador(antigo, -1);
        AtualizarContador(novo, 1);

        // VerificarFimDeJogo();
        // VerificarCondicaoDeFim();
        StartCoroutine(VerificarCondicaoDepois());
    }

    private IEnumerator VerificarCondicaoDepois()
    {
        yield return new WaitForEndOfFrame();
        VerificarCondicaoDeFim();
    }

    private void AtualizarContador(TipoDeCelula tipo, int delta)
    {
        switch (tipo)
        {
            case TipoDeCelula.Arvore:
                arvoresVivas += delta;
                break;
            case TipoDeCelula.Broto:
                brotos += delta;
                break;
            case TipoDeCelula.Fogo:
                celulasEmChamas += delta;
                break;
        }
    }

    // private void VerificarFimDeJogo()
    // {
    //     if (jogoFinalizado) return;

    //     if (celulasEmChamas == 0)
    //     {
    //         FinalizarJogo(true);
    //     }
    //     else if (arvoresVivas <= 0 && brotos <= 0)
    //     {
    //         FinalizarJogo(false);
    //     }
    // }

    // private void FinalizarJogo(bool vitoria)
    // {
    //     jogoFinalizado = true;

    //     if (vitoria)
    //     {
    //         int arvoresRestantes = arvoresVivas;
    //         Pontuacao.Instance.PontuarFinal(arvoresRestantes);
    //         Debug.Log($"Vitória! Árvores restantes: {arvoresRestantes}");
    //     }
    //     else
    //     {
    //         Debug.Log("Derrota! Nenhuma árvore ou broto restante.");
    //     }

    //     Time.timeScale = 0f;
    // }

    private void VerificarCondicaoDeFim()
    {
        if (jogoFinalizado) return;

        if (celulasEmChamas == 0)
        {
            jogoFinalizado = true;
            Vitoria();
        }
        else if (arvoresVivas <= 0 && brotos <= 0)
        {
            jogoFinalizado = true;
            Derrota();
        }
    }

    private void Vitoria()
    {
        DadosFinaisDeJogo.Venceu = true;
        DadosFinaisDeJogo.PontuacaoDuranteOJogo = Pontuacao.Instance.PontuacaoAtual;
        DadosFinaisDeJogo.PontuacaoBonus = CalcularBonus();

        transicaoDeCena.TransicionarParaCena("TelaFinal");
    }

    private void Derrota()
    {
        DadosFinaisDeJogo.Venceu = false;
        transicaoDeCena.TransicionarParaCena("TelaFinal");
    }

    private int CalcularBonus()
    {
        int bonus = 0;

        foreach (var celula in grid.TodasAsCelulas())
        {
            if (celula.tipo == TipoDeCelula.Arvore)
                bonus += Pontuacao.Instance.PontosPorArvoreFinal;
        }
        return bonus;
    }
}

