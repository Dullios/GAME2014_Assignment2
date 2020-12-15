using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPlatformBehaviour : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private int countdown;
    public int respawnTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        countdown = 4;
    }

    public void Countdown()
    {
        countdown--;
        anim.SetInteger("BombTimer", countdown);

        if (countdown < 0)
        {
            spriteRenderer.enabled = false;
            countdown = 4;
            StartCoroutine(RespawnRoutine());
        }
    }

    IEnumerator RespawnRoutine()
    {
        yield return new WaitForSeconds(respawnTime);
        if (countdown != 4)
            countdown = 4;
        anim.SetInteger("BombTimer", countdown);
        spriteRenderer.enabled = true;
    }
}
