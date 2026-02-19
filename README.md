# Document Sorter (dcsrt)

PDF document classifier that automatically organizes documents based on keyword matching rules. Perfect for sorting scanned documents from scanners into organized folder structures.

## Features

- **Automatic Classification**: Matches PDF documents against keyword rules and moves them to destination folders
- **Training Mode**: Analyzes sample documents to generate optimal keyword vocabularies
- **Fuzzy Matching**: Optional spell-checking and typo tolerance for OCR'd documents
- **Dry Run Mode**: Preview classification results without moving files
- **Interactive Viewer**: Test rules against documents with real-time feedback

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
- `-i, --delete-invalid`: Auto-delete corrupted PDFs (default: false)
- `-x, --silently-exit`: Exit without waiting for input (default: false)

### Training Mode

Generate optimal keyword vocabularies from sample documents.

**Basic usage:**
```bash
dcsrt.exe training -s "C:\SampleDocuments"
```

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

## Matching Strategies

### StringSearch (Default)
- Fast exact keyword matching
- Case-insensitive
- Best for clean OCR or digital PDFs
- Use when: Documents have accurate text extraction

### Probabilistic
- Fuzzy matching with typo tolerance
- Slower but handles OCR errors
- Adjustable threshold (0-100)
- Use when: Scanned documents have OCR mistakes

## Tips

**Adjusting Match Sensitivity:**
- Lower `--match-percentage` (e.g., 50): Fewer keywords required, more matches
- Higher `--match-percentage` (e.g., 90): More keywords required, stricter matching

**Handling OCR Errors:**
- Use `-m Probabilistic` for fuzzy matching
- Increase `-t` (e.g., 95) for stricter typo tolerance
- Decrease `-t` (e.g., 85) to allow more variations

**Testing Rules:**
- Always use `--dry-run` first to preview results
- Use dcsrt-viewer.exe to test individual documents against rules
- Check logs for match percentages and rule collisions

**Rule Collisions:**
When multiple rules match, the tool chooses by:
1. Highest keyword count
2. Alphabetical RuleId

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
dcsrt.exe analysis -s "C:\Inbox" -m Probabilistic -t 90
```

**Generate keywords from sample medical statements:**
```bash
dcsrt.exe training -s "C:\Samples\Medical"
```

**Automated daily processing:**
```bash
dcsrt.exe analysis -s "C:\DailyScans" -x -i
```

## Troubleshooting

**No matches found:**
- Check keyword spelling in Codex.json
- Lower `--match-percentage` (try 50)
- Use training mode to generate better keywords

**Too many false matches:**
- Increase `--match-percentage` (try 80-90)
- Add more specific keywords to rules
- Check for rule collisions in logs

**Corrupted PDF errors:**
- Use `--delete-invalid` to auto-remove bad files
- Check scanner settings for better quality

## License

Copyright © 2019-2026 Jacob Marks
