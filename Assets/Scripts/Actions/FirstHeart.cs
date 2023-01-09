using System;
using System.Collections;
using UnityEngine;

namespace Actions
{
    public class FirstHeart : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                StartCoroutine("RemoveHearts");
            }
        }

        private IEnumerator RemoveHearts()
        {
            yield return new WaitForSeconds(1);
            while (GlobalVariables.hearts > 1)
            {
                GlobalVariables.hearts--;
                Debug.Log("Hearts : " + GlobalVariables.hearts);
                yield return new WaitForSeconds(1);
            }
            Debug.Log("Done");
        }
    }
}