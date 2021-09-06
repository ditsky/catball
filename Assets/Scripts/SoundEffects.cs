using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    public AudioSource dash;
    public AudioSource pop;
    public AudioSource powerup;
    public AudioSource bounce;
    public AudioSource goal;
    public AudioSource shoot;

    public void Dash()
    {
        dash.Play();
    }

    public void Pop()
    {
        pop.Play();
    }

    public void Powerup()
    {
        powerup.Play();
    }

    public void Bounce()
    {
        bounce.Play();
    }
    public void Goal()
    {
        goal.Play();
    }
    public void Shoot()
    {
        shoot.Play();
    }
}
