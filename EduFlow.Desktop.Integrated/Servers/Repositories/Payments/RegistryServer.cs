﻿using EduFlow.BLL.DTOs.Payments.Registry;
using EduFlow.Desktop.Integrated.Api.Auth;
using EduFlow.Desktop.Integrated.Security;
using EduFlow.Desktop.Integrated.Servers.Interfaces.Payments;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace EduFlow.Desktop.Integrated.Servers.Repositories.Payments;

public class RegistryServer : IRegistryServer
{
    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.DeleteAsync(client.BaseAddress);

            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        catch(Exception ex)
        {
            return false;
        }
    }

    public async Task<List<RegistryForResultDto>> FilterAsync(RegistryForFilterDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries/filter");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            var result = await response.Content.ReadAsStringAsync();

            List<RegistryForResultDto> registries = JsonConvert.DeserializeObject<List<RegistryForResultDto>>(result)!;

            return registries;
        }
        catch(Exception ex)
        {
            return new List<RegistryForResultDto>();
        }
    }

    public async Task<List<RegistryForResultDto>> GetAllAsync()
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            List<RegistryForResultDto> registries = JsonConvert.DeserializeObject<List<RegistryForResultDto>>(result)!;

            return registries;
        }
        catch(Exception ex)
        {
            return new List<RegistryForResultDto>();
        }
    }

    public async Task<RegistryForResultDto> GetByIdAsync(long id)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries/{id}");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync(client.BaseAddress);

            var result = await response.Content.ReadAsStringAsync();

            RegistryForResultDto registry = JsonConvert.DeserializeObject<RegistryForResultDto>(result)!;

            return registry;
        }
        catch(Exception ex)
        {
            return new RegistryForResultDto();
        }
    }

    public async Task<long> IncomeAsync(RegistryForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;

            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries/income");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return long.Parse(result);
            }
            else
                return 0;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }

    public async Task<long> OutlayAsync(RegistryForCreateDto dto)
    {
        try
        {
            HttpClient client = new HttpClient();
            var token = IdentitySingelton.GetInstance().Token;
            
            client.BaseAddress = new Uri($"{AuthApi.BASE_URL}/api/registries/outlay");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var request = new HttpRequestMessage(HttpMethod.Post, client.BaseAddress)
            {
                Content = new StringContent(
                    JsonConvert.SerializeObject(dto),
                    System.Text.Encoding.UTF8,
                    "application/json")
            };
            
            var response = await client.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return long.Parse(result);
            }
            else
                return 0;
        }
        catch(Exception ex)
        {
            return 0;
        }
    }
}
