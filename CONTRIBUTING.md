# Contributing to ResearchAtlas

Thank you for your interest in contributing to ResearchAtlas. We welcome bug
reports, feature proposals, documentation improvements, tests, and code
changes.

## Before you begin

- Search the [open issues](https://github.com/researchatlas-network/researchatlas/issues) before creating a new one.
- Open an issue before starting substantial changes so that the proposal and
  its scope can be discussed.
- Do not report security vulnerabilities in public issues. Follow the
  [security policy](SECURITY.md) instead.

## Reporting issues

Use GitHub issues for reproducible bugs and well-defined feature requests. A
useful report includes:

- A clear description of the problem or proposed outcome.
- Steps to reproduce the issue, when applicable.
- Expected and actual behavior.
- Relevant environment details, logs, or screenshots, with credentials and
  other sensitive information removed.

## Submitting changes

1. Fork the repository and create a focused branch from the default branch.
2. Make the change while following the existing code style and project
   structure.
3. Add or update tests and documentation when they are relevant to the change.
4. Run the checks relevant to your changes.
5. Open a pull request against the default branch.

Keep pull requests small and focused. Avoid unrelated refactoring, formatting
changes, generated build artifacts, and secrets.

## Pull request guidelines

In the pull request description, explain:

- The problem being solved and the approach taken.
- Any issue the pull request addresses.
- The validation performed.
- Any configuration, migration, compatibility, or deployment considerations.

Pull requests should be ready for review, respond constructively to feedback,
and keep their scope limited to the stated purpose.

## Architecture guidelines

ResearchAtlas follows Clean Architecture and Domain-Driven Design. Keep
business rules and invariants in the Domain layer, use Application for
use-case orchestration, and isolate external concerns in Infrastructure
projects. Hosts should remain focused on configuration and composition.

## License

By contributing, you agree that your contributions may be distributed under
the terms of the [GNU Affero General Public License v3.0](LICENSE).
