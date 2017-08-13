#pragma strict

//MADE BY: Mick Boere

var part : ParticleSystem;
var lights : Light[];
var lightsIntensity : float = 0;
var lerpSpeed : float = 2.0;
var newmat : Material;
var wet : boolean = false;
private var SmartDestroy : GameObject;
private var burning : boolean = false;

function Awake ()
{
    SmartDestroy = GameObject.FindGameObjectWithTag("SmartDestroy");
}

function Start ()
{
    for(var l : int = 0; l < lights.Length; l++)
    {
        lights[l].intensity = 0;
    }
}

function Fire ()
{
    if(!burning && !wet)
    {
        burning = true;
        gameObject.GetComponent.<Renderer>().material = newmat;
        //LerpLights(lightsIntensity);
        LightIntensity(lightsIntensity);
        part.Play();
        Invoke("Burn", part.duration);
    }
}

function LightIntensity (to : float)
{
    for(var i : int = 0; i < lights.Length; i++)
    {
        lights[i].intensity = to;
    }
}

/* !!! NOT RECOMMENDED !!!
function LerpLights (to : float)
{
    for(var i : int = 0; i < lights.Length; i++)
    {
        if(to > lights[i].intensity)
        {
            while(lights[i].intensity <= to - 0.1)
            {
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, lightsIntensity, lerpSpeed * Time.deltaTime);
            }
            if(lights[i].intensity >= to - 0.1)
            {
                    lights[i].intensity = to;
            }
            yield;
        }
        else if (to < lights[i].intensity)
        {
            while(lights[i].intensity >= to + 0.1)
            {
                lights[i].intensity = Mathf.Lerp(lights[i].intensity, lightsIntensity, lerpSpeed * Time.deltaTime);
            }
            if(lights[i].intensity <= to + 0.1)
            {
                lights[i].intensity = to;
            }
            yield;
        }
    }
}
*/

function Burn ()
{
    SmartDestroy.SendMessage("Add", gameObject);
}

function Extinguish ()
{
    if(!wet)
    {
        wet = true;
        burning = false;
        part.Stop();
        CancelInvoke("Burn");
        //LerpLights(0.0);
        LightIntensity(0);
        Invoke("Dry", 3);
    }
}

function Dry ()
{
    wet = false;
}