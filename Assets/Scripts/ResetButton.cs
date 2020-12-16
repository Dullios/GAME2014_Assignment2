using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public PlayerBehaviour player;

    public void OnButtonPressed()
    {
        player.Respawn();
    }
}
