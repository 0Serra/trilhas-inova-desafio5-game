using UnityEngine;

public class InicializadorDeTelaFinal : MonoBehaviour
{
    private void Start()
    {
        if (DadosFinaisDeJogo.Venceu)
        {
            FindObjectOfType<TelaFinalUI>().MostrarVitoria(
                DadosFinaisDeJogo.PontuacaoDuranteOJogo,
                DadosFinaisDeJogo.PontuacaoBonus
            );
        }
        else
        {
            FindObjectOfType<TelaFinalUI>().MostrarDerrota();
        }
    }
}
