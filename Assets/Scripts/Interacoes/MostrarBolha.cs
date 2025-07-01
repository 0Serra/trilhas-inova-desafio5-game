using UnityEngine;
using UnityEngine.UI;

public class MostrarBolha : MonoBehaviour
{
    [SerializeField] private Image imagemGota;

    public void MostrarGota(bool mostrar)
    {
        imagemGota.gameObject.SetActive(mostrar);
    }
}
