using UnityEngine;


public class CameraFollow : MonoBehaviour

{
	public Transform target;
	public float smoothSpeed = 0.125f;
	public Vector3 locationOffset;
	public Vector3 rotationOffset;

	void FixedUpdate()
	{
		if(target == null)
			return;

		var desiredPosition = target.position + target.rotation * locationOffset;
		var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
		transform.position = smoothedPosition;

		var desiredrotation = target.rotation * Quaternion.Euler(rotationOffset);
		var smoothedrotation = Quaternion.Lerp(transform.rotation, desiredrotation, smoothSpeed);
		transform.rotation = smoothedrotation;
	}
	public void SetTarget(Transform target)
	{
		this.target = target;
	}
}