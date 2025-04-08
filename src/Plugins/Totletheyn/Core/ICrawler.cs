using System.Collections.Generic;

namespace Totletheyn.Core;

public interface ICrawler
{
    bool IsNewIssueAvailable();
    IEnumerable<Issue> GetNewIssues(List<string> lastIssueTitles);
}