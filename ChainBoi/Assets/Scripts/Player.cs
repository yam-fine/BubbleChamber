using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject tailPiecePrefab;
    [SerializeField] Transform tailTip;
    [SerializeField] float distanceBetweenTails = 0;
    [SerializeField] float defaultMass = 1, defaultDrag = 2, tipDrag = 0;
    [SerializeField] CinemachineVirtualCamera vcam;
    [SerializeField] float shakeIntensity;

    public UnityEvent enemyDestroyedEvent;

    Rigidbody2D rb;
    float hori, verti;
    float tipMass = 1;
    int tailLen = 0;
    float shakeTime = .2f;
    CinemachineBasicMultiChannelPerlin vcamMCP;

    static Player instance;
    public static Player Instance {
        get {
            if (!instance) {
                instance = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            }
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        tailTip = transform;
        vcamMCP = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void FixedUpdate() {
        hori = Input.GetAxis("Horizontal");
        verti = Input.GetAxis("Vertical");
        if (hori > 0)
            rb.AddForce(Vector2.right * speed * Time.deltaTime, ForceMode2D.Impulse);
        if (hori < 0)
            rb.AddForce(Vector2.left * speed * Time.deltaTime, ForceMode2D.Impulse);
        if (verti > 0)
            rb.AddForce(Vector2.up * speed * Time.deltaTime, ForceMode2D.Impulse);
        if (verti < 0)
            rb.AddForce(Vector2.down * speed * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "Enemy") {
            Die();
        }
        else if(collision.tag == "Food") {
            GameManager.IncScore();
            enemyDestroyedEvent.Invoke();
            Grow();
            collision.GetComponent<Enemy>().Die();
        }
    }

    void Die() { // todo : Save score if it's the highest
                 // todo : add a msg
        SceneManager.LoadScene("MainMenu");
    }

    void Grow() {
        StartCoroutine(Shake());
        tailLen++;
        Vector3 pos = new Vector3(tailTip.position.x, tailTip.position.y - distanceBetweenTails, tailTip.position.z);
        GameObject newTip = Instantiate(tailPiecePrefab, pos, tailTip.rotation, transform);
        HingeJoint2D hj = tailTip.GetComponent<HingeJoint2D>();
        hj.enabled = true;
        hj.connectedBody = newTip.GetComponent<Rigidbody2D>();
        Rigidbody2D tailRb = tailTip.GetComponent<Rigidbody2D>();
        if (tailTip != transform) {
            tailRb.mass = defaultMass;
            tailRb.drag = defaultDrag;
        }

        tailTip = newTip.transform;
        tailRb = tailTip.GetComponent<Rigidbody2D>();
        tipMass = tailLen / 2;
        tailRb.mass = tipMass;
        tailRb.drag = tipDrag;
    }

    IEnumerator Shake() {
        float timePassed = 0;
        vcamMCP.m_AmplitudeGain = shakeIntensity;
        while (timePassed < shakeTime) {
            timePassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        vcamMCP.m_AmplitudeGain = 0;
    }
}
