#pragma strict

//MADE BY: Mick Boere

var spreadAfter : float = 1;
var damagepp : float = 8;
var force : float = 100;

function OnParticleCollision (other : GameObject)
{
		var body : Rigidbody = other.GetComponent.<Rigidbody>();
		other.SendMessageUpwards("ApplyDamage", damagepp, SendMessageOptions.DontRequireReceiver);
		
		if (body)
		{
			var direction : Vector3 = other.transform.position - transform.position;
			direction = direction.normalized;
			body.AddForce (direction * force);
		}
		
		yield WaitForSeconds(spreadAfter);
		if (other != null)
		{
		  other.SendMessageUpwards("Fire", null, SendMessageOptions.DontRequireReceiver);
        }
}