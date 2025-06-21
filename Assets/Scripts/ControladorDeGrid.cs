using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControladorDeGrid : MonoBehaviour
{
    [SerializeField] private Vector2 posicaoInicial;
    public Vector2 PosicaoInicial => posicaoInicial;

    [SerializeField] private int colunasGrid;
    public int ColunasGrid => colunasGrid;

    [SerializeField] private int linhasGrid;
    public int LinhasGrid => linhasGrid;

    [SerializeField] public GameObject prefabArvore;
    [SerializeField] private GameObject prefabAgua;
    [SerializeField] private GameObject prefabFogo;
    [SerializeField] private GameObject prefabArvoreQueimada;

    [SerializeField] private Vector2Int[] posicoesDeAgua;

    private Celula[,] gridArray;
    private List<Celula> fogosAtivos = new List<Celula>();
    public int pontuacao = 0;

    private void Awake()
    {
        GerarGrid();
        StartCoroutine(GeradorDeFogo());
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

                Vector2 pos = new(posicaoInicial.x + i, posicaoInicial.y + j);
                GameObject objCelula = Instantiate(prefab, pos, Quaternion.identity);
                objCelula.name = $"{tipo}({i},{j})";
                objCelula.transform.parent = transform;

                gridArray[i, j] = new Celula(new Vector2Int(i, j), tipo, objCelula);
            }
        }
    }

    private bool PosicaoDeAgua(int x, int y)
    {
        foreach (var pos in posicoesDeAgua)
        {
            if (pos.x == x && pos.y == y)
                return true;
        }
        return false;
    }

    public Celula PegarCelula(int x, int y)
    {
        if (x < 0 || x >= colunasGrid || y < 0 || y >= linhasGrid)
            return null;

        return gridArray[x, y];
    }

    private IEnumerator GeradorDeFogo()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f); // tempo entre novos focos

            if (fogosAtivos.Count < 4)
                IniciarFogoAleatorio();
        }
    }

    private void IniciarFogoAleatorio()
    {
        Vector2Int pos = PegarArvoreAleatoria();

        if (pos == Vector2Int.one * -1) return;

        Celula celula = PegarCelula(pos.x, pos.y);

        if (celula.objeto != null)
        {
            Destroy(celula.objeto);
            celula.objeto = null;
        }

        Vector3 worldPos = new(posicaoInicial.x + pos.x, posicaoInicial.y + pos.y, 0);
        GameObject fogo = Instantiate(prefabFogo, worldPos, Quaternion.identity);
        fogo.name = $"Fogo({pos.x},{pos.y})";
        fogo.transform.parent = transform;

        celula.objeto = fogo;
        celula.tipo = TipoDeCelula.Fogo;

        fogosAtivos.Add(celula);
        StartCoroutine(ContarTempoFogo(celula));
    }

    private IEnumerator ContarTempoFogo(Celula celula)
    {
        float tempo = 15f;
        while (tempo > 0f)
        {
            if (celula.tipo != TipoDeCelula.Fogo)
                yield break;

            tempo -= Time.deltaTime;
            yield return null;
        }

        // Fogo não foi apagado — queima a árvore
        QueimarArvore(celula);
    }

    private void QueimarArvore(Celula celula)
    {
        if (celula.tipo != TipoDeCelula.Fogo) return;

        if (celula.objeto != null)
            Destroy(celula.objeto);

        Vector3 pos = new(posicaoInicial.x + celula.posicao.x, posicaoInicial.y + celula.posicao.y, 0);
        GameObject queimado = Instantiate(prefabArvoreQueimada, pos, Quaternion.identity);
        queimado.name = $"Queimada({celula.posicao.x},{celula.posicao.y})";
        queimado.transform.parent = transform;

        celula.objeto = queimado;
        celula.tipo = TipoDeCelula.Arvore;

        fogosAtivos.Remove(celula);
        pontuacao--; // perdeu ponto
        Debug.Log($"🌳 A árvore queimou! Pontuação: {pontuacao}");
    }

    public bool ApagarFogoNaPosicao(Vector2Int pos)
    {
        Celula celula = PegarCelula(pos.x, pos.y);

        if (celula != null && celula.tipo == TipoDeCelula.Fogo)
        {
            if (celula.objeto != null)
                Destroy(celula.objeto);

            Vector3 worldPos = new(posicaoInicial.x + pos.x, posicaoInicial.y + pos.y, 0);
            GameObject arvore = Instantiate(prefabArvore, worldPos, Quaternion.identity);
            arvore.name = $"Arvore({pos.x},{pos.y})";
            arvore.transform.parent = transform;

            celula.objeto = arvore;
            celula.tipo = TipoDeCelula.Arvore;

            fogosAtivos.Remove(celula);
            pontuacao++; // ganhou ponto
            Debug.Log($"💧 Fogo apagado! Pontuação: {pontuacao}");
            return true;
        }

        return false;
    }

    private Vector2Int PegarArvoreAleatoria()
    {
        List<Vector2Int> livres = new();

        for (int i = 0; i < colunasGrid; i++)
        {
            for (int j = 0; j < linhasGrid; j++)
            {
                Celula c = gridArray[i, j];
                if (c.tipo == TipoDeCelula.Arvore && !posicoesDeAgua.Contains(new Vector2Int(i + 1, j + 1)))
                    livres.Add(new Vector2Int(i, j));
            }
        }

        if (livres.Count == 0)
            return Vector2Int.one * -1;

        return livres[Random.Range(0, livres.Count)];
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
                Vector3 p = new(posicaoInicial.x - 1 + pos.x, posicaoInicial.y - 1 + pos.y, 0);
                Gizmos.DrawCube(p, Vector3.one * 0.8f);
            }
        }
    }
}
