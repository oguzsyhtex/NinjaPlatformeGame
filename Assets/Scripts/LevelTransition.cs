using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LevelTransition : MonoBehaviour
{
    public string nextLevelName;

    public int minCoinsRequired = 2;

    public Text warningText;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GoldManager goldManager = FindObjectOfType<GoldManager>();

            if (goldManager != null)
            {
                if (goldManager.goldCount >= minCoinsRequired)
                {
                    SceneManager.LoadScene(nextLevelName);
                }
                else
                {
                    StartCoroutine(ShowWarning("yeterli coin yok gecis icin en az 2 coin toplamalisin!", 4f));
                }
            }


        }
    }

    IEnumerator ShowWarning(string message, float delay)
    {
        warningText.text = message;
        warningText.enabled = true;

        yield return new WaitForSeconds(delay);

        warningText.enabled = false;
    }



}
