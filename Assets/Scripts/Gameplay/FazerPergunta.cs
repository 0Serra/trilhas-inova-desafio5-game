using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FazerPergunta : MonoBehaviour
{
    [SerializeField] private GameObject painelPergunta;
    [SerializeField] private TextMeshProUGUI textoPergunta;
    [SerializeField] private Button botaoA;
    [SerializeField] private Button botaoB;
    [SerializeField] private TextMeshProUGUI textoBotaoA;
    [SerializeField] private TextMeshProUGUI textoBotaoB;
    [SerializeField] private TextMeshProUGUI textoTimer;

    [SerializeField] private GameObject painelResultado;
    [SerializeField] private TextMeshProUGUI textoResultado;

    [SerializeField] private BancoDePerguntas bancoDePerguntas;
    [SerializeField] private float tempoParaResponder;

    private Pergunta perguntaAtual;
    private Action<bool> callbackResposta;
    private bool aguardandoCliqueParaContinuar = false;

    private void Start()
    {
        painelPergunta.SetActive(false);
        painelResultado.SetActive(false);
    }

    private void Update()
    {
        if (aguardandoCliqueParaContinuar && Input.GetMouseButtonDown(0))
        {
            painelResultado.SetActive(false);
            aguardandoCliqueParaContinuar = false;

            ControladorDeJogo.Instance.VoltarAoJogo();
        }
    }

    public void MostrarPergunta(Action<bool> onResposta)
    {
        callbackResposta = onResposta;

        perguntaAtual = bancoDePerguntas.perguntas[UnityEngine.Random.Range(0, bancoDePerguntas.perguntas.Count)];
        painelPergunta.SetActive(true);
        painelResultado.SetActive(false);
        aguardandoCliqueParaContinuar = false;

        textoPergunta.text = perguntaAtual.enunciado;
        textoBotaoA.text = perguntaAtual.alternativaA;
        textoBotaoB.text = perguntaAtual.alternativaB;

        botaoA.onClick.RemoveAllListeners();
        botaoB.onClick.RemoveAllListeners();

        botaoA.onClick.AddListener(() => SelecionarResposta(0));
        botaoB.onClick.AddListener(() => SelecionarResposta(1));

        StartCoroutine(CronometroDeResposta());
    }

    private IEnumerator CronometroDeResposta()
    {
        float tempoRestante = tempoParaResponder;

        while (tempoRestante > 0f)
        {
            textoTimer.text = $"Tempo: {(int)tempoRestante}s";
            tempoRestante -= Time.unscaledDeltaTime;
            yield return null;

            if (aguardandoCliqueParaContinuar) yield break;
        }

        SelecionarResposta(-1);
    }

    private void SelecionarResposta(int respostaEscolhida)
    {
        StopAllCoroutines();
        painelPergunta.SetActive(false);
        painelResultado.SetActive(true);
        aguardandoCliqueParaContinuar = true;

        bool acertou = (respostaEscolhida == perguntaAtual.indiceCorreto);
        if (respostaEscolhida == -1)
        {
            textoResultado.text = "TEMPO ESGOTADO!\n\n\nClique para continuar";
        }
        else if (acertou)
        {
            textoResultado.text = "ACERTOU!\n\nPower-up desbloqueado!\n\n\nClique para continuar";
        }
        else
        {
            textoResultado.text = "ERROU!\n\n\nClique para continuar";
        }

        callbackResposta?.Invoke(acertou);
    }
}
