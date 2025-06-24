using System.Collections.Generic;
using UnityEngine;

public class InteracaoDoJogador : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid grid;
    private MovimentacaoDoJogador movimentacao;
    private bool selecionando = false;
    private List<Celula> celulasDestacadas = new();
    private int indiceSelecionado = 0;
    private Vector2Int? ultimoIndiceSelecionado = null;
    private Vector2Int[] direcoes = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
            };

    public bool ModoDeSelecao() => selecionando;

    private void Start()
    {
        movimentacao = GetComponent<MovimentacaoDoJogador>();
    }

    private void Update()
    {
        if (movimentacao.EstaMovendo()) return;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        {
            EntrarNoModoSelecao();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.RightShift))
        {
            SairDoModoSelecao();
        }

        if (selecionando)
        {
            MoverSelecaoDeCelula();
        }
    }

    private void EntrarNoModoSelecao()
    {
        if (selecionando) return;

        selecionando = true;
        celulasDestacadas.Clear();

        Vector2Int posicaoAtual = movimentacao.PegarPosicaoAtual();

        foreach (var direcao in direcoes)
        {
            Vector2Int posicaoDaCelula = posicaoAtual + direcao;

            if (!grid.CelulaExiste(posicaoDaCelula)) continue;

            Celula celula = grid.PegarCelula(posicaoDaCelula.x, posicaoDaCelula.y);
            celula.Destacar(true);
            celulasDestacadas.Add(celula);
        }

        // indiceSelecionado = 0;

        if (ultimoIndiceSelecionado.HasValue)
        {
            for (int i = 0; i < celulasDestacadas.Count; i++)
            {
                if (celulasDestacadas[i].posicao == ultimoIndiceSelecionado.Value)
                {
                    indiceSelecionado = i;
                    break;
                }
            }
        }

        AtualizarBordaDeCelula();
    }

    private void SairDoModoSelecao()
    {
        if (!selecionando) return;

        foreach (var celula in celulasDestacadas)
        {
            Transform borda = celula.objeto.transform.Find("Borda");
            borda.gameObject.SetActive(false);
            celula.Destacar(false);
        }

        if (celulasDestacadas.Count > indiceSelecionado)
        {
            ultimoIndiceSelecionado = celulasDestacadas[indiceSelecionado].posicao;
        }
        else
        {
            ultimoIndiceSelecionado = null;
        }

        celulasDestacadas.Clear();
        selecionando = false;
    }

    private void AtualizarBordaDeCelula()
    {
        for (int i = 0; i < celulasDestacadas.Count; i++)
        {
            Transform borda = celulasDestacadas[i].objeto.transform.Find("Borda");

            borda.gameObject.SetActive(i == indiceSelecionado);
        }
    }

    private void MoverSelecaoDeCelula()
    {
        Vector2Int direcaoSelecionada = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) direcaoSelecionada = Vector2Int.up;
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) direcaoSelecionada = Vector2Int.down;
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) direcaoSelecionada = Vector2Int.left;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) direcaoSelecionada = Vector2Int.right;

        if (direcaoSelecionada == Vector2Int.zero) return;

        Vector2Int posicaoAtual = movimentacao.PegarPosicaoAtual();
        Vector2Int posicaoAlvo = posicaoAtual + direcaoSelecionada;

        for (int i = 0; i < celulasDestacadas.Count; i++)
        {
            if (celulasDestacadas[i].posicao == posicaoAlvo)
            {
                indiceSelecionado = i;
                AtualizarBordaDeCelula();
                return;
            }
        }
    }
}
