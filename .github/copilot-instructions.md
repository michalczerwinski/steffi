# Copilot Instructions for Steffi

## Terminal Usage

- **Prefer built-in tools** (`get_file`, `create_file`, `replace_string_in_file`, `file_search`, `code_search`, etc.) over terminal commands whenever possible.

## Project Structure

- **Workspace root** (solution directory): `D:\Git\Private\steffi\src\`
- **Repository root**: `D:\Git\Private\steffi\` (one level above the workspace root)
- Files at the repo root (e.g., `readme.md`, `gallery.md`, `gallery/`, `scripts/`) are accessed via `..\ ` relative paths from the workspace root.

## Steffi DSL

- Steffi is a DSL for vector graphics. Source files use the `.stf` extension.
- The parser **does not support negative number literals** — all coordinate values must be >= 0.
- Comments use `/* block */` and `// line` syntax.
- For syntax highlighting in markdown, use ` ```css ``` ` fenced code blocks (best match for the `property: value;` style).
