using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using Ocelot.Middleware;
using Ocelot.Multiplexer;

public class CustomAggregator : IDefinedAggregator
{
    public async Task<DownstreamResponse> Aggregate(List<HttpContext> responses)
    {
        var categoryResponse = responses.FirstOrDefault(r => r.Request.Path.Value.Contains("categories"));
        var reviewResponse = responses.FirstOrDefault(r => r.Request.Path.Value.Contains("reviews"));

        // Приведення Items["DownstreamResponse"] до DownstreamResponse
        var categoryContent = categoryResponse?.Items["DownstreamResponse"] as DownstreamResponse;
        var reviewContent = reviewResponse?.Items["DownstreamResponse"] as DownstreamResponse;

        if (categoryContent == null || reviewContent == null)
        {
            return new DownstreamResponse(new StringContent("Error retrieving responses"), System.Net.HttpStatusCode.InternalServerError, new List<Header>(), "Error");
        }

        var categoryJson = await categoryContent.Content.ReadAsStringAsync();
        var reviewJson = await reviewContent.Content.ReadAsStringAsync();

        // Об'єднуємо JSON-об'єкти
        var aggregatedContent = new JObject
        {
            ["Category"] = JToken.Parse(categoryJson),
            ["Review"] = JToken.Parse(reviewJson)
        }.ToString();

        var stringContent = new StringContent(aggregatedContent, Encoding.UTF8, "application/json");

        return new DownstreamResponse(stringContent, System.Net.HttpStatusCode.OK, new List<Header>(), "OK");
    }
}
