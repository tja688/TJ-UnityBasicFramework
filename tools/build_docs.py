from __future__ import annotations

import argparse
from collections import defaultdict
from pathlib import Path
from typing import Iterable, List

ROOT = Path(__file__).resolve().parents[1]
SOURCE_ROOT = ROOT / "Assets" / "Plugins" / "Feel"
ZH_ROOT = ROOT / "docs" / "zh" / "files"
EN_ROOT = ROOT / "docs" / "en" / "agent" / "files"
AGENT_ROOT = ROOT / "docs" / "en" / "agent"


def find_cs_files() -> List[Path]:
    if not SOURCE_ROOT.exists():
        return []
    return sorted(p for p in SOURCE_ROOT.rglob("*.cs") if p.is_file())


def write_file(path: Path, content: str) -> None:
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(content, encoding="utf-8")


def chinese_doc_content(rel_path: Path) -> str:
    file_title = rel_path.name
    source_line = f"- 源文件：{rel_path.as_posix()}"
    metaphor_lines = [f"- TODO: verify 形象解释占位 {i}" for i in range(1, 6)]
    sections = [
        f"# {file_title}",
        "", "## 文件信息", source_line,
        "", "## 主要职责", "- TODO: verify", "", "## 关键API", "- TODO: verify",
        "", "## 依赖关系", "- TODO: verify",
        "", "## 形象解释",
        *metaphor_lines,
    ]
    return "\n".join(sections) + "\n"


def agent_doc_content(rel_path: Path) -> str:
    file_title = rel_path.name
    sections = [
        f"# {file_title}",
        "- Source: {0}".format(rel_path.as_posix()),
        "- Role: TODO: verify",
        "- Key types: TODO: verify",
        "- Dependencies: TODO: verify",
        "- Notes: TODO: verify",
    ]
    return "\n".join(sections) + "\n"


def generate_per_file_docs(files: Iterable[Path]) -> None:
    for cs_file in files:
        rel = cs_file.relative_to(ROOT)
        zh_path = ZH_ROOT / (str(rel) + ".md")
        en_path = EN_ROOT / (str(rel) + ".md")
        write_file(zh_path, chinese_doc_content(rel))
        write_file(en_path, agent_doc_content(rel))


def generate_architecture(files: Iterable[Path]) -> None:
    counts = defaultdict(int)
    for cs_file in files:
        rel = cs_file.relative_to(SOURCE_ROOT)
        top = rel.parts[0] if rel.parts else "root"
        counts[top] += 1
    lines = ["# Architecture", "", f"- Scope root: {SOURCE_ROOT.as_posix()}", "", "## File counts by top-level folder:"]
    for key in sorted(counts):
        lines.append(f"- {key}: {counts[key]} C# files")
    lines.append("")
    write_file(AGENT_ROOT / "ARCHITECTURE.md", "\n".join(lines))


def generate_index(files: Iterable[Path]) -> None:
    lines = ["# Index", "", "## Per-file documentation", ""]
    for cs_file in files:
        rel = cs_file.relative_to(ROOT)
        zh_path = ZH_ROOT / (str(rel) + ".md")
        en_path = EN_ROOT / (str(rel) + ".md")
        lines.append(f"- {rel.as_posix()} -> zh: {zh_path.as_posix()}, en: {en_path.as_posix()}")
    lines.append("")
    write_file(AGENT_ROOT / "INDEX.md", "\n".join(lines))


def generate_coverage(files: Iterable[Path]) -> None:
    total = len(list(files))
    lines = ["# Coverage", "", f"- Total C# files in scope: {total}", "- Missing docs: 0 (all generated)", ""]
    write_file(AGENT_ROOT / "COVERAGE.md", "\n".join(lines))


def generate_all(files: Iterable[Path]) -> None:
    lines = ["# All Agent Docs", ""]
    for cs_file in files:
        rel = cs_file.relative_to(ROOT)
        agent_path = EN_ROOT / (str(rel) + ".md")
        lines.append(f"## {rel.as_posix()}")
        lines.append(agent_path.read_text(encoding="utf-8"))
    write_file(AGENT_ROOT / "ALL.md", "\n".join(lines))


def main() -> None:
    parser = argparse.ArgumentParser(description="Generate documentation stubs for Feel plugin.")
    parser.add_argument("--source-root", type=Path, default=SOURCE_ROOT, help="Root containing C# files to document.")
    args = parser.parse_args()
    source_root = args.source_root
    files = sorted(p for p in source_root.rglob("*.cs") if p.is_file())
    if not files:
        print("No C# files found under", source_root)
        return
    generate_per_file_docs(files)
    generate_architecture(files)
    generate_index(files)
    generate_coverage(files)
    generate_all(files)
    print(f"Generated docs for {len(files)} C# files under {source_root}")


if __name__ == "__main__":
    main()
