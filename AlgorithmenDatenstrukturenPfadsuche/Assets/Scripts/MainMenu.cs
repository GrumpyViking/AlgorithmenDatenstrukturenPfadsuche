using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    //public GameObject loadingScreen;
    
    public void ChangeScene(int sceneID)
    {
        StartCoroutine(LoadAsynchronously(sceneID));//Koroutinen ermöglichen es während der Ausführung Rückmeldungen zum gesamten Programm zu geben
    }

    //IEnumerator ist der Rückgabetyp für Koroutinen 
    IEnumerator LoadAsynchronously(int sceneID)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneID); //Variable operation beinhaltet vielzahl der werte die während der AsyncOperation anfallen
        //loadingScreen.SetActive(true); //Aktiviert die Anzeige der Ladeanzeige
        yield return null;
        /*
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f); // Standard ist in unity der Wert operation.progress von 0-0.9 dadurch umwandlung zu 0-1
            slider.value = progress; // anzeige Ladestand
             // das fortsetzen der while-schleife wird bis zum nächsten Frame ausgesetzt
        }
        */
    }

    public void ExitProgram()
    {
        Application.Quit();//Beendet die Anwendung, wenn das Projekt exportiert wurde
        
        UnityEditor.EditorApplication.isPlaying = false; // Beenden wenn im Editor gestartet
    }
}
