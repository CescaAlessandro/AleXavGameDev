using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionBehaviour : MonoBehaviour
{
    //metodo per aggiornare le icone dei pulsanti nella level selection in base ai livelli sbloccati
    public void UpdateLevels()
    {
        var scrollView = this.transform.GetChild(0).transform.GetChild(1);
        var content = scrollView.transform.GetChild(0).transform.GetChild(0);
        var numberLevelRows = content.transform.childCount;

        for (int i = 0; i< numberLevelRows; i++)
        {
            var levelRow = content.transform.GetChild(i);
            var numberLevels = levelRow.transform.childCount;

            for (int j = 0; j < numberLevels; j++)
            {
                var textAndImage = levelRow.GetChild(j);
                var unlockedImageObject = textAndImage.GetChild(0);
                var lockedImageObject = textAndImage.GetChild(1);
                var textObject = textAndImage.GetChild(2);
                var text = textObject.GetComponent<TextMeshProUGUI>().text;

                //leggo lo stato del livello relativo al pulsante 
                var lockStatus = SaveManager.Instance().GetLevelState(text);
                //e scelgo se mostrare l'icona del livello sbloccato o meno
                if (lockStatus == 0)
                {
                    unlockedImageObject.gameObject.SetActive(false);
                    lockedImageObject.gameObject.SetActive(true);
                }
                else
                {
                    unlockedImageObject.gameObject.SetActive(true);
                    lockedImageObject.gameObject.SetActive(false);
                }
            }
        }
    }
}
