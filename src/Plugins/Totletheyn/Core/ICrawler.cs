using System.Collections.Generic;

namespace Totletheyn.Core;

public interface ICrawler
{
    bool IsNewIssueAvailable(Issue? lastIssue);
    IEnumerable<Issue> GetNewIssues(Issue? lastIssue);
}