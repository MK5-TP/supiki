using UnityEngine;

public class squashSupiki : MonoBehaviour
{
    [Header("操作対象")]
    public Transform rootBone;        // 根元のボーン (bone_1)
    public Rigidbody2D headBone;      // キャラの頭のボーン (bone_2)

    [Header("挙動設定")]
    public float squashForce = 40f;   // 追従する強さ
    public float limitpower = 0.1f;
    public float grabRadius = 1.5f;
    public AudioClip crySound1;        // ｱｰｳ！
    public AudioClip crySound2;        // ｳﾜｧ！

    private AudioSource _audioSource;
    private bool _isDragging = false;
    private Camera _mainCamera;
    
    private Vector3 _railDirection;

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null) _audioSource = gameObject.AddComponent<AudioSource>();
        
        _mainCamera = Camera.main;

        
        // 斜め
        if (rootBone != null && headBone != null)
        {
            Vector3 diff = headBone.transform.position - rootBone.position;
            _railDirection = diff.normalized; // 方向だけ（長さ1のベクトル）にする
        }
    }

    void Update()
    {
        // --- 入力検知 ---
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = GetWorldMousePos();
            float distance = Vector2.Distance(mousePos, headBone.transform.position);

            if (distance <= grabRadius)
            {
                _isDragging = true;
                if (crySound1 != null) _audioSource.PlayOneShot(crySound1);
            }

        }
        else if (Input.GetMouseButtonUp(0))
        {
            if(_isDragging){
                if (crySound2 != null) _audioSource.PlayOneShot(crySound2);
            }
            _isDragging = false;
        }

       
        if (_isDragging && rootBone != null)
        {
            
            Vector3 mousePos = GetWorldMousePos();
            mousePos.z = 0; 

            Vector3 rootToMouse = mousePos - rootBone.position;         
            float distanceOnRail = Vector3.Dot(rootToMouse, _railDirection);

            distanceOnRail = Mathf.Max(distanceOnRail, limitpower); 
            Vector3 targetPos = rootBone.position + (_railDirection * distanceOnRail);

            
            Vector2 direction = targetPos - headBone.transform.position;
            headBone.velocity = direction * squashForce;
        }
    }
    Vector3 GetWorldMousePos()
    {
        Vector3 p = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        p.z = 0;
        return p;
    }

    void OnDrawGizmos()
    {
        if (headBone != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(headBone.transform.position, grabRadius);
        }
    }
}