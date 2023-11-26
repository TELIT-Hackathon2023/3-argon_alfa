using UnityEngine;
public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopWindowObj;
    private PlayerController _player;
    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MainPlayer"))
        {
            shopWindowObj.SetActive(true);

            _player = other.gameObject.GetComponent<PlayerController>();
            _player.Stop();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            shopWindowObj.SetActive(false);
            _player.canMove = true;
        }
    }
}