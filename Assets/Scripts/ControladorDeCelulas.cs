using UnityEngine;

public enum TipoDeCelula
{
    Arvore,
    Fogo,
    Agua,
    Cinzas
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
        SpriteRenderer renderer = objeto.GetComponent<SpriteRenderer>();

        SpritesArvores visual = objeto.GetComponent<SpritesArvores>();

        Sprite sprite = typeof(SpritesArvores).GetField(spriteNome).GetValue(visual) as Sprite;

        renderer.sprite = sprite;
    }

    public void ApagarFogo()
    {
        if (tipo != TipoDeCelula.Fogo) return;

        tipo = TipoDeCelula.Cinzas;

        MudarSprite("spriteCinzas");
    }
}
