using Cloudcrate.AspNetCore.Blazor.Browser.Storage;
using Microsoft.AspNetCore.Blazor;
using Sotsera.Blazor.Toaster;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherOutlet.Shared;
using WeatherOutlet.Shared.Places;
using WeatherOutlet.Shared.Models;

namespace WeatherOutlet.Client.Services
{
    public class AppState
    {
        private readonly LocalStorage localStorage;
        private readonly IToaster toaster;

        public List<string> SearchHistory { get; set; }

        public PlaceData PlaceData { get; set; }
        public List<TodoItem> Todos { get; set; }

        public bool SearchInProgress { get; private set; }
        public HttpClient http { get; }

        public event Action OnAppStateChanged;
        public delegate Task OnMarkTodoAsCompletedHandler(TodoItem todoItem);

        public AppState(LocalStorage localStorage, IToaster toaster, HttpClient httpClient)
        {
            this.localStorage = localStorage;
            this.toaster = toaster;
            http = httpClient;
            GetSearchHistory();
        }

        public async Task LoadTodos()
        {
            SearchInProgress = true;
            NotifyStateChanged();

            Todos = await http.GetJsonAsync<List<TodoItem>>($"/api/todos");

            SearchInProgress = false;
            NotifyStateChanged();
        }

        public async Task OnMarkTodoAsCompleted(TodoItem todoItem)
        {
            todoItem.IsCompleted = true;
            todoItem.CompletedAt = DateTime.Now;

            await http.PutJsonAsync<TodoItem>($"/api/todos/{todoItem.Id}", todoItem);
            await LoadTodos();
        }

        public async Task Search(SearchCriteria searchCriteria)
        {
            SearchInProgress = true;
            NotifyStateChanged();

            InsertPlaceIntoSearchHistoryIfNotExist(searchCriteria.Place);

            PlaceData = await http.GetJsonAsync<PlaceData>($"/api/places/{searchCriteria.Place}");

            SearchInProgress = false;
            NotifyStateChanged();
        }

        private void GetSearchHistory()
        {
            SearchHistory = localStorage.GetItem<List<string>>("searchHistory") ?? new List<string>();
            NotifyStateChanged();
        }

        private void SetSearchHistory()
        {
            localStorage.SetItem("searchHistory", SearchHistory);
        }

        public void ClearSearchHistory()
        {
            SearchHistory = new List<string>();
            SetSearchHistory();
            NotifyStateChanged();

            toaster.Info("Search history has been cleared!");
        }

        private void InsertPlaceIntoSearchHistoryIfNotExist(string searchQuery)
        {
            if (string.IsNullOrWhiteSpace(searchQuery))
                return;

            if (SearchHistory.Contains(searchQuery))
                return;

            SearchHistory.Add(searchQuery);

            toaster.Info($"Place '{searchQuery}' is added to your search history");

            NotifyStateChanged();
            SetSearchHistory();
        }

        private void NotifyStateChanged() => OnAppStateChanged?.Invoke();
    }
}
