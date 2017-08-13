enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 };
public var axes = RotationAxes.MouseXAndY;
var amount : float = 0.02;
var maxAmount : float = 0.03;
var smooth : float = 3;
private var def : Vector3;
var curPos : Vector3;
var factorX : float;
var factorY : float;
 
function Start (){
    def = transform.localPosition;
}
 
function Update (){
curPos = transform.localPosition;
if (axes == RotationAxes.MouseXAndY){
factorX = -Input.GetAxis("Mouse X") * amount;
        factorY = -Input.GetAxis("Mouse Y") * amount;
}
else if (axes == RotationAxes.MouseX){
factorX = -Input.GetAxis("Mouse X") * amount;
}
else if (axes == RotationAxes.MouseY){
        factorY = -Input.GetAxis("Mouse Y") * amount;
}
        if (factorX > maxAmount)
        factorX = maxAmount;
       
        if (factorX < -maxAmount)
        factorX = -maxAmount;
       
        if (factorY > maxAmount)
        factorY = maxAmount;
       
        if (factorY < -maxAmount)
        factorY = -maxAmount;
        
 
        var pos : Vector3 = new Vector3(def.x+factorX, def.y+factorY, def.z);
        transform.localPosition = Vector3.Lerp(transform.localPosition, pos, Time.deltaTime * smooth);       
}