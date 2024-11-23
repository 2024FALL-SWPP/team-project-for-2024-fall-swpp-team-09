using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Anomaly4Manager : MonoBehaviour
{
   private GameObject mainLaptop;
   private List<GameObject> laptopList = new List<GameObject>();

   void OnEnable()
   {
       mainLaptop = GameObject.FindWithTag("Laptop");
       if (mainLaptop == null)
        {
           Debug.LogError("Cannot find object with Laptop tag!");
           return;
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
           return;
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

       StartCoroutine(ScreenChangeSequence());
   }

   private IEnumerator ScreenChangeSequence()
   {
       yield return new WaitForSeconds(5f);
       Debug.Log("First screen change after 5 seconds");

       // mainLaptop 화면 변경
       LaptopScreenController mainController = mainLaptop.GetComponent<LaptopScreenController>();
       if(mainController != null)
       {
           mainController.ChangeScreen(10);
           Debug.Log("Changed main laptop screen to 10");
           // modified by 신채환
           // 0단계 추가에 따른 화면 색인 변경 반영
       }

       // 5개 랩탑 화면 변경
       foreach(GameObject laptop in laptopList)
       {
           LaptopScreenController controller = laptop.GetComponent<LaptopScreenController>();
           if(controller != null)
           {
               controller.ChangeScreen(11);
               Debug.Log($"Changed screen to 11 for laptop: {laptop.name}");
               // modified by 신채환
               // 0단계 추가에 따른 화면 색인 변경 반영
           }
       }

       yield return new WaitForSeconds(10f);
       Debug.Log("Final screen change after 10 seconds");

       // 모든 랩탑 원래 화면으로 복귀
       int originalScreen = GameManager.Instance.GetCurrentStage();
            // modified by 신채환
            // 0단계 추가에 따른 화면 색인 변경 반영
       
       // mainLaptop 포함
       if(mainController != null)
       {
           mainController.ChangeScreen(originalScreen);
           Debug.Log("Restored main laptop screen");
       }

       // 5개 랩탑
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