
public class AcaoColetarAgua : AcoesDeInteracao
{
    public override float Duracao => 1.5f;

    public override bool PodeInteragir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        return celulaAlvo.tipo == TipoDeCelula.Agua && !estadosDoJogador.TemAgua;
    }

    public override void Interagir(Celula celulaAlvo, EstadosDoJogador estadosDoJogador)
    {
        estadosDoJogador.ColetarAgua();
    }
}
