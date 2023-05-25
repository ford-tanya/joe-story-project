using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    private const string apiUrl = "http://localhost:8000/ticket/getTicket";

    IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);

        // ส่งคำขอไปยังเว็บเซอร์วิส
        yield return www.SendWebRequest();

        // ตรวจสอบสถานะการเรียกใช้งาน
        if (www.result == UnityWebRequest.Result.Success)
        {
            string responseText = www.downloadHandler.text;
            ResponseData responseData = JsonUtility.FromJson<ResponseData>(responseText);

            if (responseData.status == "success")
            {
                foreach (TicketData ticketData in responseData.data)
                {
                    Debug.Log("Ticket ID: " + ticketData.ticket_id);
                    Debug.Log("Title: " + ticketData.title);
                    Debug.Log("Update Time: " + ticketData.update_time);
                    Debug.Log("Status: " + ticketData.status_name);
                }
            }
        }
        else
        {
            // เกิดข้อผิดพลาดในการเรียกใช้งาน
            Debug.Log("Error: " + www.error);
        }
    }

    [System.Serializable]
    public class ResponseData
    {
        public string status;
        public TicketData[] data;
    }

    [System.Serializable]
    public class TicketData
    {
        public int ticket_id;
        public string title;
        public string update_time;
        public string status_name;
    }

}
