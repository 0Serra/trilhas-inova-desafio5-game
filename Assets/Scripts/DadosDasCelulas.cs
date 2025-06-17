using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoDeCelula
{
    Arvore,
    Fogo,
    Agua
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

    public void DefinirTipo(TipoDeCelula novoTipo)
    {
        tipo = novoTipo;
    }
}