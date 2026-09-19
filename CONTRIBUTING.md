# Contributing to Cross.DataFilter

Thank you for your interest in the project.

## Quick links

- [Report an issue](https://github.com/denis-peshkov/Cross.DataFilter/issues/new/choose)
- [Open PRs](https://github.com/denis-peshkov/Cross.DataFilter/pulls)
- [CI (.NET)](https://github.com/denis-peshkov/Cross.DataFilter/actions/workflows/dotnet.yml)
- [SonarCloud](https://sonarcloud.io/summary/new_code?id=Cross.DataFilter)
- [NuGet](https://www.nuget.org/packages/Cross.DataFilter/)
- [README](README.md)
- [Release notes](docs/CHANGELOG.md)
- Breaking changes: [`docs/BREAKING.md`](docs/BREAKING.md)
- Open backlog: [`docs/TO-DO.md`](docs/TO-DO.md)
- Sibling core library: [Cross.CQRS](https://github.com/denis-peshkov/Cross.CQRS)

---

## What is Cross.DataFilter?

**Cross.DataFilter** is a NuGet library for [Cross.CQRS](https://github.com/denis-peshkov/Cross.CQRS) + EF Core hosts:

- paginated queries (`PaginatedItemsQuery*`, `PaginatedResult`, sorting DTOs);
- autocomplete queries (`AutoCompleteQuery*`, result DTOs);
- attribute-driven sorting helpers and `IQueryable` extensions;
- CQRS-friendly handlers / validators for the above.

Supported TFM: **net8.0**.

Consumers send queries through MediatR (`IMediator` / `ISender`) after registering Cross.CQRS in the host.

---

## How you can help

| Type | Examples |
|---|---|
| **Report** | Bug with repro steps, expected/actual behavior, package version |
| **Fix** | Pagination/sort/autocomplete regression, validator bug, packing mismatch |
| **Build** | New filter/sort helper, tests, docs |
| **Review** | PR review, especially public query base types and packaging |
| **Document** | README, `docs/CHANGELOG.md`, `docs/BREAKING.md`, release plans |

---

## Development principles

### Public surface is a contract

`PaginatedItemsQuery*`, `AutoCompleteQuery*`, public DTOs/attributes/extensions, and documented host usage are contracts for NuGet consumers. Breaking changes require an entry in `docs/BREAKING.md` only (`config.nuspec` `releaseNotes` links there and must not duplicate the list).

### Minimal diff

Do not mix refactoring, formatting untouched files, and a feature in one PR. Drive-by changes belong in a separate PR.

### Repository conventions

- `.editorconfig` — style source (**UTF-8 BOM**, **LF**, 4 spaces for `.cs`).
- Prefer `GlobalUsings.cs` with `global using` directives (no local `using` in type files). Library and test projects use `ImplicitUsings` = `disable`.
- New `.cs` / `.csproj` / `.sln` / `.slnx` files — **UTF-8 with BOM**.
- Tests — **NUnit** + FluentAssertions (+ Moq/Bogus as needed); prefer method names `Given[X]_When[Y]_Then[Z]` (async → `…Async`); add/update tests with behavior changes.
- Library awaits: `ConfigureAwait(false)` where CA2007 applies; tests may differ (see `.editorconfig`).

---

## In scope / out of scope

### In scope

- `Cross.DataFilter/` — library (`Handlers/`, `Dtos/`, `Extensions/`, `Attributes/`, `Enums/`);
- `Cross.DataFilter.Tests/` — unit / integration tests;
- `README.md`, `docs/CHANGELOG.md`, `docs/BREAKING.md`, `Cross.DataFilter/config.nuspec`;
- CI: `.github/workflows/dotnet.yml`.

### Out of scope (without maintainer discussion)

- Changes that belong in **Cross.CQRS** core (`AddCQRS`, FluentValidation scan, core license behavior, request/result filters);
- Large architecture refactors “for aesthetics”;
- New external dependencies without a strong reason;
- Consumer-breaking changes without a `docs/BREAKING.md` entry;
- Secrets, keys, `.env` in commits.

---

## Branches and releases

```
feature/* ──┐
fix/*     ──┼── PR ──► dev ── merge ──► master ──► NuGet + git tag
chore/*   ──┘                              ▲
                                           │
                                 release/* / hotfix/* (owner only)
```

| Branch | Purpose | Who |
|---|---|---|
| `master` | Stable release; GitVersion, **stable** git tag (`vX.Y.Z`), NuGet push | **Owner only** — direct push and PRs |
| `release/*` | Release preparation; NuGet (may be `-preview.*`); **no** git tag for pre-releases | **Owner only** — branch creation and push |
| `hotfix/*` | Urgent production patches; same tag/NuGet rules as `release/*` | **Owner only** — branch creation and push |
| `dev` | Feature integration; NuGet pre-release (`-dev.*`); **never** creates git tags | **Default PR target** for all contributors |
| `feature/*` | New functionality (build/test only — no tag/NuGet) | Contributors |
| `fix/*` | Bug fixes (build/test only — no tag/NuGet) | Contributors |
| `chore/*` | CI, deps, docs-only, maintenance (no tag/NuGet) | Contributors |

Git tags are created only for **stable** `X.Y.Z` (no `-` in `semVer`) on `master` / `release/*` / `hotfix/*`. **`dev` never creates git tags**; it may still push NuGet pre-release packages.

**Access rules (project policy):**

- Contributors open PRs **only into `dev`** from `feature/*`, `fix/*`, or `chore/*`.
- PRs targeting **`master`** — repository owner only (`denis-peshkov`).
- Pushing to **`master`**, **`release/*`**, or **`hotfix/*`** — owner only.
- Release merge `dev` → `master`, tags, and NuGet publish — maintainer step after the release checklist.
- CI today: `.github/workflows/dotnet.yml` (build, test, SonarCloud, pack, tag on `master`/`release/*`/`hotfix/*`, NuGet push). Branch-policy / back-merge workflows are not present in this repository yet; follow the table above anyway.

Versioning: **GitVersion** (`GitVersion.yml`). `dev` is pre-release (`-dev.N`), not a release branch.

---

## Branch naming

Prefix + kebab-case description:

| Prefix | When |
|---|---|
| `feature/` | New functionality |
| `fix/` | Bug fix |
| `chore/` | CI, deps, docs-only, maintenance |
| `release/` | Release preparation (owner) |
| `hotfix/` | Urgent production patch (owner) |

Examples:

```
release/2.0.0-cross-cqrs-12
hotfix/nuget-pack-path
feature/nested-property-sorting
fix/pagination-total-count
chore/editorconfig-and-docs
```

---

## Commit messages

Use **clear English** messages in imperative/descriptive style:

```
Add nested property sorting tests
Fix PaginatedItemsQuery validator for empty page size
Update README Quick Start for sealed record queries
```

For breaking changes, explicitly include `BREAKING:` in the commit body or PR title/description.

`docs/CHANGELOG.md` is maintained by maintainers before release (not a contributor checklist item).

---

## Pull request process

### 1. Preparation

```bash
git checkout dev
git pull origin dev
git checkout -b feature/short-description
```

### 2. Changes

- Follow existing folder layout (`Handlers/`, `Dtos/`, `Extensions/`, `Attributes/`, `Enums/`, …).
- Do not touch unrelated files.
- Breaking change → `docs/BREAKING.md` only (nuspec keeps a link, not a duplicate list).

### 3. Tests (required)

See [Testing](#testing).

### 4. Open PR

- **Base branch:** `dev` (required for contributors)
- **Do not** open PRs into `master`, `release/*`, or `hotfix/*` unless you are the repository owner
- Description: what, why, how to verify (**English** — for GitHub history)
- Breaking consumer change → prefix the **PR title** with `BREAKING:`

### 5. CI

Must pass:

- `.NET` workflow (`dotnet build` + `dotnet test` on `Cross.DataFilter.slnx`)
- SonarCloud analysis when enabled on the workflow

### 6. Review and merge

After approval — merge into `dev`. Release to `master` and NuGet publish is a separate maintainer step.

### One PR rule

**One PR = one feature or one fix.** Split large changes (library + tests → docs → CI).

---

## Testing

### Local run

```bash
dotnet build Cross.DataFilter.slnx
dotnet test Cross.DataFilter.Tests/Cross.DataFilter.Tests.csproj
```

With coverage (OpenCover), as in CI:

```bash
dotnet test Cross.DataFilter.Tests/Cross.DataFilter.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory ./TestResults \
  -- DataCollectionRunSettings.DataCollectors.DataCollector.Configuration.Format=opencover
```

### Pre-PR checklist

- [ ] Tests added/updated for changed behavior (`Given[X]_When[Y]_Then[Z]`; async → `Async`)
- [ ] `dotnet build` / `dotnet test` — green locally
- [ ] No secrets in code or test data
- [ ] README / `docs/BREAKING.md` / `Cross.DataFilter/config.nuspec` updated when the public surface or packaging changes

---

## Documentation

| What changed | Update |
|---|---|
| Public API / host usage | `README.md` |
| Breaking change for consumers | `docs/BREAKING.md` only (`config.nuspec` `releaseNotes` = link, no duplicate list) |
| Released behavior | **`docs/CHANGELOG.md` (maintainers, on release work)** and short `config.nuspec` `releaseNotes` (+ link to BREAKING) |
| Packaging / dependencies | `Cross.DataFilter/config.nuspec` |
| Release readiness | `docs/RELEASE-PLAN-*.md` |
| Deferred findings | `docs/TO-DO.md` |

---

## License

Code is under [RPL 1.5](LICENSE.md) (Reciprocal Public License). By contributing, you agree that derivative works are distributed under the same terms, or under a [Peshkov commercial license](https://peshkov.biz/license).

There is no separate CLA — merging a PR means agreement with the repository license.

---

## Questions?

- Bugs and features: [GitHub Issues](https://github.com/denis-peshkov/Cross.DataFilter/issues)
- Core CQRS / `AddCQRS` issues: [Cross.CQRS](https://github.com/denis-peshkov/Cross.CQRS/issues)

**Thank you for contributing to Cross.DataFilter.**
