# Agents and Skills Index

This file documents the available Claude Code agents and skills for this project.

## Available Agents

| Name                     | Path                            | Purpose                                                                              |
| ------------------------ | ------------------------------- | ------------------------------------------------------------------------------------ |
| test-quality-auditor     | .ai/agents/auditor/AGENT.MD     | Audits test suites across languages and produces a multi-dimensional quality report. |
| code-testing-builder     | .ai/agents/builder/AGENT.MD     | Runs build and compile commands and reports success or failure details.              |
| code-testing-fixer       | .ai/agents/fixer/AGENT.MD       | Fixes compilation and build errors in source or test files.                          |
| code-testing-generator   | .ai/agents/generator/SKILL.MD   | Orchestrates the research → plan → implement workflow for test generation.           |
| code-testing-implementer | .ai/agents/implementer/AGENT.MD | Implements a test plan phase by writing tests and verifying they compile and pass.   |
| code-testing-researcher  | .ai/agents/reseacher/AGENT.MD   | Analyzes codebases to understand structure, testing patterns, and testability.       |
| code-testing-linter      | .ai/agents/linter/SKILL.MD      | Formats and fixes lint or style issues in generated code.                            |
| code-testing-planner     | .ai/agents/planner/AGENT.MD     | Creates phased implementation plans for test generation work.                        |
| code-testing-tester      | .ai/agents/tester/AGENT.MD      | Runs tests and reports pass/fail results with details.                               |

## Available Skills

| Name                    | Path                                        | Purpose                                                                                              |
| ----------------------- | ------------------------------------------- | ---------------------------------------------------------------------------------------------------- |
| code-review             | .ai/skills/code-review/SKILL.MD             | Provides a C# / .NET 8 code review guide for modern language and framework best practices.           |
| code-testing            | .ai/skills/code-testing/SKILL.MD            | Generates unit tests and coordinates the associated research, planning, and implementation pipeline. |
| coverage-analysis       | .ai/skills/coverage-analysis/SKILL.MD       | Performs project-wide coverage and CRAP hotspot analysis for .NET projects.                          |
| crap-score              | .ai/skills/crap-score/SKLL.MD               | Calculates targeted CRAP scores for a specific .NET method, class, or file.                          |
| project-manager-backlog | .ai/skills/project-manager-backlog/SKILL.md | Manages backlog tasks using the backlog.md CLI workflow.                                             |
| run-tests               | .ai/skills/run-tests/SKILL.MD               | Runs and filters .NET tests with dotnet test.                                                        |
| web-api                 | .ai/skills/web-api/SKILL.MD                 | Guides ASP.NET Core Web API endpoint creation, OpenAPI metadata, and error handling.                 |

## Usage

Each agent or skill file contains:

- a `name`
- a `description`
- behavioral instructions or workflow guidance
- execution constraints

Agents and skills are intended to complement the main system prompt, not replace it.
