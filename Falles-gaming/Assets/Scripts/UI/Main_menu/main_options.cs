using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    public void Jugar()
    {
        Debug.Log("Iniciando el juego...");
        SceneManager.LoadScene("SampleScene"); // Nombre de tu escena de juego
    }

    public void Instruccions()
    {
        SceneManager.LoadScene("InstruccionsScene"); // Nombre de tu escena de juego
        Debug.Log("anar a la escena instruccions");
    }
}
