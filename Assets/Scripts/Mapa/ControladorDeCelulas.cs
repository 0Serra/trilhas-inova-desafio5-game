using UnityEngine;

public enum TipoDeCelula
{
    Arvore,
    Fogo,
    Agua,
    Cinzas,
    Broto
}

public class Celula
{
    public Vector2Int posicao;
    public TipoDeCelula tipo;
    public GameObject objeto;

    public Celula(Vector2Int posicao, TipoDeCelula tipo, GameObject objeto)
    {
        this.posicao = posicao;
        this.tipo = tipo;
        this.objeto = objeto;
    }

    public void Destacar(bool estado)
    {
        if (objeto == null) return;

        Transform destaque = objeto.transform.Find("Destaque");

        if (destaque != null)
        {
            destaque.gameObject.SetActive(estado);
        }
    }

    public void DefinirTipo(TipoDeCelula novoTipo)
    {
        tipo = novoTipo;
    }

    public void MudarSprite(string spriteNome)
    {
        if (objeto == null) return;

        SpriteRenderer renderer = objeto.GetComponent<SpriteRenderer>();
        SpritesArvores visual = objeto.GetComponent<SpritesArvores>();
        Animator animator = objeto.GetComponent<Animator>();

        if (spriteNome == "spriteFogo")
        {
            if (animator != null && visual != null && visual.animacaoFogoController != null)
            {
                animator.enabled = true;
                animator.runtimeAnimatorController = visual.animacaoFogoController;

                if (renderer != null)
                    renderer.sprite = null;

                // Tocar som de fogo (looping)
                TocarSomQueimando(visual);
            }
            else
            {
                Debug.LogWarning("Animator ou animacaoFogoController não encontrado no objeto.");
            }
        }
        else
        {
            if (animator != null)
                animator.enabled = false;

            if (visual != null)
            {
                // Parar som de fogo se ainda estiver tocando
                visual.PararSom();

                var campoSprite = typeof(SpritesArvores).GetField(spriteNome);
                if (campoSprite != null)
                {
                    Sprite sprite = campoSprite.GetValue(visual) as Sprite;
                    if (sprite != null && renderer != null)
                    {
                        renderer.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogWarning($"Sprite {spriteNome} não encontrado em SpritesArvores.");
                    }
                }
                else
                {
                    Debug.LogWarning($"Campo {spriteNome} não existe em SpritesArvores.");
                }
            }
        }
    }

    public void ApagarFogo()
    {
        if (tipo != TipoDeCelula.Fogo) return;

        tipo = TipoDeCelula.Cinzas;

        MudarSprite("spriteCinzas");

        // Tocar som de apagar fogo (uma vez)
        SpritesArvores visual = objeto.GetComponent<SpritesArvores>();
        if (visual != null)
        {
            visual.TocarSom(visual.somDeApagarFogo);
        }
    }

    private void TocarSomQueimando(SpritesArvores visual)
    {
        if (visual == null) return;

        AudioSource audio = objeto.GetComponent<AudioSource>();
        if (audio == null)
            audio = objeto.AddComponent<AudioSource>();

        audio.clip = visual.somDeQueimando;
        audio.loop = true;
        audio.Play();
    }
}
