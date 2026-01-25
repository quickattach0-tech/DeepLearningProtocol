# GitHub Wiki Setup Instructions

This document explains how to set up the GitHub Wiki for the Deep Learning Protocol project.

## What is GitHub Wiki?

GitHub Wiki is a built-in documentation system where you can create pages and organize them hierarchically. It's separate from your main repository code but lives alongside your project.

## Setup Steps

### Step 1: Enable Wiki in Repository Settings

1. Go to your repository: `https://github.com/quickattach0-tech/DeepLearningProtocol`
2. Click **Settings** (top-right)
3. Scroll down to **Features** section
4. ✓ Check **Wiki** (should be enabled by default)
5. Click **Save**

### Step 2: Access the Wiki

1. In your repository, click the **Wiki** tab (right side, next to "Discussions")
2. Click **Create the first page** (if empty)
3. GitHub will create a default Home page

### Step 3: Add Wiki Pages Manually

#### Method A: Via GitHub Web Interface (Recommended for first setup)

1. Click **Wiki** tab
2. Click **New Page** button
3. Enter **Page Title:** `Getting Started`
4. Enter **Page Content:** (copy from [docs/Getting-Started.md](docs/Getting-Started.md))
5. Click **Save Page**

Repeat for each page:
- Getting Started
- Architecture
- Testing
- DLP Guide

#### Method B: Via Git (Advanced - for bulk import)

```bash
# Clone the wiki repository
git clone https://github.com/quickattach0-tech/DeepLearningProtocol.wiki.git
cd DeepLearningProtocol.wiki

# Copy pages from docs folder
cp ../docs/Getting-Started.md ./Getting-Started.md
cp ../docs/Architecture.md ./Architecture.md
cp ../docs/Testing.md ./Testing.md
cp ../docs/DLP-Guide.md ./DLP-Guide.md

# Commit and push
git add .
git commit -m "Add wiki documentation"
git push origin master
```

### Step 4: Create Navigation

GitHub Wiki supports a **_Sidebar.md** file for navigation. Create it in the wiki:

1. Click **New Page**
2. Title: `_Sidebar`
3. Content:

```markdown
## Documentation

### Getting Started
- [[Getting Started|Getting-Started]]
- [[Installation & Prerequisites|Getting-Started#prerequisites]]

### Understanding the System
- [[Architecture Overview|Architecture]]
- [[Components|Architecture#components]]
- [[Data Flow|Architecture#data-flow]]

### Core Features
- [[Data Loss Prevention|DLP-Guide]]
- [[DLP Rules|DLP-Guide#detection-rules]]
- [[Backup System|DLP-Guide#backup-mechanism]]

### Development
- [[Testing Guide|Testing]]
- [[Contributing|https://github.com/quickattach0-tech/DeepLearningProtocol/blob/main/CONTRIBUTING.md]]

### Reference
- [[GitHub Repository|https://github.com/quickattach0-tech/DeepLearningProtocol]]
- [[Issues|https://github.com/quickattach0-tech/DeepLearningProtocol/issues]]
- [[Discussions|https://github.com/quickattach0-tech/DeepLearningProtocol/discussions]]
```

4. Click **Save Page**

### Step 5: Create Home Page

Edit the default Home page to contain:

```markdown
# Deep Learning Protocol Wiki

Welcome! Choose a topic below to get started.

## Quick Links

- **[Getting Started](Getting-Started)** — Setup in 5 minutes
- **[Architecture](Architecture)** — How the system works
- **[DLP Guide](DLP-Guide)** — Data protection explained
- **[Testing Guide](Testing)** — Test suite documentation

## For Different Roles

**I just want to use it:**
- [Getting Started](Getting-Started)
- [FAQ](../README.md#faq)

**I want to understand how it works:**
- [Architecture](Architecture)
- [DLP Guide](DLP-Guide)

**I want to contribute:**
- [Contributing Guide](../CONTRIBUTING.md)
- [Testing Guide](Testing)

---

See [Full Index](#page-index) below.
```

## Wiki Pages Included

### 1. Getting Started
- Installation instructions
- Prerequisites (.NET 10.0)
- Build & run commands
- VS Code setup
- FAQ for beginners

### 2. Architecture
- System overview with diagrams
- Component descriptions (7 classes)
- Data flow walkthrough
- Interface hierarchy
- Extension points

### 3. Testing
- Running tests (all, specific, coverage)
- Test suite overview (7 tests)
- Writing new tests
- XUnit assertions reference
- CI/CD integration
- Troubleshooting

### 4. DLP Guide
- What is DLP
- Detection rules (4 categories)
- Backup mechanism
- Real-world scenarios
- Configuration options
- FAQ

## Linking Between Pages

In GitHub Wiki, use this syntax:

```markdown
# Link to another page
[[Page Title|Page-Name]]

# Examples:
[[Getting Started|Getting-Started]]
[[Architecture Overview|Architecture]]
[[DLP Guide|DLP-Guide]]

# External links (normal markdown)
[GitHub Repository](https://github.com/quickattach0-tech/DeepLearningProtocol)
```

## Managing Wiki Content

### Editing Existing Pages

1. Click **Wiki** tab
2. Select page from list
3. Click **Edit** (pencil icon)
4. Update content
5. Click **Save Page**

### Deleting Pages

1. Click **Edit** on the page
2. Click **Delete this page** button at bottom
3. Confirm deletion

### Reordering Pages

GitHub Wiki doesn't have built-in reordering, but you can control order via **_Sidebar.md**.

## Best Practices

✅ **Do's:**
- Keep pages focused and concise
- Use clear headings and subheadings
- Include code examples
- Add links between related pages
- Keep wiki updated with code changes
- Use `_Sidebar.md` for navigation

❌ **Don'ts:**
- Don't duplicate README.md entirely
- Don't add outdated information
- Don't link to unstable/broken URLs
- Don't make wiki too deep (3 levels max)
- Don't ignore user feedback in Issues/Discussions

## Syncing with Repository

### Keep Wiki Updated

When making changes to code:

1. Update [Program.cs](../DeepLearningProtocol/Program.cs) 
2. Update related docs in `/docs` folder
3. Manually update corresponding Wiki pages
   - OR use Git method (Step 3B above)

### Two-Way Sync (Advanced)

```bash
# In GitHub Actions workflow (.github/workflows/dotnet.yml), add:
- name: Update Wiki
  run: |
    git clone https://github.com/${{ github.repository }}.wiki.git wiki
    cp docs/*.md wiki/
    cd wiki
    git config user.email "action@github.com"
    git config user.name "GitHub Action"
    git add .
    git commit -m "Auto-sync docs" || true
    git push
```

This automatically syncs `/docs` folder to Wiki on each push.

## Troubleshooting

### Wiki Tab Not Visible

**Problem:** Can't find Wiki tab  
**Solution:**
1. Go to Settings
2. Check **Wiki** is enabled in Features
3. Refresh page

### Changes Not Appearing

**Problem:** Updated page but changes don't show  
**Solution:**
1. Hard refresh browser: Ctrl+Shift+R
2. Try incognito/private window
3. Check you saved the page

### Can't Create Pages

**Problem:** "New Page" button disabled  
**Solution:**
1. Check repository access (need write permission)
2. Verify Wiki is enabled in Settings
3. Try via Git method instead

## Integrating with Project Docs

The project has three documentation layers:

1. **README.md** — Project overview & quick start
2. **`/docs` folder** — Detailed markdown guides
3. **GitHub Wiki** — Organized, web-based knowledge base

**Workflow:**
```
Code Change
    ↓
Update /docs/*.md (source of truth)
    ↓
Update README.md if user-facing
    ↓
Sync to GitHub Wiki via Git or manual copy
```

## Example: Adding a New Feature

1. Update [Program.cs](../DeepLearningProtocol/Program.cs)
2. Add tests to [DeepLearningProtocolTests.cs](../DeepLearningProtocol.Tests/DeepLearningProtocolTests.cs)
3. Update [Architecture.md](../docs/Architecture.md) with new component
4. Update [Testing.md](../docs/Testing.md) if needed
5. Update README.md if user-visible
6. Sync to Wiki:
   - Option A: Manual copy-paste to Wiki page
   - Option B: Use Git clone method (Step 3B)

## FAQ

**Q: Should I use Wiki or GitHub Pages?**  
A: Wiki is perfect for this project. GitHub Pages is better for static sites.

**Q: Can I version control wiki?**  
A: Yes! `git clone https://github.com/owner/repo.wiki.git` to work locally.

**Q: How do users access the wiki?**  
A: Click **Wiki** tab in repository, or direct URL: `github.com/owner/repo/wiki`

**Q: Can I embed images?**  
A: Yes! Upload to main repo (e.g., `/images/`), then link in wiki:
```markdown
![Alt text](../images/diagram.png)
```

**Q: Who can edit the wiki?**  
A: Anyone with repository access (controlled by repository permissions).

## Related Resources

- [GitHub Wiki Documentation](https://docs.github.com/en/communities/documenting-your-project-with-wikis)
- [Markdown Syntax](https://docs.github.com/en/get-started/writing-on-github/getting-started-with-writing-and-formatting-on-github)
- [GitHub Pages vs Wiki](https://docs.github.com/en/pages/getting-started-with-github-pages)

---

**Next Steps:**
1. Follow "Setup Steps" above
2. Create pages in GitHub Wiki
3. Update as project evolves
4. Link from README.md to Wiki

**Support:** Open [Issue](https://github.com/quickattach0-tech/DeepLearningProtocol/issues) if you need help!
