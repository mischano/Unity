using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/complex-gravity/
public class GravitySource : MonoBehaviour
{
    void OnEnable()
    {
        EnableGravity();
    }

    void OnDisable()
    {
        DisableGravity();
    }

    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }

    public void EnableGravity()
    {
        CustomGravity.Register(this);
    }

    public void DisableGravity()
    {
        CustomGravity.Unregister(this);
    }
}