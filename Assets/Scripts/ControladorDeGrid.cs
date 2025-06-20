using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UIElements;

public class ControladorDeGrid : MonoBehaviour
{
    [SerializeField] private int colunasGrid;
    [SerializeField] private int linhasGrid;
    [SerializeField] private GameObject prefabArvore;
    [SerializeField] private GameObject prefabAgua;
    [SerializeField] private Vector2 posicaoInicial;
    [SerializeField] private Vector2Int[] posicoesDeAgua;
    private Celula[,] gridArray;

    private void Start()
    {
        GerarGrid();
    }

    private void GerarGrid()
    {
        gridArray = new Celula[colunasGrid, linhasGrid];

        for (int i = 0; i < colunasGrid; i++)
        {
            for (int j = 0; j < linhasGrid; j++)
            {
                TipoDeCelula tipo = TipoDeCelula.Arvore;
                GameObject prefab = prefabArvore;

                if (PosicaoDeAgua(i + 1, j + 1))
                {
                    tipo = TipoDeCelula.Agua;
                    prefab = prefabAgua;
                }

                Vector2 posicao = new(posicaoInicial.x + i, posicaoInicial.y + j);
                GameObject objCelula = Instantiate(prefab, posicao, Quaternion.identity);
                objCelula.name = $"{tipo}({i}, {j})";
                objCelula.transform.parent = transform;

                Celula celula = new(new Vector2Int(i, j), tipo, objCelula);
                gridArray[i, j] = celula;
            }
        }
    }

    private bool PosicaoDeAgua(int x, int y)
    {
        foreach (var pos in posicoesDeAgua)
        {
            if (pos.x == x && pos.y == y)
            {
                return true;
            }
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < colunasGrid; i++)
        {
            for (int j = 0; j < linhasGrid; j++)
            {
                Vector3 pos = new Vector3(posicaoInicial.x + i, posicaoInicial.y + j, 0);
                Gizmos.DrawWireCube(pos, Vector3.one);
            }
        }

        if (posicoesDeAgua != null)
        {
            Gizmos.color = Color.blue;

            foreach (var pos in posicoesDeAgua)
            {
                Vector3 position = new(posicaoInicial.x + pos.x, posicaoInicial.y + pos.y, 0);
                Gizmos.DrawCube(position, Vector3.one * 0.8f);
            }
        }
    }
}
