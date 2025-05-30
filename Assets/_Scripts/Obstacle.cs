using _Scripts;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private FanceAnimation hitAnimation;
    public Tile tile;

    private void OnEnable()
    {
        var rotation = transform.rotation.eulerAngles;
        rotation.x = 0f;
        transform.rotation = Quaternion.Euler(rotation);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        gameObject.GetComponent<Collider>().enabled = false;
        hitAnimation.Animate();
        gameObject.transform.SetParent(null);
        tile.obstacles.Remove(this);
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
