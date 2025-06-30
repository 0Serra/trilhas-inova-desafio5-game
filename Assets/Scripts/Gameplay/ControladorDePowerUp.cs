using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoDePowerup
{
    Chuva
}

public class ControladorDePowerUp : MonoBehaviour
{
    public static ControladorDePowerUp Instance { get; private set; }

    private TipoDePowerup powerupAtual;
    private bool powerupDisponivel = false;
    public bool aguardandoCliqueParaAtivar = false;
    private int cliqueDeUso = 0;

    [SerializeField] private int raioDaChuva;
    [SerializeField] private ControladorDeGrid grid;
    private List<Celula> celulasDestacadas = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void GerarPowerupAleatorio()
    {
        powerupAtual = (TipoDePowerup)Random.Range(0, System.Enum.GetValues(typeof(TipoDePowerup)).Length);
        powerupDisponivel = true;

        Debug.Log($"Power-up recebido: {powerupAtual}");
    }

    private void Update()
    {
        if (!powerupDisponivel || !ControladorDeJogo.Instance.JogadorTemPowerupAtivo()) return;

        if (!aguardandoCliqueParaAtivar) return;

        // if (!Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        //     return;
        if (Input.GetMouseButtonDown(0)) cliqueDeUso++;

        Vector3 mouseMundo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        int celulaX = Mathf.RoundToInt(mouseMundo.x - grid.PosicaoInicial.x);
        int celulaY = Mathf.RoundToInt(mouseMundo.y - grid.PosicaoInicial.y);
        Vector2Int celulaAlvo = new Vector2Int(celulaX, celulaY);

        if (grid.CelulaExiste(celulaAlvo))
        {
            MostrarAreaDaChuva(celulaAlvo);
        }
        else
        {
            EsconderAreaDaChuva();
        }

        if (Input.GetMouseButtonDown(0) && cliqueDeUso > 1)
        {
            cliqueDeUso = 0;

            if (grid.CelulaExiste(celulaAlvo))
            {
                UsarPowerup(powerupAtual, celulaAlvo);
            }
        }
    }

    private void UsarPowerup(TipoDePowerup tipo, Vector2Int centro)
    {
        Debug.Log("USOU POWERUP!");
        switch (tipo)
        {
            case TipoDePowerup.Chuva:
                AplicarChuva(centro);
                EsconderAreaDaChuva();
                break;
                // outros powerups
        }

        powerupDisponivel = false;
        aguardandoCliqueParaAtivar = false;
        ControladorDeJogo.Instance.UsarPowerup();
    }

    private void MostrarAreaDaChuva(Vector2Int centro)
    {
        // Se já está mostrando nessa mesma célula, não atualiza
        if (celulasDestacadas.Count > 0 && celulasDestacadas[0].posicao == centro) return;

        EsconderAreaDaChuva();

        for (int dx = -raioDaChuva; dx <= raioDaChuva; dx++)
        {
            for (int dy = -raioDaChuva; dy <= raioDaChuva; dy++)
            {
                Vector2Int pos = centro + new Vector2Int(dx, dy);
                if (grid.CelulaExiste(pos))
                {
                    Celula celula = grid.PegarCelula(pos.x, pos.y);
                    Transform borda = celula.objeto.transform.Find("Borda");
                    if (borda != null) borda.gameObject.SetActive(true);
                    celulasDestacadas.Add(celula);
                }
            }
        }
    }

    private void EsconderAreaDaChuva()
    {
        foreach (var celula in celulasDestacadas)
        {
            Transform borda = celula.objeto.transform.Find("Borda");
            if (borda != null) borda.gameObject.SetActive(false);
        }

        celulasDestacadas.Clear();
    }


    private void AplicarChuva(Vector2Int centro)
    {
        for (int dx = -raioDaChuva; dx <= raioDaChuva; dx++)
        {
            for (int dy = -raioDaChuva; dy <= raioDaChuva; dy++)
            {
                Vector2Int pos = centro + new Vector2Int(dx, dy);
                if (grid.CelulaExiste(pos))
                {
                    Celula celula = grid.PegarCelula(pos.x, pos.y);
                    if (celula.tipo == TipoDeCelula.Fogo)
                    {
                        celula.ApagarFogo();
                        Pontuacao.Instance.PontuarApagarFogo();
                    }
                }
            }
        }

        Debug.Log("Chuva ativada!");
    }
}
