using System.Collections.Generic;
using UnityEngine;

public class InteracaoDoJogador : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid grid;
    private MovimentacaoDoJogador movimentacao;
    private bool selecionando = false;
    private List<Celula> celulasDestacadas = new();

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
    }

    private void EntrarNoModoSelecao()
    {
        if (selecionando) return;

        selecionando = true;
        celulasDestacadas.Clear();

        Vector2Int posicaoAtual = movimentacao.PegarPosicaoAtual();

        Vector2Int[] direcoes = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var direcao in direcoes)
        {
            Vector2Int posicaoDaCelula = posicaoAtual + direcao;

            if (!grid.CelulaExiste(posicaoDaCelula)) continue;

            Celula celula = grid.PegarCelula(posicaoDaCelula.x, posicaoDaCelula.y);
            celula.Destacar(true);
            celulasDestacadas.Add(celula);
        }
    }

    private void SairDoModoSelecao()
    {
        if (!selecionando) return;

        foreach (var celula in celulasDestacadas)
        {
            celula.Destacar(false);
        }

        celulasDestacadas.Clear();
        selecionando = false;
    }
}
