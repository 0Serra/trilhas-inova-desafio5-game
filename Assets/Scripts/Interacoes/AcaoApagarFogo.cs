
public class AcaoApagarFogo : AcoesDeInteracao
{
    public override float Duracao => 2.0f;

    public override bool PodeInteragir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        return celulaAlvo.tipo == TipoDeCelula.Fogo && estadosDoJogador.TemAgua;
    }

    public override void Interagir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        estadosDoJogador.UsarAgua();
        celulaAlvo.ApagarFogo();
    }
}
