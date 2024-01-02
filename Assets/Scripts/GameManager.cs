using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 
using TMPro; 
public class GameManager : MonoBehaviour
{
    public GameObject homePanel;
    public GameObject endPanel;
    public GameObject playerPrefab; // Prefab del giocatore
    private GameObject currentPlayer; // Istanza corrente del giocatore
    public float loseYPosition = -5f;
        public GameObject targetPrefab; // Prefab dell'oggetto target
    private List<GameObject> targets = new List<GameObject>(); // Lista per tenere traccia dei target attivi
    public int numberOfTargets = 3; // Numero di target da mantenere in gioco
    private float startTime;
       public float spawnXMin = -2f; // Minima posizione X per lo spawn
    public float spawnXMax = 2f;  // Massima posizione X per lo spawn
    public float spawnYMin = -5f; // Minima posizione Y per lo spawn
    public float spawnYMax = 5f;  // Massima posizione Y per lo spawn
    public float spawnZPosition = 20f; // La posizione Z per lo spawn dei target
    

    public TextMeshProUGUI survivalTimeText; // Riferimento al testo del tempo di sopravvivenza
    public TextMeshProUGUI shotsFiredText; 
    private int shotsFired = 0;
    public TextMeshProUGUI bestScoreText; // Aggiungi questo
   public GameObject shootButton; // Pulsante per sparare
    public GameObject flipButton; // Pulsante per capovolgere


    void Update()
    {
        if (currentPlayer != null && currentPlayer.transform.position.y < loseYPosition)
        {
            EndGame();
        }
    }

public void StartGame()
{
    homePanel.SetActive(false);
    endPanel.SetActive(false);

    // Crea il giocatore
    currentPlayer = Instantiate(playerPrefab);

    // Assegna la posizione desiderata al giocatore (ad esempio, posizione iniziale)
    currentPlayer.transform.position = new Vector3(0f, 0.5f, 0f); // Modifica le coordinate come necessario

   

    startTime = Time.time;
    shotsFired = 0;

    shootButton.SetActive(true);
    flipButton.SetActive(true);
      SpawnTargets(numberOfTargets);
}




void SpawnTargets(int count)
{
    for (int i = 0; i < count; i++)
    {
        SpawnTarget();
    }
}
void SpawnTarget()
{
    Vector3 spawnPosition = GetRandomPosition(); // Ottieni una posizione casuale

    // Imposta una rotazione specifica se necessario
    float rotationX = -90f; // Sostituisci con il valore desiderato
    float rotationY = 0f; // Sostituisci con il valore desiderato
    float rotationZ = 0f; // Sostituisci con il valore desiderato
    Quaternion spawnRotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

    GameObject newTarget = Instantiate(targetPrefab, spawnPosition, spawnRotation);
    targets.Add(newTarget);
}

   Vector3 GetRandomPosition()
    {
        // Utilizza la posizione Z del giocatore corrente
        float playerZ = currentPlayer.transform.position.z;

        // Calcola una posizione casuale all'interno dei limiti definiti
        float x = Random.Range(spawnXMin, spawnXMax);
        float y = Random.Range(spawnYMin, spawnYMax);


    return new Vector3(x, y, spawnZPosition);
        }

public void TargetHit(GameObject target)
{
    targets.Remove(target);
    Destroy(target);
    SpawnTarget(); // Crea un nuovo target dopo che uno è stato colpito
}
public void ShotFired()
{
    shotsFired++;
}


public void EndGame()
{
            shootButton.SetActive(false); // Disabilita il pulsante di sparo
    flipButton.SetActive(false);

    
    endPanel.SetActive(true);
    if (currentPlayer != null)
    {
        Destroy(currentPlayer); // Distrugge l'istanza del giocatore
        currentPlayer = null;
    }

    foreach (var target in targets)
    {
        Destroy(target); // Distrugge ogni target
    }
    targets.Clear(); // Pulisce la lista dei target


     float survivalTime = Time.time - startTime;
    int bestShotsFired = PlayerPrefs.GetInt("BestShotsFired", int.MaxValue);
    float bestSurvivalTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0);

    // Aggiorna il miglior punteggio se il giocatore ha sopravvissuto più a lungo o ha sparato meno colpi
    if (survivalTime > bestSurvivalTime || (survivalTime == bestSurvivalTime && shotsFired < bestShotsFired))
    {
        PlayerPrefs.SetFloat("BestSurvivalTime", survivalTime);
        PlayerPrefs.SetInt("BestShotsFired", shotsFired);
    }

    survivalTimeText.text = "Time Survived: " + survivalTime.ToString("F2") + " seconds";
    shotsFiredText.text = "Shots Fired: " + shotsFired.ToString();
    FindObjectOfType<BulletManager>().ResetBullets();

}


    public void RestartGame()
    {
StartGame() ;
   }

public void OpenHomePanel()
{
    homePanel.SetActive(true);
    endPanel.SetActive(false);

    float bestSurvivalTime = PlayerPrefs.GetFloat("BestSurvivalTime", 0);
    int bestShotsFired = PlayerPrefs.GetInt("BestShotsFired", int.MaxValue);

    bestScoreText.text = "Best Time: " + bestSurvivalTime.ToString("F2") + " seconds\n" +
                         "Best Shots Fired: " + bestShotsFired.ToString();
}
}
