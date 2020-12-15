using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SwitchPlatformBehaviour : MonoBehaviour
{
    public PlayerBehaviour player;
    private TilemapRenderer tileRenderer;
    private TilemapCollider2D tileCollider;

    public float refreshTime;
    private bool canSwitch = true;

    private void Start()
    {
        tileRenderer = GetComponent<TilemapRenderer>();
        tileCollider = GetComponent<TilemapCollider2D>();

        player.OnJump.AddListener(Switch);
    }

    public void Switch()
    {
        if (canSwitch)
        {
            tileRenderer.enabled = !tileRenderer.enabled;
            tileCollider.enabled = !tileCollider.enabled;
            canSwitch = false;
            StartCoroutine(RefreshSwitch());
        }
    }

    IEnumerator RefreshSwitch()
    {
        yield return new WaitForSeconds(refreshTime);
        canSwitch = true;
    }
}
