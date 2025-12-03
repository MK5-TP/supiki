using UnityEngine;
using System.Collections;

public class idleSupiki : MonoBehaviour
{
    [Header("暇人の設定")]
    public float jumpPower = 5.0f;    
    public AudioClip[] voices;        
    public Sprite[] faceVariations;   
    public Sprite defaultFace;        

    private Rigidbody2D _rb;
    private SpriteRenderer _renderer;
    private AudioSource _audioSource;

    
    void OnEnable()
    {
        
        _rb = GetComponent<Rigidbody2D>();
        _renderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

        if (defaultFace == null && _renderer != null) defaultFace = _renderer.sprite;

        
        StartCoroutine(IdleRoutine());
    }

    
    void OnDisable()
    {
        
        if (_renderer != null && defaultFace != null) _renderer.sprite = defaultFace;
    }

    IEnumerator IdleRoutine()
    {
        while (true)
        {
            
            float wait = Random.Range(1.5f, 3.5f);
            yield return new WaitForSeconds(wait);

            
            if (_rb != null)
            {
                
                _rb.velocity = Vector2.up * jumpPower;
            }

           
            if (voices != null && voices.Length > 0 && Random.value > 0.3f)
            {
                _audioSource.PlayOneShot(voices[Random.Range(0, voices.Length)]);
            }

            
            if (_renderer != null && faceVariations != null && faceVariations.Length > 0)
            {
                _renderer.sprite = faceVariations[Random.Range(0, faceVariations.Length)];
            }

            yield return new WaitForSeconds(0.5f);
            if (_renderer != null && defaultFace != null) _renderer.sprite = defaultFace;
        }
    }
}