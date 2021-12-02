using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/complex-gravity/
public class GravitySource : MonoBehaviour
{
    bool _isEnabled;

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
        if (_isEnabled)
        {
            return;
        }
        _isEnabled = true;
        CustomGravity.Register(this);
    }

    public void DisableGravity()
    {
        if (!_isEnabled)
        {
            return;
        }
        _isEnabled = false;
        CustomGravity.Unregister(this);
    }
}