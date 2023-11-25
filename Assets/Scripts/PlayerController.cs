using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	private Vector2 _mousePosition;
	private Vector2 _targetPos;
	[SerializeField] private float moveSpeed = 5f;
	private float xmin, xmax, ymin, ymax;
	private int lIndex, rIndex, uIndex, dIndex;
	public Sprite[] LeftMovement;
	public Sprite[] RightMovement;
	public Sprite[] UpMovement;
	public Sprite[] DownMovement;
	private Vector3 distVector;
	public float spriteChangeDist;
	public bool canMove;
	private Rigidbody2D rigidBody;
	private Vector3 newPos;
	public LevelManager levelManager;
	private Vector2 moveDirection = new Vector2(1f, 0f);
	[SerializeField]
	public Stat Health;
	public GameObject bulletPrefab;
	public float bulletSpeed = 5f;
	public float firingRate = 5f;
	public AudioClip fireSound;

	Vector3 previousPosition;
    Vector3 lastMoveDirection;

	void Start () {
		lIndex = rIndex = uIndex = dIndex = 0;
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
		Vector3 upmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, distance));
		Vector3 downmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
		xmin = leftmost.x + (GetComponent<Renderer>().bounds.size.x)/2;
		xmax = rightmost.x - (GetComponent<Renderer>().bounds.size.x)/2;
		ymin = downmost.y + (GetComponent<Renderer>().bounds.size.y)/2;
		ymax = upmost.y - (GetComponent<Renderer>().bounds.size.y)/2;
		rigidBody = GetComponent<Rigidbody2D>();

		previousPosition = transform.position;
        lastMoveDirection = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (canMove) {
			Move ();
		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.00001f, firingRate);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			CancelInvoke("Fire");
		}
	}

	void OnTriggerStay2D (Collider2D collider)
	{
		if (collider.gameObject.tag == "GardenObjects" || collider.gameObject.name == "Girl") {
			float minYObject = collider.gameObject.transform.position.y - (collider.gameObject.GetComponent<Renderer> ().bounds.size.y) / 2;
			float minYPlayer = transform.position.y - (GetComponent<Renderer> ().bounds.size.y) / 2;
			if (minYPlayer < minYObject) {
				transform.position = new Vector3 (transform.position.x, transform.position.y, collider.gameObject.transform.position.z - 1);
			} else {
				transform.position = new Vector3 (transform.position.x, transform.position.y, collider.gameObject.transform.position.z + 1);
			}
		}
	}

	void Move ()
	{
		if (Input.GetMouseButton(0)) 
        {
            _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
 
        if (_targetPos != Vector2.zero) 
        {
            Vector2 direction = (_targetPos - (Vector2)transform.position).normalized;
        	transform.position = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;
        }
        else if (_targetPos == (Vector2)transform.position) 
        {
            _targetPos = Vector3.zero;
        }

		Debug.Log(lastMoveDirection);
		if ((Vector2)lastMoveDirection == Vector2.zero) {
			GetComponent<SpriteRenderer> ().sprite = DownMovement [0];
			return;
		}

		bool maxValX = Mathf.Max(Mathf.Abs(lastMoveDirection.x), Mathf.Abs(lastMoveDirection.y)) == Mathf.Abs(lastMoveDirection.x);
		if (maxValX)
			GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];

		if (maxValX)
		{
			if (lastMoveDirection.x > 0)
			{
				GetComponent<SpriteRenderer> ().sprite = RightMovement [rIndex];
				if (Vector3.Distance (transform.position, lastMoveDirection) > spriteChangeDist) {
					rIndex++;
				}
				if(rIndex >= RightMovement.Length) {rIndex = 0;}
			}
			else
			{
				GetComponent<SpriteRenderer> ().sprite = LeftMovement [lIndex];
				if (Vector3.Distance (transform.position, lastMoveDirection) > spriteChangeDist) {
					lIndex++;
				}
				if(lIndex >= LeftMovement.Length) {lIndex = 0;}
			}
		}
		else
		{
			if (lastMoveDirection.y > 0)
			{
				GetComponent<SpriteRenderer> ().sprite = UpMovement [uIndex];
				if (Vector3.Distance (transform.position, lastMoveDirection) > spriteChangeDist) {
					uIndex++;
				}
				if(uIndex >= UpMovement.Length) {uIndex = 0;}
			}
			else
			{
				GetComponent<SpriteRenderer> ().sprite = DownMovement [dIndex];
				if (Vector3.Distance (transform.position, lastMoveDirection) > spriteChangeDist) {
					dIndex++;
				}
				if(dIndex >= DownMovement.Length) {dIndex = 0;}
			}
		}
	}

	void Fire ()
	{
		if (PlayerBullet.usedBullets < PlayerBullet.maxBullets) {
			GameObject bullet = Instantiate (bulletPrefab, transform.position, Quaternion.identity) as GameObject;
			bullet.GetComponent<PlayerBullet> ().originPoint = transform.position;
			bullet.GetComponent<Rigidbody2D> ().velocity = moveDirection * bulletSpeed;
			if (moveDirection != Vector2.zero) {
				float angle = Mathf.Atan2 (moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
				bullet.transform.rotation = Quaternion.AngleAxis (angle, Vector3.forward);
			}
			AudioSource.PlayClipAtPoint (fireSound, transform.position);
		}
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		HitWithBullet(collider);
	}

	void HitWithBullet (Collider2D collider)
	{
		EnemyBullet missile = collider.gameObject.GetComponent<EnemyBullet> ();

		if (missile) {
			Health.CurrentHealth -= missile.GetDamage ();
			missile.Hit();
		}

		if (Health.CurrentHealth <= 0) {
			levelManager.LoadLevel("Lose Screen");
		}
	}

	public void FixedUpdate() {
		lastMoveDirection = (transform.position - previousPosition).normalized;
		previousPosition = transform.position;
    }
}
