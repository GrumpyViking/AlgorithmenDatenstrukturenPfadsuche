using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Verantwortlich für den Szenen wechsel des Programms.
 * 
 * Martin Schuster
 */

public class MainMenu : MonoBehaviour {
    public void ChangeScene(int sceneID) {
        PlayerSceneData.lastScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadAsynchronously(sceneID));//Koroutinen ermöglichen es während der Ausführung Rückmeldungen zu geben
    }

    // Rückkehr zur vorherigen Szene
    public void LastScene() {
        ChangeScene(PlayerSceneData.lastScene);
    }

    //IEnumerator ist der Rückgabetyp für Koroutinen 
    IEnumerator LoadAsynchronously(int sceneID) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID); //Variable operation beinhaltet vielzahl der werte die während der AsyncOperation anfallen
        yield return null;
    }

    // Beendet das Programm
    public void ExitProgram() {
        Application.Quit();//Beendet die Anwendung, wenn das Projekt exportiert wurde

        UnityEditor.EditorApplication.isPlaying = false; // Beenden wenn im Editor gestartet (Aus Kommentieren für den Export des Spiels)
    }
}
