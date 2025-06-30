using System.Collections;
using UnityEngine;

public class MovimentacaoDoJogador : MonoBehaviour
{
    [SerializeField] private ControladorDeGrid diretorDeGrid;
    [SerializeField] private Vector2Int posicaoInicial;
    [SerializeField] private float tempoDeMovimento;

    [Header("Sprites de Direção")]
    [SerializeField] private Sprite spriteCima;
    [SerializeField] private Sprite spriteBaixo;
    [SerializeField] private Sprite spriteEsquerda;
    [SerializeField] private Sprite spriteDireita;

    [Header("Som de Passo")]
    [SerializeField] private AudioClip somDePasso;

    private SpriteRenderer spriteRenderer;
    private Vector2Int posicaoAtual;
    private bool movendo = false;
    private InteracaoDoJogador interação;
    private AudioSource audioSource;

    public Vector2Int PegarPosicaoAtual() => posicaoAtual;
    public bool EstaMovendo() => movendo;

    private void Start()
    {
        posicaoAtual = new Vector2Int(posicaoInicial.x - 1, posicaoInicial.y - 1);
        transform.position = new Vector3(
            diretorDeGrid.PosicaoInicial.x + posicaoAtual.x,
            diretorDeGrid.PosicaoInicial.y + posicaoAtual.y,
            0
        );

        interação = GetComponent<InteracaoDoJogador>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Configura AudioSource ou adiciona se não existir
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    private void Update()
    {
        if (interação.ModoDeSelecao()) return;

        Vector2Int direcao = Vector2Int.zero;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            direcao = Vector2Int.up;
            spriteRenderer.sprite = spriteCima;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            direcao = Vector2Int.down;
            spriteRenderer.sprite = spriteBaixo;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direcao = Vector2Int.left;
            spriteRenderer.sprite = spriteEsquerda;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            direcao = Vector2Int.right;
            spriteRenderer.sprite = spriteDireita;
        }

        if (direcao != Vector2Int.zero)
        {
            TentarMover(direcao);
        }
    }

    private void TentarMover(Vector2Int direcao)
    {
        if (movendo) return;

        Vector2Int posicaoAlvo = posicaoAtual + direcao;

        if (!diretorDeGrid.CelulaExiste(posicaoAlvo)) return;

        Celula celulaAlvo = diretorDeGrid.PegarCelula(posicaoAlvo.x, posicaoAlvo.y);

        if (celulaAlvo != null && celulaAlvo.tipo == TipoDeCelula.Arvore)
        {
            // Toca o som no pitch normal
            if (somDePasso != null)
            {
                audioSource.clip = somDePasso;
                audioSource.pitch = 1f; // velocidade normal
                audioSource.Play();
                StartCoroutine(PararSomDepoisDe(tempoDeMovimento));
            }

            StartCoroutine(IrParaCelula(posicaoAlvo));
        }
    }

    private IEnumerator IrParaCelula(Vector2Int posicao)
    {
        movendo = true;

        Vector3 origem = transform.position;
        Vector3 destino = new Vector3(
            diretorDeGrid.PosicaoInicial.x + posicao.x,
            diretorDeGrid.PosicaoInicial.y + posicao.y,
            0
        );

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

    private IEnumerator PararSomDepoisDe(float duracao)
    {
        yield return new WaitForSeconds(duracao);
        audioSource.Stop();
    }
}
