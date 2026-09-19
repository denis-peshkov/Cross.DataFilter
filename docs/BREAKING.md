# Breaking changes (NuGet consumers)

Breaking changes for **Cross.DataFilter**, grouped by **from → to** package version.
Sections are **newest first** (top) → **oldest last** (bottom). When skipping releases, apply every intervening section **from oldest to newest** (bottom-up through the relevant range).

| Upgrade path | Section |
|---|---|
| `1.0.1` → `2.0.0+` | [From 1.0.1 to 2.0.0](#from-101-to-200) |

Breaking-change details live **only** in this file. [`Cross.DataFilter/config.nuspec`](../Cross.DataFilter/config.nuspec) `releaseNotes` should link here and must not duplicate the versioned sections.

When shipping a new breaking change: insert a **From X.Y.Z to A.B.C** section **at the top** of the versioned sections (and a matching TOC row), and prefix the **PR title** with `BREAKING:`.

---

## From 1.0.1 to 2.0.0

Release: [v2.0.0](https://github.com/denis-peshkov/Cross.DataFilter/releases/tag/v2.0.0).

### Core dependency

| Area | Was (1.0.x) | Now (2.0.0) |
|---|---|---|
| Cross.CQRS | **10.1.3** | **12.0.0** (csproj + `config.nuspec`) |

**Action:** upgrade the host to Cross.CQRS **12.0.0** before restoring this package.

### Query base types

| Area | Was (1.0.x) | Now (2.0.0) |
|---|---|---|
| `PaginatedItemsQuery*` | `abstract class` | `abstract record` |
| `AutoCompleteQuery` | `abstract class` | `abstract record` |
| `AutoCompleteQuery<TFilter>` | `class` | `abstract record` |

**Action:** change host query types that inherit these bases to `record` (typically `sealed record`) and keep constructor forwarding to `(page, pageSize, sorting, filter)` / `(page, pageSize, filter)`.

### EF Core package

| Area | Was (1.0.x) | Now (2.0.0) |
|---|---|---|
| `Microsoft.EntityFrameworkCore` (csproj) | **8.0.8** | **8.0.31** |
| `Microsoft.EntityFrameworkCore` (`config.nuspec` dependency) | **8.0.8** | **8.0.8** (still; align with csproj before publish) |

**Action:** restore against a compatible EF Core **8.0.x**. Prefer aligning the host with **8.0.31** when consuming a build compiled against that patch. Treat the nuspec dependency as the published lower bound until it is bumped.

### License file

| Area | Was (1.0.x) | Now (2.0.0) |
|---|---|---|
| License file | `LICENSE` | `LICENSE.md` (RPL 1.5 + commercial option) |

**Action:** update links that pointed at `LICENSE`. Package `license` metadata in `config.nuspec` must match `LICENSE.md` (do not ship `MIT` if the repo license is RPL 1.5).
