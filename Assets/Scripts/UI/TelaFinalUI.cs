using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class TelaFinalUI : MonoBehaviour
{
    [SerializeField] private GameObject painelVitoria;
    [SerializeField] private GameObject painelDerrota;

    [SerializeField] private TextMeshProUGUI textoPontuacaoFeita;
    [SerializeField] private TextMeshProUGUI textoBonus;
    [SerializeField] private TextMeshProUGUI textoTotal;

    [SerializeField] private TMP_InputField inputNomeJogador;
    [SerializeField] private Button botaoSalvarPontuacao;
    [SerializeField] private Transform rankingContainer;
    [SerializeField] private GameObject linhaRankingPrefab;

    [SerializeField] private Button botaoTentarNovamente;
    [SerializeField] private Button botaoMenuPrincipal;

    private void Start()
    {
        painelVitoria.SetActive(false);
        painelDerrota.SetActive(false);

        botaoTentarNovamente.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
        botaoMenuPrincipal.onClick.AddListener(() => SceneManager.LoadScene("Main Menu"));

        botaoSalvarPontuacao.onClick.AddListener(SalvarPontuacao);

        if (DadosFinaisDeJogo.Venceu)
            AtualizarRanking();
    }

    public void MostrarVitoria(int pontuacaoFeita, int bonus)
    {
        painelVitoria.SetActive(true);

        int total = pontuacaoFeita + bonus;

        textoPontuacaoFeita.text = pontuacaoFeita.ToString();
        textoBonus.text = bonus.ToString();
        textoTotal.text = total.ToString();
    }

    public void MostrarDerrota()
    {
        painelDerrota.SetActive(true);
    }

    private void SalvarPontuacao()
    {
        string nome = inputNomeJogador.text;
        if (string.IsNullOrEmpty(nome)) nome = "Jogador";

        int total = int.Parse(textoTotal.text.Trim());

        SistemaDeRanking.SalvarPontuacao(nome, total);
        AtualizarRanking();
        botaoSalvarPontuacao.interactable = false;
    }

    private void AtualizarRanking()
    {
        foreach (Transform child in rankingContainer)
            Destroy(child.gameObject);

        List<EntradaRanking> ranking = SistemaDeRanking.CarregarRanking();

        foreach (var entrada in ranking)
        {
            GameObject linha = Instantiate(linhaRankingPrefab, rankingContainer);

            TextMeshProUGUI nomeTexto = linha.transform.Find("TextoNome").GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI pontuacaoTexto = linha.transform.Find("TextoPontuacao").GetComponent<TextMeshProUGUI>();

            nomeTexto.text = entrada.nome;
            pontuacaoTexto.text = entrada.pontuacao.ToString();
        }
    }
}
