using UnityEngine;
using System.Collections;

public class idleSupiki : MonoBehaviour
{
    [Header("ｱｲﾄﾞﾘﾝｸﾞ")]
    public float jumpHeight = 1.2f;   
    public float jumpDuration = 0.8f; 
    public AudioClip[] voices;    //ｼﾞｮﾜﾖｰ    
    public Sprite[] faceVariations;   
    public Sprite defaultFace;  

    [Header("ｶﾞｸﾌﾞﾙ")]
    public float shakeAmount = 0.05f;    
    public Sprite shiverFace;         
    public AudioClip[] shiverVoices;//ｽﾋﾟｷﾈﾙｼﾞﾊﾞｾﾞﾖ!!!

    private SpriteRenderer _renderer;
    private AudioSource _audioSource;
    private bool _isShivering = false;
    private Vector3 _basePos;

    void Start(){
         if (!_audioSource.isPlaying && voices != null)
            {
                _audioSource.PlayOneShot(voices[0]);
            }
    }


    void OnEnable()
    {
        
        _renderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();

        if (defaultFace == null && _renderer != null) defaultFace = _renderer.sprite;
        _basePos = transform.position;

        _isShivering = true;
        SetShiverState(false);
    }

    
    void OnDisable()
    {
        StopAllCoroutines();
        transform.position = _basePos;
        if (_renderer != null && defaultFace != null) _renderer.sprite = defaultFace;
    }

    void Update()
    {
        if (_isShivering)
        {
            Vector3 shake = Random.insideUnitCircle * shakeAmount;
            transform.position = _basePos + shake;
        }
    }

    public void SetShiverState(bool enable)
    {
        if (_isShivering == enable) return;

        _isShivering = enable;

        if (_isShivering)
        {
            
            StopAllCoroutines(); 

            transform.rotation = Quaternion.Euler(0, 0, 0);

            if (_audioSource.isPlaying) _audioSource.Stop();//ｼﾞｮﾜﾖｰ停止

            if (_renderer != null && shiverFace != null)
            {
                _renderer.sprite = shiverFace;
            }
            if(Random.Range(0, 1.0f) < 0.1f){
                if (shiverVoices[1] != null)
                {
                    _audioSource.PlayOneShot(shiverVoices[1]);
                }
            }else{
                if (shiverVoices[0] != null)
                {
                    _audioSource.PlayOneShot(shiverVoices[0]);
                }

            }
            //if (_renderer != null && defaultFace != null) _renderer.sprite = defaultFace;
        }
        else
        {
            transform.position = _basePos;
            StartCoroutine(IdleRoutine());
        }
    }

    IEnumerator IdleRoutine()
    {
        yield return new WaitForSeconds(0.2f);

        while (true)
        {
            if (RandomBool())
            {
                
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            yield return StartCoroutine(PerformJump());//ジャンプ

            if (!_audioSource.isPlaying && voices != null && voices.Length > 0 && Random.value > 0.3f)
            {
                _audioSource.PlayOneShot(voices[Random.Range(0, voices.Length)]);
            }//ｼﾞｮﾜﾖｰｼﾞｮﾜﾖ

            
            if (_renderer != null && faceVariations != null && faceVariations.Length > 0)
            {
                _renderer.sprite = faceVariations[Random.Range(0, faceVariations.Length)];
            }// ^ワ^

            //yield return new WaitForSeconds(0.2f);
            //if (_renderer != null && defaultFace != null) _renderer.sprite = defaultFace;

            float wait = Random.Range(0.0f, 0.5f);
            if(wait < 0.2f) wait = 0.0f;

            yield return new WaitForSeconds(wait);
        }
    }

    IEnumerator PerformJump()
    {
        float elapsed = 0f;
        //float iscontinue = 1.0f;

       // {
       // iscontinue = Random.Range(0.0f, 1.0f);

      //  jumpHeight = Random.Range(0.1f,2.2f);
        
        while (elapsed < jumpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / jumpDuration; 

            
            float yOffset = jumpHeight * 4f * t * (1f - t);

            // 基準位置 + 高さ
            transform.position = _basePos + Vector3.up * yOffset;

            yield return null; // 1フレーム待つ
        //}

        //elapsed = 0f;

       // }while(iscontinue > 3.0f);
        
        transform.position = _basePos;
    }
}

public static bool RandomBool()
    {
        return Random.Range(0, 2) == 0;
    }
}