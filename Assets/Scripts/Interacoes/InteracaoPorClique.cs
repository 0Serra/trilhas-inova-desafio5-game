using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InteracaoPorClique : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid grid;
    private EstadosDoJogador estados;
    private MovimentacaoDoJogador movimentacao;

    private List<Celula> celulasAdjacentes = new();
    private Celula celulaSelecionada;
    private Celula celulaInteragindo;
    private AcoesDeInteracao interacaoAtual;
    private Celula ultimaComBorda = null;
    private MostrarBolha mostrarBolha;

    private float cronometroInteracao = 0f;
    private bool mousePressionado = false;

    private void Start()
    {
        estados = GetComponent<EstadosDoJogador>();
        movimentacao = GetComponent<MovimentacaoDoJogador>();
        mostrarBolha = GetComponent<MostrarBolha>();
    }

    private void Update()
    {
        if (movimentacao.EstaMovendo()) return;

        AtualizarCelulasAdjacentes();
        DetectarCelulaComMouse();

        if (Input.GetMouseButtonDown(0))
        {
            TentarIniciarInteracao();
        }

        if (Input.GetMouseButton(0))
        {
            mousePressionado = true;
            AtualizarInteracao();
        }

        if (Input.GetMouseButtonUp(0))
        {
            CancelarInteracao();
        }
    }

    private void AtualizarCelulasAdjacentes()
    {
        celulasAdjacentes.Clear();

        Vector2Int posicao = movimentacao.PegarPosicaoAtual();
        Vector2Int[] direcoes = {
        Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right,
        new Vector2Int(1, 1), new Vector2Int(-1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1)
    };

        foreach (var dir in direcoes)
        {
            Vector2Int vizinha = posicao + dir;
            if (grid.CelulaExiste(vizinha))
            {
                Celula celula = grid.PegarCelula(vizinha.x, vizinha.y);
                celulasAdjacentes.Add(celula);
            }
        }
    }

    private void DetectarCelulaComMouse()
    {
        celulaSelecionada = null;

        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mouseWorldPos);

        if (hit != null)
        {
            foreach (var celula in celulasAdjacentes)
            {
                if (celula.objeto == hit.gameObject)
                {
                    celulaSelecionada = celula;
                    break;
                }
            }
        }

        if (ultimaComBorda != null && ultimaComBorda != celulaSelecionada)
        {
            Transform bordaAnterior = ultimaComBorda.objeto.transform.Find("Borda");
            if (bordaAnterior != null) bordaAnterior.gameObject.SetActive(false);
            ultimaComBorda = null;
        }

        if (celulaSelecionada != null)
        {
            Transform bordaNova = celulaSelecionada.objeto.transform.Find("Borda");
            if (bordaNova != null)
            {
                bordaNova.gameObject.SetActive(true);
                ultimaComBorda = celulaSelecionada;
            }
        }
    }

    private void TentarIniciarInteracao()
    {
        if (celulaSelecionada == null) return;

        interacaoAtual = PegarInteracaoDaCelula(celulaSelecionada);
        if (interacaoAtual == null || !interacaoAtual.PodeInteragir(celulaSelecionada, estados)) return;

        celulaInteragindo = celulaSelecionada;
        cronometroInteracao = 0f;
    }

    private void AtualizarInteracao()
    {
        if (!mousePressionado || celulaInteragindo == null) return;

        if (celulaInteragindo != celulaSelecionada)
        {
            CancelarInteracao();

            if (celulaSelecionada != null)
            {
                TentarIniciarInteracao();
            }

            return;
        }

        cronometroInteracao += Time.deltaTime;

        if (cronometroInteracao >= interacaoAtual.Duracao)
        {
            interacaoAtual.Interagir(celulaInteragindo, estados);
            celulaInteragindo.Destacar(false);

            if (interacaoAtual is AcaoColetarAgua)
            {
                mostrarBolha.MostrarGota(true);
            }
            if (interacaoAtual is AcaoApagarFogo)
            {
                mostrarBolha.MostrarGota(false);
            }

            Transform borda = celulaInteragindo.objeto.transform.Find("Borda");
            if (borda != null) borda.gameObject.SetActive(false);

            interacaoAtual = null;
            celulaInteragindo = null;
            mousePressionado = false;
        }
    }

    public void CancelarInteracao()
    {
        mousePressionado = false;

        if (celulaInteragindo != null)
        {
            celulaInteragindo.Destacar(false);
            Transform borda = celulaInteragindo.objeto.transform.Find("Borda");
            if (borda != null) borda.gameObject.SetActive(false);
        }

        interacaoAtual = null;
        celulaInteragindo = null;
    }

    private AcoesDeInteracao PegarInteracaoDaCelula(Celula celula)
    {
        if (celula.tipo == TipoDeCelula.Agua && !estados.TemAgua)
            return new AcaoColetarAgua();
        else if (celula.tipo == TipoDeCelula.Fogo && estados.TemAgua)
            return new AcaoApagarFogo();
        else if (celula.tipo == TipoDeCelula.Cinzas)
            return new AcaoPlantarBroto();

        return null;
    }
}
