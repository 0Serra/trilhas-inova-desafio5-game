using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EntradaRanking
{
    public string nome;
    public int pontuacao;
}

public static class SistemaDeRanking
{
    private const string ChaveRanking = "RankingLocal";
    private const int MaxEntradas = 5;

    public static void SalvarPontuacao(string nome, int pontuacao)
    {
        List<EntradaRanking> ranking = CarregarRanking();

        ranking.Add(new EntradaRanking { nome = nome, pontuacao = pontuacao });

        ranking.Sort((a, b) => b.pontuacao.CompareTo(a.pontuacao));

        if (ranking.Count > MaxEntradas)
            ranking.RemoveRange(MaxEntradas, ranking.Count - MaxEntradas);

        string json = JsonUtility.ToJson(new ListaRanking { entradas = ranking });
        PlayerPrefs.SetString(ChaveRanking, json);
        PlayerPrefs.Save();
    }

    public static List<EntradaRanking> CarregarRanking()
    {
        if (!PlayerPrefs.HasKey(ChaveRanking))
            return new List<EntradaRanking>();

        string json = PlayerPrefs.GetString(ChaveRanking);
        ListaRanking lista = JsonUtility.FromJson<ListaRanking>(json);
        return lista.entradas ?? new List<EntradaRanking>();
    }

    [System.Serializable]
    private class ListaRanking
    {
        public List<EntradaRanking> entradas;
    }
}
