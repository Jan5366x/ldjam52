using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShowLevelNotCompletedMessage : ShowMessage
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (GlobalVariables.world == GlobalVariables.World.OtherDimention &&
            col.gameObject.CompareTag("Player") &&
            !GlobalVariables.CanLevelBeCompleted())
        {
            TriggerEntered(col);
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        TriggerExited(col);
    }
}