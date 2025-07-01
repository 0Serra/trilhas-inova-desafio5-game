using TMPro;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    public static Pontuacao Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textoPontuacao;

    [SerializeField] private int pontosApagarFogo;
    [SerializeField] private int pontosPlantarBroto;
    [SerializeField] private int pontosAcertoPergunta;
    public int PontosPorArvoreFinal;

    public int PontuacaoAtual { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        AtualizarPontuacao();
    }

    private void AtualizarPontuacao()
    {
        textoPontuacao.text = "Pontuação: " + PontuacaoAtual;
    }

    public void AdicionarPontos(int pontos)
    {
        PontuacaoAtual += pontos;
        AtualizarPontuacao();
    }

    public void PontuarApagarFogo() => AdicionarPontos(pontosApagarFogo);
    public void PontuarPlantarBroto() => AdicionarPontos(pontosPlantarBroto);
    public void PontuarAcertoPergunta() => AdicionarPontos(pontosAcertoPergunta);

    public void PontuarFinal(int arvoresSaudaveis)
    {
        AdicionarPontos(arvoresSaudaveis * PontosPorArvoreFinal);
    }

    public int PegarPontuacao()
    {
        return PontuacaoAtual;
    }
}
