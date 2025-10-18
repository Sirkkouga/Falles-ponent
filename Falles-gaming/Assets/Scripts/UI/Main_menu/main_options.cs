using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        Debug.Log("Iniciando el juego...");
        SceneManager.LoadScene("Juego"); // Nombre de tu escena de juego
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Saliendo del juego...");
    }
}
