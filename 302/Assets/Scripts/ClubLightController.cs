using UnityEngine;

public class ClubLightController : MonoBehaviour 
{
    private Light clubLight;
    
    public enum LightMode { ColorChange, Strobe, AudioReactive, Rotation }
    
    [Header("Light Mode")]
    public LightMode currentMode = LightMode.ColorChange;
    
    [Header("Common Settings")]
    public float baseIntensity = 5f; // 강도 증가
    public float lightRange = 15f; // 범위 증가
    public float spotAngle = 45f; // 각도 증가
    
    [Header("Color Change Settings")]
    public float colorChangeSpeed = 2f; // 속도 증가
    public bool useRandomColors = true; // 랜덤 색상 활성화
    public Color[] customColors;
    private int currentColorIndex = 0;
    private float colorTimer = 0f;
    
    [Header("Rotation Settings")]
    public bool enableRotation = true;
    public float rotationSpeed = 100f; // 회전 속도 증가
    public Vector3 rotationAxis = Vector3.up;

    void Start()
    {
        // 라이트 컴포넌트 설정
        clubLight = gameObject.GetComponent<Light>();
        if (clubLight == null)
        {
            clubLight = gameObject.AddComponent<Light>();
            Debug.Log("Light component added");
        }
            
        // 기본 라이트 설정
        clubLight.type = LightType.Spot;
        clubLight.range = lightRange;
        clubLight.spotAngle = spotAngle;
        clubLight.intensity = baseIntensity;
        
        // 초기 회전 설정 
        transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
        
        Debug.Log("Light initialized - Type: " + clubLight.type);
        
        // 기본 컬러 설정
        if (customColors == null || customColors.Length == 0)
        {
            customColors = new Color[] { 
                Color.red, Color.blue, Color.green, Color.yellow, Color.magenta 
            };
        }
    }
    
    void Update()
    {
        // 회전 처리
        if (enableRotation)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
            Debug.Log("Rotating: " + transform.rotation.eulerAngles);
        }
        
        switch (currentMode)
        {
            case LightMode.ColorChange:
                UpdateColorChange();
                break;
        }
    }
    
    void UpdateColorChange()
    {
        if (useRandomColors)
        {
            float hue = Mathf.PingPong(Time.time * colorChangeSpeed, 1f);
            Color newColor = Color.HSVToRGB(hue, 1f, 1f);
            clubLight.color = newColor;
            Debug.Log("Color changed to: " + newColor);
        }
        else
        {
            colorTimer += Time.deltaTime * colorChangeSpeed;
            if (colorTimer >= 1f)
            {
                colorTimer = 0f;
                currentColorIndex = (currentColorIndex + 1) % customColors.Length;
                Debug.Log("Switching to color index: " + currentColorIndex);
            }
            
            Color targetColor = customColors[currentColorIndex];
            Color nextColor = customColors[(currentColorIndex + 1) % customColors.Length];
            clubLight.color = Color.Lerp(targetColor, nextColor, colorTimer);
        }
    }
}