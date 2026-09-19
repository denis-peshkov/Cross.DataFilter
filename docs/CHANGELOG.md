# Changelog — Cross.DataFilter

Newest releases first. Published versions: [GitHub Releases](https://github.com/denis-peshkov/Cross.DataFilter/releases).

Breaking upgrade notes for NuGet consumers: [`BREAKING.md`](BREAKING.md).

---

## v2.0.0 — 19 Sep 2026

### Dependencies

- Bumped Cross.CQRS to **12.0.0** (`PackageReference` + `config.nuspec`).
- Bumped `Microsoft.EntityFrameworkCore` to **8.0.31** in the library csproj (pack `config.nuspec` dependency still lists **8.0.8** until aligned).

### Breaking / API

- `PaginatedItemsQuery*` and `AutoCompleteQuery*` are `abstract record` (was `class`); host queries should be `sealed record`.
- Consumer notes: `docs/BREAKING.md` **From 1.0.1 to 2.0.0**.

### Versioning

- `GitVersion.yml` `next-version: 2.0.0` and branch-config alignment.

### Tests

- Renamed test project `Cross.DataFilter.UnitTests` → `Cross.DataFilter.Tests` with `TestCategory` (`Unit` / `Integration`) on each `[Test]`.
- `TestEntityPaginationQuery` is `sealed record`; test project PropertyGroup / JWT package alignment.

### Documentation

- Added `docs/BREAKING.md`, `docs/CHANGELOG.md`, `docs/RELEASE-PLAN-2.0.0.md`, `docs/TO-DO.md`, `CONTRIBUTING.md`.
- README: badges and Quick Start samples for the record-based API.
- Solution file: `.sln` → `.slnx`.

### Repository tooling

- Maintainer kit: `.cursor` rules/skills/triage automation.
- CI: `.github/workflows/dotnet.yml` updates for the release branch.

---

## v1.0.1 — 6 Jul 2025

### Packaging

- Small NuGet packaging fix ([PR #1](https://github.com/denis-peshkov/Cross.DataFilter/pull/1)).

---

## v1.0.0 — 6 Jul 2025

### Library

- Initial public release of Cross.DataFilter (pagination, sorting, autocomplete helpers for EF Core + Cross.CQRS).
