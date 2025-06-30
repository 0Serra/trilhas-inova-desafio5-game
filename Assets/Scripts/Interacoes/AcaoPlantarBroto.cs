
public class AcaoPlantarBroto : AcoesDeInteracao
{
    public override float Duracao => 2.0f;

    public override bool PodeInteragir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        return celulaAlvo.tipo == TipoDeCelula.Cinzas;
    }

    public override void Interagir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        celulaAlvo.DefinirTipo(TipoDeCelula.Broto);
        celulaAlvo.MudarSprite("spriteBroto");

        CrescedorDeBrotos.Instance.ComecarCrescimento(celulaAlvo);

        Pontuacao.Instance.PontuarPlantarBroto();
    }
}
