using TMPro;
using UnityEngine;

public class Pontuacao : MonoBehaviour
{
    public static Pontuacao Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI textoPontuacao;

    [SerializeField] private int pontosApagarFogo = 10;
    [SerializeField] private int pontosPlantarBroto = 5;
    [SerializeField] private int pontosAcertoPergunta = 15;
    [SerializeField] private int pontosPorArvoreFinal = 2;

    private int pontuacaoAtual = 0;

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
        textoPontuacao.text = "Pontuação: " + pontuacaoAtual;
    }

    public void AdicionarPontos(int pontos)
    {
        pontuacaoAtual += pontos;
        AtualizarPontuacao();
    }

    public void PontuarApagarFogo() => AdicionarPontos(pontosApagarFogo);
    public void PontuarPlantarBroto() => AdicionarPontos(pontosPlantarBroto);
    public void PontuarAcertoPergunta() => AdicionarPontos(pontosAcertoPergunta);

    public void PontuarFinal(int arvoresSaudaveis)
    {
        AdicionarPontos(arvoresSaudaveis * pontosPorArvoreFinal);
    }

    public int PegarPontuacao()
    {
        return pontuacaoAtual;
    }
}
