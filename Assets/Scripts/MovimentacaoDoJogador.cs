using System.Collections;
using UnityEngine;

public class MovimentacaoDoJogador : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid diretorDeGrid;
    [SerializeField] private Vector2Int posicaoInicial;
    [SerializeField] private float tempoDeMovimento;

    // 🔄 Novos sprites para cada direção
    [SerializeField] private Sprite spriteCima;
    [SerializeField] private Sprite spriteBaixo;
    [SerializeField] private Sprite spriteEsquerda;
    [SerializeField] private Sprite spriteDireita;

    private SpriteRenderer spriteRenderer; // 🔄 Referência ao SpriteRenderer
    private Vector2Int posicaoAtual;
    private bool movendo = false;
    private InteracaoDoJogador interação;

    public Vector2Int PegarPosicaoAtual() => posicaoAtual;
    public bool EstaMovendo() => movendo;

    private void Start()
    {
        posicaoAtual = new(posicaoInicial.x - 1, posicaoInicial.y - 1);
        transform.position = new(diretorDeGrid.PosicaoInicial.x + posicaoAtual.x,
                                  diretorDeGrid.PosicaoInicial.y + posicaoAtual.y,
                                  0);
        interação = GetComponent<InteracaoDoJogador>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 🧩 Pega o SpriteRenderer do GameObject
    }

    void Update()
    {
        if (interação.ModoDeSelecao()) return;

        Vector2Int direcao = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direcao = Vector2Int.up;
            spriteRenderer.sprite = spriteCima; // 🔼
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direcao = Vector2Int.down;
            spriteRenderer.sprite = spriteBaixo; // 🔽
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direcao = Vector2Int.left;
            spriteRenderer.sprite = spriteEsquerda; // ◀️
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direcao = Vector2Int.right;
            spriteRenderer.sprite = spriteDireita; // ▶️
        }

        if (direcao != Vector2Int.zero)
        {
            TentarMover(direcao);
        }
    }

    IEnumerator IrParaCelula(Vector2Int posicao)
    {
        movendo = true;
        Vector3 origem = transform.position;
        Vector3 destino = new(diretorDeGrid.PosicaoInicial.x + posicao.x,
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

        if (!diretorDeGrid.CelulaExiste(posicaoAlvo)) return;

        Celula celulaAlvo = diretorDeGrid.PegarCelula(posicaoAlvo.x, posicaoAlvo.y);

        if (celulaAlvo != null && celulaAlvo.tipo == TipoDeCelula.Arvore)
        {
            StartCoroutine(IrParaCelula(posicaoAlvo));
        }
    }
}
