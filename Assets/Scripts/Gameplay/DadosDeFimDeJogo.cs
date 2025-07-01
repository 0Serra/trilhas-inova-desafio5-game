public static class DadosFinaisDeJogo
{
    public static bool Venceu { get; set; }
    public static int PontuacaoDuranteOJogo { get; set; }
    public static int PontuacaoBonus { get; set; }

    public static int PontuacaoTotal => PontuacaoDuranteOJogo + PontuacaoBonus;
}