using System.Collections;
using UnityEngine;

public class MovimentacaoDoJogador : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid diretorDeGrid;
    [SerializeField] private Vector2Int posicaoInicial;
    [SerializeField] private float tempoDeMovimento;

    private Vector2Int posicaoAtual;
    private bool movendo = false;
    private bool temAgua = false;

    private void Start()
    {
        posicaoAtual = new Vector2Int(posicaoInicial.x - 1, posicaoInicial.y - 1);
        transform.position = new Vector3(
            diretorDeGrid.PosicaoInicial.x + posicaoAtual.x,
            diretorDeGrid.PosicaoInicial.y + posicaoAtual.y,
            0);
    }

    void Update()
    {
        Vector2Int direcao = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            direcao = Vector2Int.up;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            direcao = Vector2Int.down;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            direcao = Vector2Int.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            direcao = Vector2Int.right;

        if (direcao != Vector2Int.zero)
        {
            TentarMover(direcao);
        }
    }

    IEnumerator IrParaCelula(Vector2Int posicao)
    {
        movendo = true;
        Vector3 origem = transform.position;
        Vector3 destino = new Vector3(
            diretorDeGrid.PosicaoInicial.x + posicao.x,
            diretorDeGrid.PosicaoInicial.y + posicao.y,
            0);
        float cronometro = 0f;

        while (cronometro < tempoDeMovimento)
        {
            cronometro += Time.deltaTime;
            transform.position = Vector3.Lerp(origem, destino, cronometro / tempoDeMovimento);
            yield return null;
        }

        transform.position = destino;
        posicaoAtual = posicao;
        movendo = false;
    }

    private void TentarMover(Vector2Int direcao)
    {
        if (movendo) return;

        Vector2Int posicaoAlvo = posicaoAtual + direcao;

        if (!CelulaExiste(posicaoAlvo)) return;

        Celula celulaAlvo = diretorDeGrid.PegarCelula(posicaoAlvo.x, posicaoAlvo.y);

        if (celulaAlvo != null)
        {
            if (celulaAlvo.tipo == TipoDeCelula.Arvore)
            {
                StartCoroutine(IrParaCelula(posicaoAlvo));
            }
            else if (celulaAlvo.tipo == TipoDeCelula.Agua)
            {
                if (!temAgua)
                {
                    temAgua = true;
                    Debug.Log("Você pegou água!");
                    StartCoroutine(IrParaCelula(posicaoAlvo));
                }
                else
                {
                    Debug.Log("Você já está carregando água!");
                }
            }
            else if (celulaAlvo.tipo == TipoDeCelula.Fogo)
            {
                if (temAgua)
                {
                    bool apagado = diretorDeGrid.ApagarFogoNaPosicao(posicaoAlvo);

                    if (apagado)
                    {
                        temAgua = false;
                        StartCoroutine(IrParaCelula(posicaoAlvo));
                    }
                }
                else
                {
                    Debug.Log("🔥 Você precisa de água para apagar o fogo!");
                }
            }
        }
    }

    private bool CelulaExiste(Vector2Int posicao)
    {
        return posicao.x >= 0 && posicao.x < diretorDeGrid.ColunasGrid &&
               posicao.y >= 0 && posicao.y < diretorDeGrid.LinhasGrid;
    }
}
