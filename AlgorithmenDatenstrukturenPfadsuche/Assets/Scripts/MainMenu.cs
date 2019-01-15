using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public void ChangeScene(int sceneID) {
        PlayerSceneData.lastScene = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadAsynchronously(sceneID));//Koroutinen ermöglichen es während der Ausführung Rückmeldungen zum gesamten Programm zu geben
    }

    public void LastScene() {
        ChangeScene(PlayerSceneData.lastScene);
    }

    //IEnumerator ist der Rückgabetyp für Koroutinen 
    IEnumerator LoadAsynchronously(int sceneID) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID); //Variable operation beinhaltet vielzahl der werte die während der AsyncOperation anfallen
        yield return null;
    }

    public void ExitProgram() {
        Application.Quit();//Beendet die Anwendung, wenn das Projekt exportiert wurde

        UnityEditor.EditorApplication.isPlaying = false; // Beenden wenn im Editor gestartet (Aus Kommentieren für den Export des Spiels)
    }
}
