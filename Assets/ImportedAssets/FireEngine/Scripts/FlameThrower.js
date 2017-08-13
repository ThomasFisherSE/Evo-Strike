#pragma strict

//MADE BY: Mick Boere

//var anim : Animator;
var button : String = "mouse 1";
var flameEmit : ParticleSystem;

function Start () {

}

function Update ()
{
  if(Input.GetKeyDown(button))
  {
    //anim.SetBool("Pressed", true);
    Invoke("Openfire", 0.1);
  }
  if(Input.GetKeyUp(button))
  {
    CancelInvoke("Openfire");
    flameEmit.Stop();
    //anim.SetBool("Pressed", false);
    flameEmit.GetComponent.<AudioSource>().Stop();
  }
}

function Openfire ()
{
    flameEmit.Play();
    flameEmit.GetComponent.<AudioSource>().Play();
}