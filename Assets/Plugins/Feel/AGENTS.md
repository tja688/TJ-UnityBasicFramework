# AGENTS.md — Repository Documentation Rules for Codex

This repo contains a Unity plugin with many C# files. Your job is to generate structured documentation for *each* C# file and also produce an English condensed “agent pack” for downstream code writing.

## 0) Non-negotiables

- Do NOT change production code behavior.
- Prefer *adding* docs and helper scripts only.
- Never delete existing files unless explicitly asked.
- If something is ambiguous, write it down as **TODO / Unknown** rather than guessing.
- Keep docs deterministic: no fluff, no speculation.

## 1) Deliverables (files to create/update)

You must generate two per-file docs for each C# script:

### A) Human Chinese doc (per file)
- Path: `docs/zh/files/<RELATIVE_PATH>.md`
- Language: **Chinese**
- Audience:
