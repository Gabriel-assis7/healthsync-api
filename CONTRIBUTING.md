# Contributing to HealthSync API

Thank you for your interest in contributing to HealthSync API! This document provides guidelines for contributing to the project.

## Table of Contents

- [Getting Started](#getting-started)
- [Git Flow](#git-flow)
- [Conventional Commits](#conventional-commits)
- [Creating Issues](#creating-issues)
- [Creating Pull Requests](#creating-pull-requests)

## Getting Started

1. **Fork the repository** and clone it locally
2. **Create a branch** following the Git Flow model (see below)
3. **Make your changes** and commit following Conventional Commits (see below)
4. **Push to your fork** and submit a pull request to the appropriate branch

## Git Flow

This project follows the Git Flow branching model. Here are the main branches:

### Main Branches

- **`master`** - Production-ready code. Only updated via release branches.
- **`dev`** - Development branch. Integration branch for features.

### Supporting Branches

- **`feature/*`** - Feature branches
  - Branch from: `dev`
  - Example: `feature/add-redis-extensions`
- **`bugfix/*`** - Bug fix branches
  - Branch from: `dev`
  - Example: `bugfix/fix-connection-pool`

- **`release/*`** - Release preparation branches
  - Branch from: `dev`
  - Example: `release/v1.0.0`

- **`hotfix/*`** - Critical production fixes
  - Branch from: `master`
  - Example: `hotfix/security-patch`

### Workflow Example

```bash
# Create a feature branch
git checkout -b feature/my-feature dev

# Make your changes and commit
# Push and create a PR to dev

# For releases, create a release branch
git checkout -b release/v1.0.0 dev

# For hotfixes, create a hotfix branch
git checkout -b hotfix/critical-fix master
```

## Conventional Commits

This project uses the Conventional Commits specification. All commits must follow this format:

```
<type>(<scope>): <subject>

<body>

<footer>
```

### Types

- **feat**: A new feature
- **fix**: A bug fix
- **docs**: Documentation changes
- **style**: Code style changes (formatting, semicolons, etc.)
- **refactor**: Code refactoring without feature or bug fix
- **perf**: Performance improvements
- **test**: Adding or updating tests
- **chore**: Build process, dependencies, or tooling changes
- **ci**: CI/CD configuration changes

### Examples

```
feat(auth): add JWT token validation

Implement JWT token validation for incoming requests.
Adds middleware to verify token signatures and expiration.

Closes #123
```

```
fix(redis): resolve connection timeout issue

- Increase connection pool size from 10 to 50
- Add retry logic with exponential backoff
- Update timeout configuration

Fixes #456
```

## Creating Issues

When creating an issue, follow these guidelines:

### Issue Template

**Title**: Clear, concise description of the problem or feature

**Description**:

1. **Problem Statement** - What is the issue or what feature is needed?
2. **Expected Behavior** - What should happen?
3. **Actual Behavior** (for bugs) - What is currently happening?
4. **Steps to Reproduce** (for bugs) - How can we reproduce this?
5. **Environment** - OS, .NET version, relevant dependencies
6. **Screenshots** (if applicable) - Visual reference

### Example Issue

```
Title: Authentication endpoint returns 500 error with invalid credentials

Problem Statement:
When passing invalid credentials to the login endpoint, the API returns
a 500 Internal Server Error instead of a 401 Unauthorized response.

Expected Behavior:
The endpoint should return a 401 Unauthorized status with a meaningful
error message when credentials are invalid.

Actual Behavior:
API returns HTTP 500 with a generic error message in the response.

Steps to Reproduce:
1. POST to /api/auth/login with { "username": "user", "password": "wrong" }
2. Observe the response status and body

Environment:
- .NET 8.0
- HealthSync API v1.2.0
```

## Creating Pull Requests

When submitting a pull request, follow these guidelines:

### PR Requirements

- [ ] Branch is based on the correct base branch (`dev` for features/bugfixes, `master` for hotfixes)
- [ ] Commits follow Conventional Commits specification
- [ ] Code follows project style guidelines
- [ ] Tests have been added/updated (if applicable)
- [ ] Documentation has been updated (if applicable)
- [ ] No breaking changes (unless discussed and approved)

### PR Template

**Title**: Use a Conventional Commit format as the title

**Description**:

```
## Summary
Brief description of the changes

## Type of Change
- [ ] Feature
- [ ] Bug Fix
- [ ] Documentation
- [ ] Other (please describe)

## Changes Made
- List item 1
- List item 2
- List item 3

## How to Test
Steps to verify the changes:
1. Step 1
2. Step 2
3. Expected result

## Related Issues
Closes #123

## Breaking Changes
Yes / No
If yes, describe the breaking changes and migration path

## Screenshots (if applicable)
Add any relevant screenshots
```

### PR Guidelines

1. **Keep PRs focused** - One feature or fix per PR when possible
2. **Keep PRs small** - Easier to review and merge
3. **Provide context** - Explain the why, not just the what
4. **Link related issues** - Use `Closes #issue-number` in the PR description
5. **Request reviews** - Ask team members for code review
6. **Address feedback** - Respond to review comments promptly
7. **Keep branch up to date** - Rebase on the base branch if needed

### Code Review Expectations

- All PRs require at least one approval before merging
- Tests must pass
- No unresolved conversations
- Code must follow project conventions

## Questions or Need Help?

- Check existing issues and PRs for similar topics
- Start a discussion if you have questions
- Reach out to the maintainers

---

Thank you for contributing to HealthSync API!
