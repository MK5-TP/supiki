using UnityEngine;

public class managerSupiki : MonoBehaviour
{
    
    public enum GameMode
    {
        Idle,       
        Shivering,  
        Squash      
    }

    [Header("ｽﾋﾟｷ状態")]
    public GameObject idleObj;        
    public idleSupiki idleScript;     
    public squashSupiki squashScript; 

    [Header("全体設定")]
    public float idleThreshold = 3.0f; // 放置時間

    private GameMode _currentMode = GameMode.Idle;
    private float _lastInteractionTime;
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
        
        
        ChangeMode(GameMode.Idle);
        _lastInteractionTime = Time.time;
    }

    void Update()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            _lastInteractionTime = Time.time;
            
            
            switch (_currentMode)
            {
                case GameMode.Idle:
                    
                    ChangeMode(GameMode.Shivering);
                    break;

                case GameMode.Shivering:
                    
                    if (CheckTapSupiki())
                    {
                        
                        ChangeMode(GameMode.Squash);
                        squashScript.ForceStartDrag();                     }
                    break;

                case GameMode.Squash:
                    
                    break;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _lastInteractionTime = Time.time;
        }

        if (_currentMode != GameMode.Idle)
        {

            if (!Input.GetMouseButton(0) && (Time.time - _lastInteractionTime > idleThreshold))
            {
                ChangeMode(GameMode.Idle);
            }
        }
    }

    
    bool CheckTapSupiki()
    {
        Vector3 mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        

        float dist = Vector2.Distance(mousePos, idleObj.transform.position);
        

        return dist <= squashScript.grabRadius;
    }


    void ChangeMode(GameMode nextMode)
    {
        _currentMode = nextMode;

        switch (nextMode)
        {
            case GameMode.Idle:

                squashScript.gameObject.SetActive(false);
                idleObj.SetActive(true);
                
                if (idleScript != null) idleScript.SetShiverState(false);


                if (squashScript.rootBone != null)
                {
                    idleObj.transform.position = squashScript.transform.position;
                    idleObj.transform.rotation = Quaternion.identity;
                }
                break;

            case GameMode.Shivering:

                squashScript.gameObject.SetActive(false);
                idleObj.SetActive(true); 
                

                if (idleScript != null) idleScript.SetShiverState(true);
                break;

            case GameMode.Squash:

                idleObj.SetActive(false); 
                squashScript.gameObject.SetActive(true); 
                break;
        }
    }
}