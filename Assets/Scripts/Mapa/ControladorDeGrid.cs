using UnityEngine;

public class ControladorDeGrid : MonoBehaviour
{
    [SerializeField] private Vector2 posicaoInicial;
    public Vector2 PosicaoInicial => posicaoInicial;
    [SerializeField] private int colunasGrid;
    public int ColunasGrid => colunasGrid;
    [SerializeField] private int linhasGrid;
    public int LinhasGrid => linhasGrid;
    [SerializeField] private GameObject prefabArvore;
    [SerializeField] private GameObject prefabAgua;
    [SerializeField] private Vector2Int[] posicoesDeAgua;
    [SerializeField] private Vector2Int[] posicoesDeFogo;
    private Celula[,] gridArray;

    private void Awake()
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

                if (PosicaoFogoInicial(i + 1, j + 1))
                {
                    tipo = TipoDeCelula.Fogo;
                }

                Vector2 posicao = new(posicaoInicial.x + i, posicaoInicial.y + j);
                GameObject objCelula = Instantiate(prefab, posicao, Quaternion.identity);

                ControladorDeFimDeJogo.Instance.RegistrarCelula(tipo);

                Celula celula = new(new Vector2Int(i, j), tipo, objCelula);
                gridArray[i, j] = celula;

                if (PosicaoFogoInicial(i + 1, j + 1))
                {
                    celula.MudarSprite("spriteFogo");
                }

                objCelula.name = $"{tipo}({i}, {j})";
                objCelula.transform.parent = transform;
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

    private bool PosicaoFogoInicial(int x, int y)
    {
        foreach (var pos in posicoesDeFogo)
        {
            if (pos.x == x && pos.y == y)
            {
                return true;
            }
        }

        return false;
    }

    public bool CelulaExiste(Vector2Int posicao)
    {
        return posicao.x >= 0 && posicao.x < colunasGrid &&
               posicao.y >= 0 && posicao.y < linhasGrid;
    }

    public Celula PegarCelula(int x, int y)
    {
        if (x < 0 || x >= colunasGrid || y < 0 || y >= linhasGrid) return null;

        return gridArray[x, y];
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < colunasGrid; i++)
        {
            for (int j = 0; j < linhasGrid; j++)
            {
                Vector3 pos = new(posicaoInicial.x + i, posicaoInicial.y + j, 0);
                Gizmos.DrawWireCube(pos, Vector3.one);
            }
        }

        if (posicoesDeAgua != null)
        {
            Gizmos.color = Color.blue;

            foreach (var pos in posicoesDeAgua)
            {
                Vector3 position = new(posicaoInicial.x - 1 + pos.x, posicaoInicial.y - 1 + pos.y, 0);
                Gizmos.DrawCube(position, Vector3.one * 0.8f);
            }
        }

        if (posicoesDeFogo != null)
        {
            Gizmos.color = Color.red;

            foreach (var pos in posicoesDeFogo)
            {
                Vector3 position = new(posicaoInicial.x - 1 + pos.x, posicaoInicial.y - 1 + pos.y, 0);
                Gizmos.DrawCube(position, Vector3.one * 0.8f);
            }
        }
    }
}
