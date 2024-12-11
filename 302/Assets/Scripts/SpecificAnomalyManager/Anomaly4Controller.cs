using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Anomaly4Controller : MonoBehaviour
{
    private GameObject mainLaptop;
    private List<GameObject> laptopList = new List<GameObject>();

    void OnEnable()
    {
        StartCoroutine(InitializeSequence());
    }

    private IEnumerator InitializeSequence()
    {
        // 1단계: LaptopFaceController 삭제
        LaptopFaceController[] laptopFaces = FindObjectsOfType<LaptopFaceController>();
        foreach (LaptopFaceController laptopFace in laptopFaces)
        {
            Destroy(laptopFace.gameObject);
        }
        
        // 약간의 대기 시간을 주어 삭제가 완료되도록 함
        yield return new WaitForSeconds(0.1f);

        // 2단계: 메인 랩탑 찾기 및 설정
        mainLaptop = GameObject.FindWithTag("Laptop");
        if (mainLaptop == null)
        {
            Debug.LogError("Cannot find object with Laptop tag!");
            yield break;
        }

        SleepLaptop sleepLaptop = mainLaptop.GetComponentInChildren<SleepLaptop>();
        if (sleepLaptop != null)
        {
            sleepLaptop.setStageClear = true;
            Debug.Log("Set setStageClear to true for mainLaptop");
        }
        else
        {
            Debug.LogError("Cannot find SleepLaptop component in mainLaptop children");
        }

        // 3단계: 추가 랩탑 활성화 및 설정
        LaptopScreenController[] allControllers = Resources.FindObjectsOfTypeAll<LaptopScreenController>();
        
        int count = 0;
        foreach (LaptopScreenController controller in allControllers)
        {
            if (!controller.gameObject.CompareTag("Laptop") && count < 5)
            {
                controller.gameObject.SetActive(true);
                laptopList.Add(controller.gameObject);
                count++;
                Debug.Log($"Activated and added laptop: {controller.gameObject.name}");
            }
        }

        if (laptopList.Count != 5)
        {
            Debug.LogError($"Expected 5 laptops, but found {laptopList.Count}");
            yield break;
        }

        foreach(GameObject laptop in laptopList)
        {
            LaptopScreenController controller = laptop.GetComponent<LaptopScreenController>();
            if(controller != null)
            {
                controller.ChangeScreen(GameManager.Instance.GetCurrentStage() - 1);
                Debug.Log($"Changed initial screen for laptop: {laptop.name}");
            }
        }

        // 4단계: 랜덤 위치 스왑
        int randomNumber = Random.Range(0, 6);
        Debug.Log($"Random number selected: {randomNumber}");

        if (randomNumber > 0)
        {
            GameObject selectedLaptop = laptopList[randomNumber - 1];
            
            Vector3 tempPosition = mainLaptop.transform.position;
            mainLaptop.transform.position = selectedLaptop.transform.position;
            selectedLaptop.transform.position = tempPosition;
            
            Debug.Log($"Swapped positions between main laptop and laptop at index {randomNumber - 1}");
        }
        else
        {
            Debug.Log("Random number was 0, no position swap occurred");
        }

        // 5단계: 화면 변경 시퀀스 시작
        StartCoroutine(ScreenChangeSequence());
    }

    private IEnumerator ScreenChangeSequence()
    {
        // 기존 ScreenChangeSequence 코드는 그대로 유지
        yield return new WaitForSeconds(5f);
        Debug.Log("First screen change after 5 seconds");

        LaptopScreenController mainController = mainLaptop.GetComponent<LaptopScreenController>();
        if(mainController != null)
        {
            mainController.ChangeScreen(10);
            Debug.Log("Changed main laptop screen to 10");
        }

        foreach(GameObject laptop in laptopList)
        {
            LaptopScreenController controller = laptop.GetComponent<LaptopScreenController>();
            if(controller != null)
            {
                controller.ChangeScreen(11);
                Debug.Log($"Changed screen to 11 for laptop: {laptop.name}");
            }
        }

        yield return new WaitForSeconds(10f);
        Debug.Log("Final screen change after 10 seconds");

        int originalScreen = GameManager.Instance.GetCurrentStage();
        
        if(mainController != null)
        {
            mainController.ChangeScreen(originalScreen);
            Debug.Log("Restored main laptop screen");
        }

        foreach(GameObject laptop in laptopList)
        {
            LaptopScreenController controller = laptop.GetComponent<LaptopScreenController>();
            if(controller != null)
            {
                controller.ChangeScreen(originalScreen);
                Debug.Log($"Restored screen for laptop: {laptop.name}");
            }
        }
    }
}