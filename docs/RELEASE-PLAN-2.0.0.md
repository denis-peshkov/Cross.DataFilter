Ниже — **проблемы внутри библиотеки**, по уровню критичности. Аудит по дельте ветки относительно базовой ветки (обычно `master`).

> **Версия:** `2.0.0` · **ветка:** `release/cross-cqrs-12` · **база:** `origin/master` (`v1.0.1`) · **дата:** `2026-09-19`
>
> **Релиз (если есть):** https://github.com/denis-peshkov/Cross.DataFilter/releases/tag/v2.0.0
>
> **Легенда:** ⬜ open · ✅ done · 🟨 partial / принято · ❌ blocker
>
> **Предыдущий план:** —
>
> Дельта: `origin/master...HEAD` — **2** коммита · **77** файлов · **+7910 / −48**. Open: C0 H1 M1 L1.

**CodeRabbit:**
- не запускался.

**PR:** —

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

### ⬜ H1. nuspec EF Core отстаёт от csproj

`Cross.DataFilter/config.nuspec` держит `Microsoft.EntityFrameworkCore` **8.0.8**, а `Cross.DataFilter.csproj` — **8.0.31**. NuGet-группа должна совпадать с PackageReference (правило multi-targeting / nuspec).

---

## Средний (противоречия / баги контрактов)

### ⬜ M1. Мёртвые `_REMOVE_Pagination` в дереве библиотеки

В `Cross.DataFilter/_REMOVE_Pagination/` лежат полностью закомментированные `QueryableExtensions` / `ServiceCollectionExtensions`. Для релиза их нужно убрать из product tree (не ship placeholder’ы).

---

## Низкий (техдолг / несогласованности)

### ⬜ L1. LICENSE path / badge

`LICENSE` удалён, добавлен `LICENSE.md`; README badge всё ещё указывает на `LICENSE`. Выровнять путь (файл или ссылка), чтобы license detection / badge не ломались.

---

## Принято (осознанный trade-off)

- SemVer только через `GitVersion.yml` (`next-version: 2.0.0`; digits в имени `release/*` игнорируются).
- Query base types — `abstract record` (`PaginatedItemsQuery*`, `AutoCompleteQuery*`) под Cross.CQRS **12** / MediatR-friendly records.
- Maintainer kit (`.cursor` rules/skills/triage, CONTRIBUTING) — часть репозитория, не NuGet.
- CHANGELOG целевой версии — dated `## vX.Y.Z` до git tag / GitHub Release (не `Unreleased`).
- TFM библиотеки: `net8.0` (как в 1.x); multi-TFM не в этом релизе.

---

## Закрыто (проверено в коде)

| # | Суть |
|---|---|
| ✅ Cross.CQRS 12.0.0 | PackageReference + nuspec: `10.1.3` → `12.0.0` |
| ✅ Query types → record | `PaginatedItemsQuery` / `AutoCompleteQuery` (`class` → `abstract record`); tests/README — `sealed record` |
| ✅ EF Core csproj bump | `Microsoft.EntityFrameworkCore` `8.0.8` → `8.0.31` в csproj (nuspec — open H1) |
| ✅ GitVersion 2.0.0 | `next-version: 2.0.0`; branch config alignment |
| ✅ Maintainer kit | `.cursor` rules/skills/triage + CI workflow tweaks |
| ✅ README / samples | badges; Quick Start constructors под новый API |
| ✅ Consumer docs rewrite | `BREAKING` / `CHANGELOG` / `TO-DO` / version plan переписаны под Cross.DataFilter (убраны чужие Cross.CQRS.EF 9.x/10.x) |
| ✅ LICENSE.md | `LICENSE` → `LICENSE.md` (+ CONTRIBUTING) |

---

## Что в библиотеке уже нормально

- Публичный фокус пакета по-прежнему pagination / sorting / autocomplete поверх EF Core + Cross.CQRS.
- `PaginatedItemsQuery` / `AutoCompleteQuery` остаются entry-point’ами для host queries.
- Tests наследуют новые record-типы (`TestEntityPaginationQuery`).
- Pack по-прежнему через `Cross.DataFilter/config.nuspec` + CI.

---

## Приоритет фиксов

1. **H1** — выровнять EF Core в `config.nuspec` с csproj (**8.0.31**).
2. **M1** — удалить `Cross.DataFilter/_REMOVE_Pagination/`.
3. **L1** — починить LICENSE path / README badge.

Открытый backlog вне дельты → [`TO-DO.md`](TO-DO.md).
