using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropagadorDeFogo : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid grid;
    [SerializeField] private float intervaloDePropagacao;
    [SerializeField, Range(0f, 1f)] private float chanceDePropagar;

    private void Start()
    {
        StartCoroutine(CicloDePropagacao());
    }

    private IEnumerator CicloDePropagacao()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervaloDePropagacao);

            List<Celula> novasCelulasEmChamas = DetectarPropagacao();

            foreach (Celula celula in novasCelulasEmChamas)
            {
                if (Random.value <= chanceDePropagar)
                {
                    celula.DefinirTipo(TipoDeCelula.Fogo);
                    celula.MudarSprite("spriteFogo");
                }
            }
        }
    }

    private List<Celula> DetectarPropagacao()
    {
        List<Celula> celulasParaPropagar = new();

        int linhas = grid.LinhasGrid;
        int colunas = grid.ColunasGrid;

        for (int i = 0; i < colunas; i++)
        {
            for (int j = 0; j < linhas; j++)
            {
                Celula celula = grid.PegarCelula(i, j);

                if (celula.tipo == TipoDeCelula.Fogo)
                {
                    List<Celula> vizinhas = PegarCelulasAdjacentes(i, j);

                    if (vizinhas.Count > 0)
                    {
                        int indice = Random.Range(0, vizinhas.Count);
                        Celula escolhida = vizinhas[indice];

                        celulasParaPropagar.Add(escolhida);
                    }
                }
            }
        }

        return celulasParaPropagar;
    }

    private List<Celula> PegarCelulasAdjacentes(int i, int j)
    {
        List<Celula> celulasAdjacentes = new();
        Vector2Int[] direcoes = {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        foreach (var dir in direcoes)
        {
            Vector2Int pos = new(i + dir.x, j + dir.y);

            if (!grid.CelulaExiste(pos)) continue;

            Celula adjacente = grid.PegarCelula(pos.x, pos.y);

            if (adjacente.tipo == TipoDeCelula.Arvore)
            {
                celulasAdjacentes.Add(adjacente);
            }
        }

        return celulasAdjacentes;
    }
}
