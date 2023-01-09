using System;
using System.Collections;
using UnityEngine;

namespace Actions
{
    public class FinalHeart : MonoBehaviour
    {
        public bool triggered = false;

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                if (!triggered)
                {
                    StartCoroutine("Finale");
                    triggered = true;
                }

            }
        }

        private IEnumerator Finale()
        {
            Debug.Log("Started Finale2");
            GlobalVariables.fillingHearts = true;
            yield return new WaitForSeconds(1);
            while (GlobalVariables.hearts < 9)
            {
                GlobalVariables.hearts++;
                Debug.Log("Hearts : "+GlobalVariables.hearts);
                yield return new WaitForSeconds(0.2f);
            }

            yield return new WaitForSeconds(1);
            GlobalVariables.fillingHearts = false;
            Debug.Log("Done");

            GameObject.FindWithTag("Player").GetComponent<GameEventHandler>().OnVictory();
        }
    }
}