using Newtonsoft.Json;

namespace Questao2
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            string teamName = "Paris Saint-Germain";
            int year = 2013;
            int totalGoals = await GetTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals + " goals in " + year);

            teamName = "Chelsea";
            year = 2014;
            totalGoals = await GetTotalScoredGoals(teamName, year);

            Console.WriteLine("Team " + teamName + " scored " + totalGoals + " goals in " + year);
        }

        public static async Task<int> GetTotalScoredGoals(string team, int year)
        {
            int totalGoals = 0;
            using (HttpClient client = new())
            {
                async Task<int> GetGoals(string url)
                {
                    int goals = 0;
                    int page = 1;
                    bool hasMorePages = true;

                    while (hasMorePages)
                    {
                        var response = await client.GetStringAsync($"{url}&page={page}");
                        var data = JsonConvert.DeserializeObject<ApiResponse>(response);

                        foreach (var match in data!.Data!)
                        {
                            goals += int.Parse(match.TeamGoals);
                        }

                        hasMorePages = page < data.TotalPages;
                        page++;
                    }

                    return goals;
                }

                string urlTeam1 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}";
                totalGoals += await GetGoals(urlTeam1);

                string urlTeam2 = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}";
                totalGoals += await GetGoals(urlTeam2);
            }

            return totalGoals;
        }

        public class ApiResponse
        {
            [JsonProperty("data")]
            public Match[]? Data { get; set; }

            [JsonProperty("total_pages")]
            public int? TotalPages { get; set; }
        }

        public class Match
        {
            [JsonProperty("team1")]
            public string? Team1 { get; set; }

            [JsonProperty("team2")]
            public string? Team2 { get; set; }

            [JsonProperty("team1goals")]
            public string? Team1Goals { get; set; }

            [JsonProperty("team2goals")]
            public string? Team2Goals { get; set; }

            public string TeamGoals => Team1Goals ?? Team2Goals!;
        }
    }
}