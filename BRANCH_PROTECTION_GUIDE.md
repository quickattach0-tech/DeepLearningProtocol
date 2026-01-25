# Branch Protection Guide

## Protecting Main Branch from Force Pushing

This guide explains how to set up branch protection rules for the main branch to prevent destructive operations.

### Why Protect the Main Branch?

- **Prevent accidental force pushes** that could delete commits
- **Enforce code review** via required pull requests
- **Maintain code quality** with required status checks
- **Ensure CI/CD** always passes before merge
- **Create audit trail** of all changes

---

## Setup Steps

### Step 1: Go to Repository Settings

1. Navigate to your GitHub repository: https://github.com/quickattach0-tech/DeepLearningProtocol
2. Click **Settings** (top right)
3. Select **Branches** from left sidebar

### Step 2: Add Branch Protection Rule

1. Click **Add rule** button
2. Enter branch name pattern: `main`
3. Configure the following options:

### Step 3: Configure Protection Rules

#### **Require a pull request before merging** ✅
- ☑ Require pull request reviews before merging
- ☑ Require status checks to pass before merging
- ☑ Require branches to be up to date before merging
- Number of required reviewers: **1** (or more)

#### **Restrict who can push** ✅
- ☑ Dismiss stale pull request approvals when new commits are pushed
- ☑ Require branches to be up to date before merging

#### **Enforce Admin Settings** ✅
- ☑ Include administrators in restrictions
- ☑ Prevent force pushes
  - Select: "Dismiss stale pull request approvals when new commits are pushed"
- ☑ Allow force pushes
  - Select: "Nobody" (prevents all force pushes)

#### **Additional Security** ✅
- ☑ Require status checks to pass
  - Check: `Build and Test` (CI/CD pipeline)
- ☑ Require a pull request before merging
- ☑ Require branches to be up to date before merging
- ☑ Require conversation resolution before merging

### Step 4: Save Rules

Click **Create** button to apply branch protection rules.

---

## What This Prevents

After branch protection is enabled on `main`:

❌ **Direct force pushes:** `git push -f origin main`  
❌ **Bypassing reviews:** Can't merge without PR approval  
❌ **Skipping tests:** Must pass all CI/CD checks  
❌ **Stale branches:** Must be updated with base branch  
❌ **Accidental deletions:** Can't delete protected branch  

✅ **Still allowed:** Pull requests with proper reviews and passing tests

---

## Workflow with Branch Protection

```
1. Create feature branch from main
   git checkout -b feature/my-feature

2. Make changes and commit
   git commit -m "feat: add new feature"

3. Push to remote
   git push -u origin feature/my-feature

4. Create Pull Request on GitHub
   (Automatically shown after push)

5. Pass CI/CD checks
   (Status checks must pass)

6. Request review
   (At least 1 approver)

7. Address feedback
   (Push additional commits if needed)

8. Merge when approved
   (Rebase, squash, or merge commit)

9. Delete feature branch
   (After merge is complete)
```

---

## GitHub CLI Alternative

While `gh` doesn't directly create protection rules, you can view and manage them:

```bash
# View branch protection status
gh api repos/quickattach0-tech/DeepLearningProtocol/branches/main/protection

# List all protection rules
gh api repos/quickattach0-tech/DeepLearningProtocol/branches \
  --jq '.[].name'
```

---

## Common Issues

### Issue: "Force push still works"
- Ensure "Prevent force pushes" is set to "Nobody"
- Check if you're a repository admin (admins bypass rules by default)
- Solution: Enable "Include administrators" option

### Issue: "Can't merge due to stale branch"
- Update your branch with `git pull origin main`
- Or GitHub provides "Update branch" button on PR

### Issue: "Status checks failing"
- Fix errors in code
- Wait for CI/CD pipeline to complete
- Or contact maintainers if check is misconfigured

---

## Best Practices

1. **Always use PRs** - Even as repo owner, use PRs for visibility
2. **Require reviews** - Catch bugs early with peer review
3. **Maintain CI/CD** - Ensure tests always pass
4. **Document changes** - Use meaningful PR descriptions
5. **Keep history clean** - Use squash merge for cleaner history
6. **Monitor protection rules** - Review quarterly for effectiveness

---

## For Collaborators

When contributing to this project:

1. ✅ **DO:** Create feature branch from main
2. ✅ **DO:** Submit PR with description
3. ✅ **DO:** Pass all status checks
4. ✅ **DO:** Request review from maintainers
5. ✅ **DO:** Address feedback promptly

6. ❌ **DON'T:** Force push to any branch
7. ❌ **DON'T:** Commit directly to main
8. ❌ **DON'T:** Ignore failing CI/CD checks
9. ❌ **DON'T:** Merge without approval
10. ❌ **DON'T:** Bypass protection rules

---

## Reference Links

- [GitHub Branch Protection Documentation](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches)
- [Configuring required status checks](https://docs.github.com/en/repositories/configuring-branches-and-merges-in-your-repository/managing-protected-branches/about-protected-branches#about-branch-protection-rules)
- [Allowing auto-merge for pull requests](https://docs.github.com/en/pull-requests/collaborating-with-pull-requests/incorporating-changes-from-a-pull-request/automatically-merging-a-pull-request)

---

**Status:** This guide documents the recommended branch protection setup for production-ready projects.  
**Last Updated:** January 25, 2026  
**Repository:** [Deep Learning Protocol](https://github.com/quickattach0-tech/DeepLearningProtocol)
