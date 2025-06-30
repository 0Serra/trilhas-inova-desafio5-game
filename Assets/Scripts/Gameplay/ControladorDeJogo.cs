using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EstadoDoJogo
{
    Jogando,
    EmPergunta,
    EmPowerup
}

public class ControladorDeJogo : MonoBehaviour
{
    public static ControladorDeJogo Instance { get; private set; }

    [SerializeField] private FazerPergunta desafioDePergunta;

    [SerializeField] private float intervaloEntrePerguntas;

    public EstadoDoJogo estadoAtual = EstadoDoJogo.Jogando;
    private float cronometroPergunta;

    private bool jogadorTemPowerup = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        cronometroPergunta = intervaloEntrePerguntas;
    }

    private void Update()
    {
        if (estadoAtual != EstadoDoJogo.Jogando) return;

        cronometroPergunta -= Time.deltaTime;

        if (cronometroPergunta <= 0f)
        {
            IniciarPergunta();
        }
    }

    private void IniciarPergunta()
    {
        estadoAtual = EstadoDoJogo.EmPergunta;
        cronometroPergunta = intervaloEntrePerguntas;

        desafioDePergunta.MostrarPergunta(ReceberResposta);
        ControlarTempo(false);
    }

    private void ReceberResposta(bool acertou)
    {
        if (acertou)
        {
            jogadorTemPowerup = true;
            estadoAtual = EstadoDoJogo.EmPowerup;

            ControladorDePowerUp.Instance.GerarPowerupAleatorio();
        }
        else
        {
            estadoAtual = EstadoDoJogo.Jogando;
        }
    }

    public void VoltarAoJogo()
    {
        if (estadoAtual == EstadoDoJogo.EmPowerup || estadoAtual == EstadoDoJogo.Jogando)
        {
            ControlarTempo(true);
            ControladorDePowerUp.Instance.aguardandoCliqueParaAtivar = true;
            Debug.Log("PODE ATIVAR!");
        }
    }

    public bool PodeFazerInteracaoNormal()
    {
        return estadoAtual == EstadoDoJogo.Jogando;
    }

    public bool JogadorTemPowerupAtivo()
    {
        return jogadorTemPowerup;
    }

    public void UsarPowerup()
    {
        jogadorTemPowerup = false;
        estadoAtual = EstadoDoJogo.Jogando;
    }

    private void ControlarTempo(bool ativo)
    {
        Time.timeScale = ativo ? 1f : 0f;
    }
}
