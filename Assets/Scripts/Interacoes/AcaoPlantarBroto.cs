using UnityEngine;

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

        // Pega o Animator do objeto da célula
        Animator animator = celulaAlvo.objeto.GetComponent<Animator>();
        SpritesArvores visual = celulaAlvo.objeto.GetComponent<SpritesArvores>();

        if (animator != null && visual != null && visual.animacaoBrotoController != null)
        {
            animator.enabled = true;
            animator.runtimeAnimatorController = visual.animacaoBrotoController;
        }

        CrescedorDeBrotos.Instance.ComecarCrescimento(celulaAlvo);

        Pontuacao.Instance.PontuarPlantarBroto();
    }
}
