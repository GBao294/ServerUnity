using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class APIScript : MonoBehaviour
{
   // xoa player khoi database 
   public async void SendDeleteRequest()
    {
        using (HttpClient client = new HttpClient())
        {
          
                HttpResponseMessage response = await client.DeleteAsync($"http://localhost:3000/Player/1");
                response.EnsureSuccessStatusCode();
                // Kiểm tra phản hồi từ API và trả về kết quả tùy thuộc vào yêu cầu của bạn
               // return response.IsSuccessStatusCode;
         
        }
    }

    async Task<HttpResponseMessage> SendPostRequest(string apiUrl, object data)
    {
        using (HttpClient client = new HttpClient())
        {
            try
            {
                string jsonData = JsonConvert.SerializeObject(data);
                HttpContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                response.EnsureSuccessStatusCode();

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi gửi yêu cầu POST: " + ex.Message);
            }
        }
    }

    // Đoạn code sử dụng phương thức SendPostRequest để gửi dữ liệu mới lên API
    // Đưa thông tin của người chơi mơi lên API
    public async void SendNewData(string name, string password, ushort lv)
    {
        var newPost = new
        {
            name = name,
            password = password,
            lv = lv
        };

        HttpResponseMessage response = await SendPostRequest("http://localhost:3000/Player", newPost);
    }
    
    
    
    //Hàm để so sánh thông tin với dữ liệu trên API
    async void GET(string name, string pass)
    {
        using (HttpClient client = new HttpClient())
        {
          
                HttpResponseMessage response = await client.GetAsync("http://localhost:3000/Player");
                response.EnsureSuccessStatusCode();
                string responseContent = await response.Content.ReadAsStringAsync();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(responseContent);
                
                foreach (User user in users)
                {
                    if (user.name == name && user.password ==pass)
                    {
                        

                    }
                }
           
        
        }
    }

    public class User
    {

       
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("password")]
        public string password { get; set; }

        [JsonProperty("lv")]
        public int lv { get; set; }



    }
}

