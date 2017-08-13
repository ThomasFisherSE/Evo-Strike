import System.Collections.Generic;

//Script made by: Mick Boere

var maxPerSecond : float = 3;
var list : List.<GameObject>;

function Start ()
{
  InvokeRepeating("Destroy", 0, 1/maxPerSecond);
}

function Add (obj : GameObject)
{
  list.Add(obj);
  obj.SetActive(false);
}

function Destroy ()
{
  if(list.Count != 0)
  { 
    Destroy(list[list.Count-1]);
    list.Remove(list[list.Count-1]);
  }
}