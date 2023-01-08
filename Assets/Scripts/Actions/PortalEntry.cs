using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalEntry : MonoBehaviour
{
    public GlobalVariables.World nextWorld = GlobalVariables.World.OtherDimention;

    public GameObject nextPortal;

    [AllowsNull]
    public string nextSceneName;

    [AllowsNull]
    public GameObject afterEffectPrefab;

    void Start()
    {
        if (GlobalVariables.world != nextWorld)
        {
            nextPortal.SetActive(false);
        }
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log($"Collision detected with {col.gameObject.name} (tag:{col.tag}) by {gameObject.name}");

        if (col.tag?.Equals("player", StringComparison.OrdinalIgnoreCase) ?? false)
        {
            SwitchWorld();
            //Debug.Log($"GlobalVariables.world: {GlobalVariables.world}");

            if (afterEffectPrefab != null)
                Instantiate(afterEffectPrefab, transform.position, Quaternion.identity);

            if (!string.IsNullOrWhiteSpace(nextSceneName))
                StartCoroutine(LoadNextSceneAsync());

            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }

    private void SwitchWorld()
    {
        GlobalVariables.world = nextWorld;
        nextPortal.SetActive(true);
    }

    IEnumerator LoadNextSceneAsync()
    {
        //Debug.Log($"Loading next scene: {nextSceneName}");
        var loadOperation = SceneManager.LoadSceneAsync(nextSceneName);
        // Do we need loading screen?

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        //Debug.Log($"Loaded next scene: {nextSceneName}");
    }
}
