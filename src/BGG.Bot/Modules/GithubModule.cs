using Discord;
using Discord.Addons.Interactive;
using Discord.Commands;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BGG.Bot.Modules
{
  public class GithubModule : InteractiveBase<SocketCommandContext>
  {
    private readonly IGitHubClient _gitHubClient;

    public GithubModule(IGitHubClient gitHubClient)
    {
      _gitHubClient = gitHubClient;
    }

    [Command("suggest")]
    public async Task Suggestion(string suggestion, [Remainder] string detail = "")
    {
      var issue = new NewIssue(suggestion) { Body = detail };
      issue.Labels.Add("enhancement");

      var createdIssue = await _gitHubClient.Issue.Create(361060222, issue);
      await ReplyAsync($"Successfully created issue #{createdIssue.Number}\r\n{createdIssue.HtmlUrl}");
    }

    [Command("bug")]
    public async Task Bug(string bug,[Remainder] string detail = "")
    {
      var issue = new NewIssue(bug) { Body = detail };
      issue.Labels.Add("bug");

      var createdIssue = await _gitHubClient.Issue.Create(361060222, issue);
      await ReplyAsync($"Successfully created issue #{createdIssue.Number}\r\n{createdIssue.HtmlUrl}");
    }

    [Command("issues")]
    public async Task Issues()
    {
      var issues = await _gitHubClient.Issue.GetAllForRepository(361060222);

      var description = new StringBuilder().AppendJoin(Environment.NewLine, issues.Select(i => Format.Url(i.Title, i.HtmlUrl))).ToString();

      var embed = new EmbedBuilder()
        .WithTitle($"There are {issues.Count} open issues")
        .WithDescription(description)
        .Build();

      await ReplyAsync(embed: embed);
    }
  }
}
