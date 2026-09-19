# Cross.DataFilter — open backlog (`TO-DO`)

Нерешённые пункты вне дельты version plan + кросс-версионные принятые trade-off’ы.

**Id high-water (не переиспользовать ≤):** `C0` `H0` `M0` `L0`

---

## Критично (безопасность)

---

## Высокий (логика / licensing / auth model)

---

## Средний (противоречия / баги контрактов)

---

## Низкий (техдолг / несогласованности)

---

## Принято (осознанный trade-off)

- Git tags: только stable SemVer с `master` / `release/*` / `hotfix/*` (никогда с `dev`; без pre-release suffix). NuGet push на `master`/`release/*`/`hotfix/*` (и при необходимости pre-release с других веток по CI).
- SemVer только через `GitVersion.yml` (без CI override / matrix fallback).
- Maintainer kit (`.cursor` rules/skills/triage) — часть репозитория, не NuGet.
- Канон release notes — `docs/CHANGELOG.md` (корневой `ReleaseNotes.md` не вести).
- CHANGELOG целевой версии — dated `## vX.Y.Z` до git tag / GitHub Release (не `Unreleased`).
- TFM библиотеки: `net8.0` до отдельного product decision о multi-targeting.
- Solution format: `Cross.DataFilter.slnx` (не `Cross.DataFilter.sln`).
- CI в этом репозитории сейчас — только `.github/workflows/dotnet.yml` (branch-policy / back-merge workflows пока нет).
- Лицензия исходников — RPL 1.5 (+ commercial option) в `LICENSE.md`; `config.nuspec` `license` должен совпадать (не MIT, если LICENSE — RPL).
