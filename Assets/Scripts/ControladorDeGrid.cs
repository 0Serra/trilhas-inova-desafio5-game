using UnityEngine;

public class ControladorDeGrid : MonoBehaviour
{
    [SerializeField] private int colunasGrid;
    [SerializeField] private int linhasGrid;
    [SerializeField] private GameObject prefabArvore;
    [SerializeField] private GameObject prefabAgua;
    [SerializeField] private Vector2 posicaoInicial;
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
                Vector2 posicao = new(posicaoInicial.x + i, posicaoInicial.y + j);
                GameObject objCelula = Instantiate(prefabArvore, posicao, Quaternion.identity);
                objCelula.name = $"Arvore({i}, {j})";
                objCelula.transform.parent = transform;

                Celula celula = new(new Vector2Int(i, j), TipoDeCelula.Arvore, objCelula);
                gridArray[i, j] = celula;
            }
        }
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
    }
}
