
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        switch (other.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You collided with the Launch Pad");
                break;

            case "Finish":
                Debug.Log("You landed on the Landing Pad");
                break;

            default:
                Debug.Log("You crashed!");
                break;
        }
    }
}
