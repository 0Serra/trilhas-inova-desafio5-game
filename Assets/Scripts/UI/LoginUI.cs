using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUI : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public LoginRequest loginRequest;
    public TextMeshProUGUI responseText;
    public TransicaoDeCena transicaoDeCena;


    void Start()
    {
        loginButton.onClick.AddListener(OnLoginClicked);
        loginRequest.OnLoginMensagem = MostrarMensagem;
        loginRequest.OnLoginSucesso = IrParaMenu;
    }

    void OnLoginClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;
        loginRequest.username = username;
        loginRequest.password = password;
        loginRequest.FazerLogin();
        responseText.text = "🔄 Verificando login...";
    }

    void MostrarMensagem(string mensagem)
    {
        responseText.text = mensagem;
    }

    void IrParaMenu()
    {
        transicaoDeCena.TransicionarParaCena("Main Menu");
    }

}
