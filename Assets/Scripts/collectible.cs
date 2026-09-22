using NUnit.Framework.Constraints;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class collectible : MonoBehaviour
{
    [SerializeField] private string m_playerTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} with tag {other.tag} has collided with {name}");
        if (other.tag == m_playerTag)
        {
            Destroy(this.gameObject);
        }
    }
}
