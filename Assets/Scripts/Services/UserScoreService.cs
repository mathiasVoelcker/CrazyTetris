using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

    public class UserScoreService
    {
        private UnityWebRequest _webReq;
        
        private HttpClient _client;

        private readonly string baseUrl = "https://crazy-tetris-api.herokuapp.com/Score/";

        public UserScoreService()
        {
            _client = new HttpClient();
        }

        public List<UserScore> GetHighestScore()
        {
            List<UserScore> userScoreList = new List<UserScore>();
            HttpResponseMessage response = _client.GetAsync(baseUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                userScoreList = JsonConvert.DeserializeObject<List<UserScore>>(responseString);
            }
            else {
                Debug.Log("ERROR!");
            }
            return userScoreList;
        }

        public List<UserScore> GetScores(int id)
        {
            List<UserScore> userScoreList = new List<UserScore>();
            HttpResponseMessage response = _client.GetAsync(baseUrl + id).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                userScoreList = JsonConvert.DeserializeObject<List<UserScore>>(responseString);
            }
            else {
                Debug.Log("ERROR!");
            }
            return userScoreList;
        }

        public int SaveScore(string user, int score)
        {
            var userScore = new UserScore() {
                Username = user,
                Score = score
            };
            var json = JsonConvert.SerializeObject(userScore);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            var response = _client.PostAsync(baseUrl, data).Result;
            // if (response.IsCompleted)
            // {
            var responseString = response.Content.ReadAsStringAsync().Result;
            return int.Parse(responseString);
            // }
        }
    }

