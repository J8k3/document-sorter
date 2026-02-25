# Document Sorter (dcsrt)

A PDF document classifier that automatically organizes your documents based on keyword matching rules. Great for sorting scanned documents from your scanner into organized folder structures.

## About This Project

I originally wrote this back in 2019 to deal with the mountain of scanned documents piling up from my scanner. It's been sitting in a private repo ever since, but I figured it might be useful to someone else dealing with the same problem.

Honestly, if I were starting this today, I'd probably just throw it at an LLM and let it figure out what each document is. But this was pre-ChatGPT days, and keyword matching actually works pretty well once you dial in your rules.

**Fair warning about training mode:** It only looks at documents you want to match (positive examples). It doesn't know what documents you DON'T want to match, so the keywords it suggests might be too generic. You'll probably need to manually tweak the rules to avoid false positives. The training output is a good starting point, not a finished product.

## Features

- **Automatic Classification**: Matches PDF documents against keyword rules and moves them to the right folders
- **Training Mode**: Analyzes sample documents to figure out the best keywords to use
- **Fuzzy Matching**: Handles typos and OCR errors in scanned documents
- **Dry Run Mode**: Preview what will happen before actually moving files
- **Interactive Viewer**: Test your rules against documents with real-time feedback

## Quick Start

### 1. Create a Codex File

Create `Codex.json` with your classification rules:

```json
{
  "RootPath": "C:\\DocumentSorter",
  "Dictionary": [
    {
      "RuleId": "BankStatement",
      "Words": ["bank", "statement", "balance"],
      "Destination": "Personal\\Finance\\Banking"
    },
    {
      "RuleId": "UtilityBill",
      "Words": ["account", "service", "due"],
      "Destination": "Personal\\Bills\\Utilities"
    }
  ]
}
```

### 2. Run Analysis

```bash
dcsrt.exe analysis -s "C:\Scanned" -c "Codex.json"
```

That's it! The tool will scan your PDFs, match them against your rules, and move them to the right folders.

## Command Reference

### Analysis Mode

Classify and organize PDF documents.

**Basic usage:**
```bash
dcsrt.exe analysis -s "C:\Scanned"
```

**Common options:**
```bash
# Dry run (preview without moving files)
dcsrt.exe analysis -s "C:\Scanned" -d

# Process only current folder (no subdirectories)
dcsrt.exe analysis -s "C:\Scanned" -r TopDirectoryOnly

# Custom codex file
dcsrt.exe analysis -s "C:\Scanned" -c "MyRules.json"

# Adjust match sensitivity (require 80% of keywords)
dcsrt.exe analysis -s "C:\Scanned" -p 80

# Use fuzzy matching for OCR errors
dcsrt.exe analysis -s "C:\Scanned" -m Probabilistic -t 92

# Silent mode for automation
dcsrt.exe analysis -s "C:\Scanned" -x
```

**All options:**
- `-s, --source-path` (required): Directory containing PDFs to process
- `-c, --codex-path`: Path to Codex JSON file (default: Codex.json)
- `-d, --dry-run`: Preview mode, don't move files (default: false)
- `-r, --recursive`: AllDirectories or TopDirectoryOnly (default: AllDirectories)
- `-p, --match-percentage`: Minimum keyword match % (default: 70)
- `-m, --matching-strategy`: StringSearch or Probabilistic (default: StringSearch)
- `-t, --matching-threshold`: Fuzzy match confidence 0-100 (default: 92)
- `-a, --pages-to-analyze`: Number of pages to extract per PDF (default: 1)
- `-n, --allow-multi-match`: Allow deterministic moves when multiple rules match (default: false)
- `-i, --delete-invalid`: Auto-delete corrupted PDFs (default: false)
- `-x, --silently-exit`: Exit without waiting for input (default: false)

### Training Mode

Generate optimal keyword vocabularies from sample documents.

**Basic usage:**
```bash
dcsrt.exe training -s "C:\SampleDocuments"
```

**Common options:**
```bash
# Adjust match percentage for testing vocabulary (default 70%)
dcsrt.exe training -s "C:\SampleDocuments" -p 50

# Analyze multiple pages per document
dcsrt.exe training -s "C:\SampleDocuments" -a 2
```

**All options:**
- `-s, --source-path` (required): Directory containing sample PDFs
- `-p, --match-percentage`: Minimum keyword match % for testing vocabulary (default: 70)
- `-a, --pages-to-analyze`: Number of pages to extract per PDF (default: 1)

**Example output:**
```
Frequency 90%: 12 words, 95.50% match rate
Frequency 85%: 15 words, 96.20% match rate
Frequency 80%: 18 words, 97.00% match rate

Optimal: 80% frequency → 18 words → 97.00% match rate
[
  "statement",
  "account",
  "balance",
  ...
]
```

Copy the generated keywords into your Codex.json rules.

## Codex File Format

```json
{
  "RootPath": "C:\\DocumentSorter",
  "Dictionary": [
    {
      "RuleId": "UniqueRuleName",
      "Words": ["keyword1", "keyword2", "keyword3"],
      "Destination": "Relative\\Path\\From\\RootPath"
    }
  ]
}
```

- **RootPath**: Base directory for all destinations
- **RuleId**: Unique identifier for the rule
- **Words**: Keywords to search for in documents (case-insensitive)
- **Destination**: Folder path relative to RootPath where matched files are moved

## Understanding the Parameters

### Match Percentage vs Matching Threshold

These two settings work together but control different things:

**Match Percentage** (`-p`, default 70%):
- Controls how many keywords from your rule need to be found
- Example: Rule has 10 keywords, 70% means at least 7 must be found
- Lower = more lenient (fewer keywords required)
- Higher = stricter (more keywords required)

**Matching Threshold** (`-t`, default 92%, only for Probabilistic mode):
- Controls how close a word needs to be to match (handles typos)
- Example: 92% means "bank" will match "bamk" if they're 92% similar
- Lower = more typos allowed (85% catches more variations)
- Higher = stricter matching (95% requires near-perfect spelling)

**Example:**
Rule: ["bank", "statement", "balance", "account"]

With `-p 75` (StringSearch):
- Document must contain at least 3 of the 4 keywords (exact matches)

With `-p 75 -m Probabilistic -t 92`:
- Document must contain at least 3 of the 4 keywords
- Each keyword can have typos if similarity is 92%+
- "bamk statemant balence" would match 3 keywords

## Matching Strategies

### StringSearch (Default)
- Fast exact keyword matching
- Case-insensitive
- Best for clean OCR or digital PDFs
- Use when your documents have accurate text extraction

### Probabilistic
- Fuzzy matching with typo tolerance
- Slower but handles OCR errors
- Adjustable threshold (0-100)
- Use when your scanned documents have OCR mistakes

## Tips

**Adjusting Match Sensitivity:**
- Lower `--match-percentage` (e.g., 50): Fewer keywords required, catches more documents
- Higher `--match-percentage` (e.g., 90): More keywords required, more precise matching

**Handling OCR Errors:**
- Use `-m Probabilistic` for fuzzy matching
- Increase `-t` (e.g., 95) for stricter typo tolerance
- Decrease `-t` (e.g., 85) to allow more variations

**Analyzing Multi-Page Documents:**
- Use `-a 2` or `-a 3` if keywords appear on later pages
- Default is 1 (first page only) for speed
- Higher values slow down processing

**Testing Rules:**
- Always use `--dry-run` first to preview what will happen
- Use dcsrt-viewer.exe to test individual documents
- Check the logs to see match percentages and rule collisions

**Rule Collisions:**
When multiple rules match the same document:
- By default, the file is skipped and a warning is logged
- Use `--allow-multi-match` to enable deterministic selection:
  1. Rule with most keywords wins
  2. If tied, alphabetically first RuleId wins
- Review your rules to make them more specific and avoid collisions

## Viewer Tool

Interactive GUI for testing rules:

```bash
dcsrt-viewer.exe
```

1. Load a PDF document
2. Paste a rule JSON in the right panel
3. Adjust matching threshold
4. See real-time match results (green = match, red = invalid JSON)

## Examples

**Sort scanned receipts:**
```bash
dcsrt.exe analysis -s "C:\Scanner\Receipts" -c "ReceiptRules.json" -d
```

**Process inbox folder with fuzzy matching:**
```bash
dcsrt.exe analysis -s "C:\Inbox" -m Probabilistic -t 90 -p 60
```

**Analyze first 3 pages of each document:**
```bash
dcsrt.exe analysis -s "C:\Scanned" -a 3
```

**Generate keywords from sample medical statements with lower match threshold:**
```bash
dcsrt.exe training -s "C:\Samples\Medical" -p 40
```

**Automated daily processing with multi-match handling:**
```bash
dcsrt.exe analysis -s "C:\DailyScans" -x -i -n
```

## Troubleshooting

**No matches found:**
- Check keyword spelling in Codex.json
- Lower `--match-percentage` (try 50 or 60)
- Use training mode to generate better keywords
- Try increasing `--pages-to-analyze` if keywords are on later pages

**Too many false matches:**
- Increase `--match-percentage` (try 80 or 90)
- Add more specific keywords to your rules
- Check the logs for rule collisions
- By default, files with multiple rule matches are skipped (use `--allow-multi-match` to override)

**Corrupted PDF errors:**
- Use `--delete-invalid` to auto-remove bad files
- Check scanner settings for better quality

**Training Mode Low Match Rate:**
- Lower `-p` value (try 30, 40, or 50) to see higher match rates
- Training tests vocabulary against sample documents using the match percentage
- If you get 46% match rate with `-p 70`, try `-p 40` to require fewer keywords per document
- Use smaller vocabularies (higher frequency thresholds like 90-95%) with higher match percentages
- Use larger vocabularies (lower frequency thresholds like 70-80%) with lower match percentages

## License

Copyright © 2019-2026 Jacob Marks
