using UnityEngine;
using UnityEngine.UI;

public class DragonMovement : MonoBehaviour
{
    [SerializeField] float d_speed = 5.0f;
    [SerializeField] float bullet_speed = 50.0f;

    public ObstacleController obstacleController;
    public GameObject bulletPrefab;
    public PageController _pageController;

    [SerializeField] Rigidbody2D d_rb;
    [SerializeField] Animator fishAnimator;
    [SerializeField] FishSettings fishSettings;
    [SerializeField] SpriteRenderer spriteRenderer;
    bool win = false;
    bool isVibrateOn;
    bool isJumping;

    void Start()
    {
        ChangeSpriteColor();
        BulletController.Start();
        VibrateCheck();
        _pageController.bulletCountText.text = BulletController.bulletCount.ToString();
        _pageController.heartText.text = HeartController.heartCount.ToString();
    }
    void VibrateCheck()
    {
        var value = PlayerPrefs.GetInt(MainMenuManager.VIBRATE_INFO_KEY);
        if (value == 0)
            isVibrateOn = false;
        else 
            isVibrateOn = true;
    }
    void ChangeSpriteColor()
    {
        spriteRenderer.color = GameManager.instance.CurrentSkinColor;
    }
    void ResetSpriteColor()
    {
        spriteRenderer.color = Color.white;
    }
    void FixedUpdate()
    {
        if (win)
        {
            d_rb.constraints = RigidbodyConstraints2D.FreezePositionY;
            transform.Translate(3 * d_speed * Time.deltaTime * Vector2.right);
        }

        if (isJumping)
            d_rb.velocity = Vector2.up * d_speed;
        else
            d_rb.velocity = Vector2.down * 4f;
    }
    public void Movement(bool _isJumping)
    {
        isJumping = _isJumping;
        //fishAnimator.SetBool("isDead", false);
    }
    public void Shoot()
    {
        if (BulletController.bulletCount > 0)
        {
            GameObject bullet = Instantiate(bulletPrefab, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), transform.rotation);
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(-bullet.transform.position.x + bullet_speed, bullet.transform.position.y, 0);
            AudioManager.instance.Play("Fire");
            BulletController.RemoveBullet();
            Destroy(bullet, 2);
            _pageController.bulletCountText.text = BulletController.bulletCount.ToString();
        }
    }
    private void StopDragon()
    {
        ResetSpriteColor();
        fishAnimator.SetBool("isDead", true);
        _pageController.fireButton.GetComponent<Button>().enabled = false;
        _pageController.jumpButton.GetComponent<Button>().enabled = false;
        _pageController.pauseButton.SetActive(false);
        BulletController.SetBullet();
        d_rb.constraints = RigidbodyConstraints2D.FreezePosition;
        d_rb.isKinematic = true;
        d_rb.velocity = Vector2.zero;
        obstacleController.speed = 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("Ground"))
        {
            StopDragon();
            HeartController.RemoveHeart();
            AudioManager.instance.Play("Lose");
            if (isVibrateOn)
            {
                Debug.Log("Vibrate");
#if UNITY_ANDROID
                Handheld.Vibrate();
#endif
            }
            _pageController.loosePage.SetActive(true);
            _pageController.heartText.text = HeartController.heartCount.ToString();
        }

        if (other.gameObject.CompareTag("WinCollider"))
        {
            win = true;
            GameManager.instance.OnLevelFinished();
            AudioManager.instance.Play("Win");
            _pageController.fireButton.GetComponent<Button>().enabled = false;
            _pageController.jumpButton.GetComponent<Button>().enabled = false;
            if (isVibrateOn)
            {
                Debug.Log("Vibrate");
#if UNITY_ANDROID
                Handheld.Vibrate();
#endif
            }
            _pageController.winPage.SetActive(true);

        }
        if (other.gameObject.CompareTag("Roof"))
        {
            if (isJumping)
                d_rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Roof") && !isJumping && _pageController.fireButton.GetComponent<Button>().enabled == true)
            d_rb.constraints = RigidbodyConstraints2D.None;
    }
}