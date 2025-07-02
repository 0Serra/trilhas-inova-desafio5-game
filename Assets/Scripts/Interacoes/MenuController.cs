using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TransicaoDeCena transicaoDeCena;

    public void NovoJogo()
    {
        transicaoDeCena.TransicionarParaCena("GameScene");
    }


    // public void Tutorial()
    // {

    //     Debug.Log("");
    // }

    // public void Recordes()
    // {}
    //     Debug.Log("");
    // }

    // public void Configuracoes()
    // {
    //     Debug.Log("");
    // }


    public void Sair()
    {
        Debug.Log("SAINDO!");
        Application.Quit();
    }
}
