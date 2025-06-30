using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Banco de Perguntas")]
public class BancoDePerguntas : ScriptableObject
{
    public List<Pergunta> perguntas;
}

[System.Serializable]
public class Pergunta
{
    public string enunciado;
    public string alternativaA;
    public string alternativaB;
    public int indiceCorreto; // 0 para A, 1 para B
}
