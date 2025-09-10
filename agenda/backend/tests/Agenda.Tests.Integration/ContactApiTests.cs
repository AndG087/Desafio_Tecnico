// Agenda.Tests.Integration/ContactsApiTests.cs
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

public class ContactsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    public ContactsApiTests(WebApplicationFactory<Program> factory) => _factory = factory;

    [Fact]
    public async Task Create_and_get_contact()
    {
        var client = _factory.CreateClient();
        // obter token (se preferir, bypass auth no ambiente de test)
        var token = (await client.PostAsJsonAsync("/api/auth/login", new { email="admin@local", password="Admin@123" }))
            .Content.ReadFromJsonAsync<dynamic>().Result.token.ToString();
        client.DefaultRequestHeaders.Authorization = new("Bearer", token);

        var create = await client.PostAsJsonAsync("/api/contacts", new { name="Ana", email="ana@mail.com", phone="+5581999999999" });
        create.EnsureSuccessStatusCode();
    }
}
